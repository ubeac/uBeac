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
    public class RoleRepository<TKey, TRole> : BaseEntityGenericRepository<TKey, TRole>, IRoleRepository<TKey, TRole>
        where TKey : IEquatable<TKey>
        where TRole : BaseIdentityRole<TKey>
    {

        protected override string CollectionName => "Roles";

        public RoleRepository(AuthMongoDbContext dbContext) : base(dbContext)
        {
            dbContext.ThrowIfNull();

            EnsureIndex(x => x.NormalizedName);
        }

        private void EnsureIndex(Expression<Func<TRole, object>> field)
        {
            var model = new CreateIndexModel<TRole>(Builders<TRole>.IndexKeys.Ascending(field));
            Collection.Indexes.CreateOne(model);
        }
        public IQueryable<TRole> Roles => Collection.AsQueryable();

        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            role.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var found = await Collection.FindAsync(x => x.NormalizedName == role.NormalizedName, cancellationToken: cancellationToken);

            if (found == null)
                await Insert(role, cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            role.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            role.UpdateDate = DateTime.Now;


            await Collection.ReplaceOneAsync(x => x.Id.Equals(role.Id), role, cancellationToken: cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            role.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            await Delete(role.Id, cancellationToken);

            return IdentityResult.Success;
        }

        public async Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            role.ThrowIfNull();
            claim.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var currentClaim = role.Claims.FirstOrDefault(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);

            if (currentClaim != null)
                return;


            var identityRoleClaim = new IdentityRoleClaim<TKey>()
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            };

            role.Claims.Add(identityRoleClaim);

            await Update(role, cancellationToken);

        }

        public Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            roleId.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var id = (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(roleId);

            return GetById(id, cancellationToken);
        }

        public async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            normalizedRoleName.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await Collection.FindAsync(x => x.NormalizedName == normalizedRoleName, cancellationToken: cancellationToken)).SingleOrDefault(cancellationToken: cancellationToken);
        }

        public async Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default)
        {
            role.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var dbRole = await GetById(role.Id, cancellationToken);

            return dbRole.Claims.Select(e => new Claim(e.ClaimType, e.ClaimValue)).ToList();
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            role.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            role.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(role.Id.ToString());
        }

        public async Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            role.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            return (await GetById(role.Id, cancellationToken))?.Name ?? role.Name;
        }

        public async Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            role.ThrowIfNull();
            claim.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            role.Claims.RemoveAll(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);

            await Update(role, cancellationToken);
        }

        public async Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.ThrowIfNull();
            normalizedName.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            role.NormalizedName = normalizedName;

            await Collection.UpdateOneAsync(x => x.Id.Equals(role.Id), Builders<TRole>.Update.Set(x => x.NormalizedName, normalizedName), cancellationToken: cancellationToken);
        }

        public async Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            role.ThrowIfNull();
            roleName.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            role.Name = roleName;

            await Collection.UpdateOneAsync(x => x.Id.Equals(role.Id), Builders<TRole>.Update.Set(x => x.Name, roleName), cancellationToken: cancellationToken);
        }

    }

    public class RoleRepository<TRole> : RoleRepository<Guid, TRole>, IRoleRepository<TRole> where TRole : BaseIdentityRole<Guid>
    {
        public RoleRepository(AuthMongoDbContext dbContext) : base(dbContext)
        {
        }
    }
}
