using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using uBeac.Common;
using uBeac.Common.Identity;
using uBeac.Identity.Repositories.Abstractions;
using uBeac.Repositories.MongoDB;

namespace uBeac.Identity.Repositories.MongoDB
{
    public class UserRepository<TKey, TUser, TRole> : EntityGenericRepository<TKey, TUser>, IUserRepository<TKey, TUser, TRole>
         where TKey : IEquatable<TKey>
        where TUser : BaseIdentityUser<TKey>
        where TRole : BaseIdentityRole<TKey>
    {

        private readonly IRoleRepository<TKey, TRole> _roleRepository;
        private readonly ILookupNormalizer _normalizer;

        protected override string CollectionName => "Users";

        public UserRepository(AuthMongoDbContext dbContext, IRoleRepository<TKey, TRole> roleRepository, ILookupNormalizer normalizer) : base(dbContext)
        {
            dbContext.ThrowIfNull();
            roleRepository.ThrowIfNull();
            normalizer.ThrowIfNull();

            _roleRepository = roleRepository;
            _normalizer = normalizer;

            EnsureIndex(x => x.NormalizedEmail);
            EnsureIndex(x => x.NormalizedUserName);

        }

        private void EnsureIndex(Expression<Func<TUser, object>> field)
        {
            var model = new CreateIndexModel<TUser>(Builders<TUser>.IndexKeys.Ascending(field));
            Collection.Indexes.CreateOne(model);
        }

        private async Task AddAsync<TFieldValue>(TUser user, Expression<Func<TUser, IEnumerable<TFieldValue>>> expression, TFieldValue value, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var addDefinition = Builders<TUser>.Update.AddToSet(expression, value);

            await Collection.UpdateOneAsync(x => x.Id.Equals(user.Id), addDefinition, cancellationToken: cancellationToken);
        }

        private async Task UpdateAsync<TFieldValue>(TUser user, Expression<Func<TUser, TFieldValue>> expression, TFieldValue value, CancellationToken cancellationToken)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var updateDefinition = Builders<TUser>.Update.Set(expression, value);

            await Collection.UpdateOneAsync(x => x.Id.Equals(user.Id), updateDefinition, cancellationToken: cancellationToken);
        }

        public IQueryable<TUser> Users => Collection.AsQueryable();

        public async Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            claims.ThrowIfNull();

            cancellationToken.ThrowIfCancellationRequested();

            foreach (var claim in claims)
            {
                var identityClaim = new IdentityUserClaim<TKey>()
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                };

                user.Claims.Add(identityClaim);

                await AddAsync(user, x => x.Claims, identityClaim, cancellationToken);
            }
        }

        public async Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            login.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var userLogin = new IdentityUserLogin<TKey>
            {
                UserId = user.Id,
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName,
                ProviderKey = login.ProviderKey
            };

            user.Logins.Add(userLogin);

            await AddAsync(user, x => x.Logins, userLogin, cancellationToken);
        }

        public async Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            roleName.ThrowIfNull();

            cancellationToken.ThrowIfCancellationRequested();

            var role = await _roleRepository.FindByNameAsync(roleName, cancellationToken);
            if (role == null)
                return;

            user.Roles.Add(role.Id);

            await UpdateAsync(user, x => x.Roles, user.Roles, cancellationToken);
        }

        public async Task<int> CountCodesAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var foundUser = await GetById(user.Id, cancellationToken);

            return foundUser?.RecoveryCodes?.Count ?? user.RecoveryCodes.Count;
        }

        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var u = (await Collection.FindAsync(x => x.UserName == user.UserName, cancellationToken: cancellationToken)).SingleOrDefault(cancellationToken);

            if (u != null)
                return IdentityResult.Failed(new IdentityError { Code = "Username already exists!" });

            await Insert(user, cancellationToken);

            if (user.Email != null)
                await SetEmailAsync(user, user.Email, cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            await Delete(user.Id, cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            normalizedEmail.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await Collection.FindAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken: cancellationToken)).SingleOrDefault(cancellationToken);
        }

        public Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            userId.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var id = (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(userId);

            return GetById(id, cancellationToken);
        }

        public async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            loginProvider.ThrowIfNull();
            providerKey.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await Collection.FindAsync(u => u.Logins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey), cancellationToken: cancellationToken)).SingleOrDefault(cancellationToken);
        }

        public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            normalizedUserName.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await Collection.FindAsync(x => x.NormalizedUserName == normalizedUserName, cancellationToken: cancellationToken)).FirstOrDefault(cancellationToken);
        }

        public async Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await GetById(user.Id, cancellationToken))?.AccessFailedCount ?? user.AccessFailedCount;
        }

        public async Task<string> GetAuthenticatorKeyAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await GetById(user.Id, cancellationToken))?.AuthenticatorKey ?? user.AuthenticatorKey;
        }

        public async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var dbUser = await GetById(user.Id, cancellationToken);
            return dbUser?.Claims?.Select(x => new Claim(x.ClaimType, x.ClaimValue))?.ToList() ?? new List<Claim>();
        }

        public async Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await GetById(user.Id, cancellationToken))?.Email ?? user.Email;
        }

        public async Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await GetById(user.Id, cancellationToken))?.EmailConfirmed ?? user.EmailConfirmed;
        }

        public async Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await GetById(user.Id, cancellationToken))?.LockoutEnabled ?? user.LockoutEnabled;
        }

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await GetById(user.Id, cancellationToken))?.LockoutEnd ?? user.LockoutEnd;
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var dbUser = await GetById(user.Id, cancellationToken);

            return dbUser?.Logins?.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey, x.ProviderDisplayName))?.ToList() ?? new List<UserLoginInfo>();
        }

        public async Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await GetById(user.Id, cancellationToken))?.NormalizedEmail ?? user.NormalizedEmail;
        }

        public async Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await GetById(user.Id, cancellationToken))?.NormalizedUserName ?? user.NormalizedUserName;
        }

        public async Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await GetById(user.Id, cancellationToken))?.PasswordHash ?? user.PasswordHash;
        }

        public async Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await GetById(user.Id, cancellationToken))?.PhoneNumber ?? user.PhoneNumber;
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await GetById(user.Id, cancellationToken))?.PhoneNumberConfirmed ?? user.PhoneNumberConfirmed;
        }

        public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var userDb = await GetById(user.Id, cancellationToken);

            if (userDb == null)
                return new List<string>();

            var roles = new List<string>();

            foreach (var roleId in userDb.Roles)
            {
                var dbRole = await _roleRepository.FindByIdAsync(roleId.ToString(), cancellationToken);

                if (dbRole != null)
                    roles.Add(dbRole.Name);

            }
            return roles;
        }

        public async Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await GetById(user.Id, cancellationToken))?.SecurityStamp ?? user.SecurityStamp;
        }

        public async Task<string> GetTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var token = user?.Tokens?.FirstOrDefault(x => x.LoginProvider == loginProvider && x.Name == name);

            if (token == null)
            {
                user = await GetById(user.Id, cancellationToken);
                return user?.Tokens?.FirstOrDefault(x => x.LoginProvider == loginProvider && x.Name == name)?.Value;
            }

            return token.Value;
        }

        public async Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await GetById(user.Id, cancellationToken))?.TwoFactorEnabled ?? user.TwoFactorEnabled;
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.UserName.ToString());
        }

        public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            claim.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await Collection.FindAsync(u => u.Claims.Any(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value), cancellationToken: cancellationToken)).ToList(cancellationToken);
        }

        public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            roleName.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var role = await _roleRepository.FindByNameAsync(roleName, cancellationToken);

            if (role == null)
                return new List<TUser>();

            var filter = Builders<TUser>.Filter.AnyEq(x => x.Roles, role.Id);

            return (await Collection.FindAsync(filter, cancellationToken: cancellationToken)).ToList(cancellationToken);
        }

        public async Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await GetById(user.Id, cancellationToken))?.PasswordHash != null;
        }

        public async Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            user.AccessFailedCount++;
            await UpdateAsync(user, x => x.AccessFailedCount, user.AccessFailedCount, cancellationToken);
            return user.AccessFailedCount;
        }

        public async Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            roleName.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var dbUser = await GetById(user.Id, cancellationToken);

            var role = await _roleRepository.FindByNameAsync(roleName, cancellationToken);

            if (role == null)
                return false;

            return dbUser?.Roles.Contains(role.Id) ?? false;
        }

        public async Task<bool> RedeemCodeAsync(TUser user, string code, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            code.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var dbUser = await GetById(user.Id, cancellationToken);

            if (dbUser == null)
                return false;

            var c = user.RecoveryCodes.FirstOrDefault(x => x.Code == code);

            if (c == null || c.Redeemed)
                return false;

            c.Redeemed = true;

            await UpdateAsync(user, x => x.RecoveryCodes, user.RecoveryCodes, cancellationToken);

            return true;
        }

        public Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            claims.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            cancellationToken.ThrowIfCancellationRequested();

            foreach (var claim in claims)
                user.Claims.RemoveAll(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);

            return UpdateAsync(user, x => x.Claims, user.Claims, cancellationToken);
        }

        public async Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            roleName.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            cancellationToken.ThrowIfCancellationRequested();

            var role = await _roleRepository.FindByNameAsync(roleName, cancellationToken);

            if (role == null) return;

            user.Roles.Remove(role.Id);

            await UpdateAsync(user, x => x.Roles, user.Roles, cancellationToken);
        }

        public async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            loginProvider.ThrowIfNull();
            providerKey.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            user.Logins.RemoveAll(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);

            await UpdateAsync(user, x => x.Logins, user.Logins, cancellationToken);
        }

        public async Task RemoveTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            loginProvider.ThrowIfNull();
            name.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var userTokens = user.Tokens ?? new List<IdentityUserToken<TKey>>();
            userTokens.RemoveAll(x => x.LoginProvider == loginProvider && x.Name == name);
            await UpdateAsync(user, x => x.Tokens, userTokens, cancellationToken);
        }

        public async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            claim.ThrowIfNull();
            newClaim.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            cancellationToken.ThrowIfCancellationRequested();

            user.Claims.RemoveAll(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            user.Claims.Add(new IdentityUserClaim<TKey>()
            {
                ClaimType = newClaim.Type,
                ClaimValue = newClaim.Value
            });

            await UpdateAsync(user, x => x.Claims, user.Claims, cancellationToken);
        }

        public Task ReplaceCodesAsync(TUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            recoveryCodes.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            user.RecoveryCodes = recoveryCodes.Select(x => new TwoFactorRecoveryCode { Code = x, Redeemed = false }).ToList();

            return UpdateAsync(user, x => x.RecoveryCodes, user.RecoveryCodes, cancellationToken);
        }

        public async Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            user.AccessFailedCount = 0;
            await UpdateAsync(user, x => x.AccessFailedCount, 0, cancellationToken);
        }

        public async Task SetAuthenticatorKeyAsync(TUser user, string key, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            key.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            user.AuthenticatorKey = key;

            await UpdateAsync(user, x => x.AuthenticatorKey, key, cancellationToken);
        }

        public async Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            email.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            await SetNormalizedEmailAsync(user, _normalizer.NormalizeEmail(user.Email), cancellationToken);

            user.Email = email;

            await UpdateAsync(user, x => x.Email, user.Email, cancellationToken);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            user.EmailConfirmed = confirmed;

            return UpdateAsync(user, x => x.EmailConfirmed, confirmed, cancellationToken);
        }

        public async Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            user.LockoutEnabled = enabled;
            await UpdateAsync(user, x => x.LockoutEnabled, user.LockoutEnabled, cancellationToken);
        }

        public async Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            user.LockoutEnd = lockoutEnd;
            await UpdateAsync(user, x => x.LockoutEnd, user.LockoutEnd, cancellationToken);
        }

        public async Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            normalizedEmail.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            user.NormalizedEmail = normalizedEmail ?? _normalizer.NormalizeEmail(user.Email);

            await UpdateAsync(user, x => x.NormalizedEmail, user.NormalizedEmail, cancellationToken);
        }

        public async Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            normalizedName.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var name = normalizedName ?? _normalizer.NormalizeName(user.UserName);

            user.NormalizedUserName = name;
            await UpdateAsync(user, x => x.NormalizedUserName, name, cancellationToken);
        }

        public async Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            passwordHash.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            user.PasswordHash = passwordHash;

            await UpdateAsync(user, x => x.PasswordHash, passwordHash, cancellationToken);
        }

        public async Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            phoneNumber.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            user.PhoneNumber = phoneNumber;
            await UpdateAsync(user, x => x.PhoneNumber, phoneNumber, cancellationToken);
        }

        public async Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            user.PhoneNumberConfirmed = confirmed;
            await UpdateAsync(user, x => x.PhoneNumberConfirmed, confirmed, cancellationToken);
        }

        public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            stamp.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            user.SecurityStamp = stamp;

            return UpdateAsync(user, x => x.SecurityStamp, user.SecurityStamp, cancellationToken);
        }

        public async Task SetTokenAsync(TUser user, string loginProvider, string name, string value, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            if (user.Tokens == null)
                user.Tokens = new List<IdentityUserToken<TKey>>();

            var token = user.Tokens.FirstOrDefault(x => x.LoginProvider == loginProvider && x.Name == name);

            if (token == null)
            {
                token = new IdentityUserToken<TKey>
                {
                    LoginProvider = loginProvider,
                    Name = name,
                    Value = value
                };

                await AddAsync(user, x => x.Tokens, token, cancellationToken);
                user.Tokens.Add(token);
            }
            else
            {
                token.Value = value;
                await UpdateAsync(user, x => x.Tokens, user.Tokens, cancellationToken);
            }
        }

        public async Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            user.TwoFactorEnabled = enabled;

            await UpdateAsync(user, x => x.TwoFactorEnabled, enabled, cancellationToken);
        }

        public async Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            userName.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            await SetNormalizedUserNameAsync(user, _normalizer.NormalizeName(userName), cancellationToken);

            user.UserName = userName;

            await UpdateAsync(user, x => x.UserName, userName, cancellationToken);
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            await Update(user, cancellationToken);

            return IdentityResult.Success;
        }


    }

    public class UserRepository<TUser, TRole> : UserRepository<Guid, TUser, TRole>, IUserRepository<TUser, TRole>
        where TUser : BaseIdentityUser<Guid>
        where TRole : BaseIdentityRole<Guid>
    {
        public UserRepository(AuthMongoDbContext dbContext, IRoleRepository<TRole> roleRepository, ILookupNormalizer normalizer) :base(dbContext, roleRepository, normalizer)
        {
        }
    }
}
