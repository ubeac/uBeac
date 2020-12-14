using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace uBeac.Identity.MongoDB
{
   
    public class MongoUserStore<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim> :
        UserStoreBase<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>,
        IProtectedUserStore<TUser>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TUserRole : IdentityUserRole<TKey>, new()
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TUserToken : IdentityUserToken<TKey>, new()
        where TRoleClaim : IdentityRoleClaim<TKey>, new()
    {

        private readonly IMongoCollection<TUser> _userCollection;
        private readonly IMongoCollection<TUserClaim> _userClaimsCollection;
        private readonly IMongoCollection<TUserLogin> _userLoginsCollection;
        private readonly IMongoCollection<TUserRole> _userRolesCollection;
        private readonly IMongoCollection<TUserToken> _userTokensCollection;
        private readonly IMongoCollection<TRole> _rolesCollection;

        public MongoUserStore(IdentityErrorDescriber describer, 
            IMongoCollection<TUser> userCollection, 
            IMongoCollection<TUserClaim> userClaimsCollection, 
            IMongoCollection<TUserLogin> userLoginsCollection, 
            IMongoCollection<TUserRole> userRolesCollection, 
            IMongoCollection<TUserToken> userTokensCollection, 
            IMongoCollection<TRole> rolesCollection) : base(describer)
        {
            userCollection.ThrowIfNull();
            userClaimsCollection.ThrowIfNull();
            userLoginsCollection.ThrowIfNull();
            userRolesCollection.ThrowIfNull();
            userTokensCollection.ThrowIfNull();
            rolesCollection.ThrowIfNull();

            _userCollection = userCollection;
            _userClaimsCollection = userClaimsCollection;
            _userLoginsCollection = userLoginsCollection;
            _userRolesCollection = userRolesCollection;
            _userTokensCollection = userTokensCollection;
            _rolesCollection = rolesCollection;

            EnsureIndex(x => x.Email);
            EnsureIndex(x => x.NormalizedEmail);
            EnsureIndex(x => x.NormalizedUserName);
            EnsureIndex(x => x.UserName);

        }

        private void EnsureIndex(Expression<Func<TUser, object>> field)
        {
            var model = new CreateIndexModel<TUser>(Builders<TUser>.IndexKeys.Ascending(field));
            _userCollection.Indexes.CreateOne(model);
        }

        #region IUserStore
        public override async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            await _userCollection.InsertOneAsync(user, cancellationToken: cancellationToken);

            return IdentityResult.Success;
        }

        public override async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            await _userCollection.DeleteOneAsync(x => x.Id.Equals(user.Id), cancellationToken);

            return IdentityResult.Success;
        }

        public override async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            userId.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var id = ConvertIdFromString(userId);

            return (await _userCollection.FindAsync(x => x.Id.Equals(id), cancellationToken: cancellationToken)).SingleOrDefault(cancellationToken);

        }

        public override async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            await _userCollection.ReplaceOneAsync(x => x.Id.Equals(user.Id), user, cancellationToken: cancellationToken);

            return IdentityResult.Success;
        }

        public override async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            normalizedUserName.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return (await _userCollection.FindAsync(x => x.NormalizedUserName == normalizedUserName, cancellationToken: cancellationToken)).FirstOrDefault(cancellationToken);
        }

        #endregion

        #region IQueryableUserStore
        public override IQueryable<TUser> Users => _userCollection.AsQueryable();
        #endregion

        #region IUserClaimStore
        public override async Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            claims.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var userClaims = new List<TUserClaim>();

            foreach (var claim in claims)
                userClaims.Add(CreateUserClaim(user, claim));

            await _userClaimsCollection.InsertManyAsync(userClaims, cancellationToken: cancellationToken);
        }

        public override async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var claims = (await _userClaimsCollection.FindAsync(x => x.UserId.Equals(user.Id), cancellationToken: cancellationToken)).ToList(cancellationToken);
            return claims.Select(x => x.ToClaim()).ToList();
        }

        public override async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            claim.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var usersClaims = (await _userClaimsCollection.FindAsync(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value, cancellationToken: cancellationToken)).ToList(cancellationToken);
            var userIds = usersClaims.Select(x => x.UserId);
            return (await _userCollection.FindAsync(x => userIds.Contains(x.Id), cancellationToken: cancellationToken)).ToList(cancellationToken);
        }

        public override async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            claim.ThrowIfNull();
            newClaim.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var filter = Builders<TUserClaim>.Filter.Where(x => x.UserId.Equals(user.Id) && claim.Type == x.ClaimType && claim.Value == x.ClaimValue);

            var update = Builders<TUserClaim>.Update.Set(x => x.ClaimType, newClaim.Type).Set(x => x.ClaimValue, newClaim.Value);

            await _userClaimsCollection.FindOneAndUpdateAsync(filter, update, cancellationToken: cancellationToken);
        }

        public override async Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            claims.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var filter = Builders<TUserClaim>.Filter.Where(x => x.UserId.Equals(user.Id) && claims.Any(y => y.Type == x.ClaimType && y.Value == x.ClaimValue));

            await _userClaimsCollection.DeleteManyAsync(filter, cancellationToken);
        }

        #endregion

        #region IUserLoginStore

        public override async Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            login.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            await _userLoginsCollection.InsertOneAsync(CreateUserLogin(user, login), cancellationToken: cancellationToken);

        }

        public override async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            loginProvider.ThrowIfNull();
            providerKey.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var userLogin = (await _userLoginsCollection.FindAsync(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey, cancellationToken: cancellationToken)).SingleOrDefault(cancellationToken);

            if (userLogin == null)
                return null;

            return await FindByIdAsync(userLogin.UserId.ToString(), cancellationToken);
        }

        public override async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var userlogins = (await _userLoginsCollection.FindAsync(x => x.UserId.Equals(user.Id))).ToList(cancellationToken);

            return userlogins.Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName)).ToList();

        }

        public override async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            loginProvider.ThrowIfNull();
            providerKey.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            await _userLoginsCollection.DeleteManyAsync(x => x.UserId.Equals(user.Id) && x.LoginProvider == loginProvider && x.ProviderKey == providerKey, cancellationToken);
        }

        #endregion

        #region IUserRoleStore

        public override async Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            roleName.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var role = await FindRoleAsync(roleName, cancellationToken);

            if (role == null)
                throw new Exception(string.Format("Role {0} does not exist!", roleName));

            await _userRolesCollection.InsertOneAsync(CreateUserRole(user, role), cancellationToken: cancellationToken);

        }

        public override async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var roleIds = (await _userRolesCollection.FindAsync(x => x.UserId.Equals(user.Id))).ToList().Select(x => x.RoleId).ToList();
            if (roleIds.Count == 0)
                return new List<string>();

            var roles = (await _rolesCollection.FindAsync(x => roleIds.Contains(x.Id), cancellationToken: cancellationToken)).ToList();

            return roles.Select(x => x.Name).ToList();
        }

        public override async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            roleName.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var role = await FindRoleAsync(roleName, cancellationToken);

            if (role == null)
                return new List<TUser>();

            var userIds = (await _userRolesCollection.FindAsync(x => x.RoleId.Equals(role.Id), cancellationToken: cancellationToken)).ToList().Select(x => x.RoleId).ToList();
            if (userIds.Count == 0)
                return new List<TUser>();

            return (await _userCollection.FindAsync(x => userIds.Contains(x.Id), cancellationToken: cancellationToken)).ToList(cancellationToken);
        }

        public override async Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            roleName.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var role = await FindRoleAsync(roleName, cancellationToken);
            if (role == null)
                return false;

            var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);
            return userRole != null;
        }

        public override async Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            roleName.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var role = await FindRoleAsync(roleName, cancellationToken);
            if (role == null)
                return;

            await _userRolesCollection.DeleteOneAsync(x => x.RoleId.Equals(role.Id) && x.UserId.Equals(user.Id), cancellationToken);

        }

        #endregion

        #region IUserEmailStore

        public override async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            normalizedEmail.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return (await _userCollection.FindAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken: cancellationToken)).SingleOrDefault(cancellationToken);
        }

        #endregion

        #region ProtectedMethods

        protected override async Task<TUser> FindUserAsync(TKey userId, CancellationToken cancellationToken)
        {
            userId.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return (await _userCollection.FindAsync(x => x.Id.Equals(userId), cancellationToken: cancellationToken)).SingleOrDefault();
        }

        protected override async Task<TUserLogin> FindUserLoginAsync(TKey userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            userId.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return (await _userLoginsCollection.FindAsync(x => x.UserId.Equals(userId) && x.LoginProvider == loginProvider && x.ProviderKey == providerKey, cancellationToken: cancellationToken)).SingleOrDefault();
        }

        protected override async Task<TUserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return (await _userLoginsCollection.FindAsync(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey, cancellationToken: cancellationToken)).SingleOrDefault();
        }

        protected override async Task<TUserToken> FindTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            user.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return (await _userTokensCollection.FindAsync(x => x.UserId.Equals(user.Id) && x.LoginProvider == loginProvider && x.Name == name, cancellationToken: cancellationToken)).SingleOrDefault();
        }

        protected override async Task AddUserTokenAsync(TUserToken token)
        {
            token.ThrowIfNull();
            ThrowIfDisposed();

            await _userTokensCollection.InsertOneAsync(token);
        }

        protected override async Task RemoveUserTokenAsync(TUserToken token)
        {
            token.ThrowIfNull();
            ThrowIfDisposed();

            await _userTokensCollection.DeleteOneAsync(x => x.UserId.Equals(token.UserId) && x.Name == token.Name && x.LoginProvider == token.LoginProvider && x.Value == token.Value);
        }

        protected async override Task<TRole> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return (await _rolesCollection.FindAsync(x => x.NormalizedName == normalizedRoleName, cancellationToken: cancellationToken)).SingleOrDefault();
        }

        protected async override Task<TUserRole> FindUserRoleAsync(TKey userId, TKey roleId, CancellationToken cancellationToken)
        {
            return (await _userRolesCollection.FindAsync(x => x.UserId.Equals(userId) && x.RoleId.Equals(roleId), cancellationToken: cancellationToken)).SingleOrDefault();
        }

        #endregion
    }

    public class MongoUserStore<TUser, TRole, TKey> : MongoUserStore<TUser, TRole, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityUserToken<TKey>, IdentityRoleClaim<TKey>>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        public MongoUserStore(IdentityErrorDescriber describer,
           IMongoCollection<TUser> userCollection,
           IMongoCollection<IdentityUserClaim<TKey>> userClaimsCollection,
           IMongoCollection<IdentityUserLogin<TKey>> userLoginsCollection,
           IMongoCollection<IdentityUserRole<TKey>> userRolesCollection,
           IMongoCollection<IdentityUserToken<TKey>> userTokensCollection,
           IMongoCollection<TRole> rolesCollection) : base(describer, userCollection, userClaimsCollection, userLoginsCollection, userRolesCollection, userTokensCollection, rolesCollection)
        {
        }
    }

}
