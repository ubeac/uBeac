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

namespace uBeac.Identity.MongoDB
{
    public class RoleStore<TRole, TKey, TUserRole, TRoleClaim> : IRoleStore<TRole>, IQueryableRoleStore<TRole>, IRoleClaimStore<TRole>, IDisposable
       where TRole : IdentityRole<TKey>
       where TKey : IEquatable<TKey>
       where TUserRole : IdentityUserRole<TKey>, new()
       where TRoleClaim : IdentityRoleClaim<TKey>, new()

    {
        private readonly IMongoCollection<TRole> _roleCollection;
        private readonly IMongoCollection<TRoleClaim> _roleClaimsCollection;
        private readonly IMongoDatabase _mongoDatabase;

        public RoleStore(IdentityMongoDatabase identityMongoDatabase, MongoDBIdentityOptions mongoDBIdentityOptions)
        {
            _mongoDatabase = identityMongoDatabase.Database;

            _roleCollection = _mongoDatabase.GetCollection<TRole>(mongoDBIdentityOptions.RolesCollection);
            _roleClaimsCollection = _mongoDatabase.GetCollection<TRoleClaim>(mongoDBIdentityOptions.RoleClaimsCollection);

            EnsureIndex(x => x.NormalizedName);
            EnsureIndex(x => x.Name);
        }

        private void EnsureIndex(Expression<Func<TRole, object>> field)
        {
            var model = new CreateIndexModel<TRole>(Builders<TRole>.IndexKeys.Ascending(field));
            _roleCollection.Indexes.CreateOne(model);
        }

        #region IRoleStore

        public virtual TKey ConvertIdFromString(string id)
        {
            if (id == null)
                return default;

            return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id);

        }

        public virtual async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            await _roleCollection.InsertOneAsync(role, cancellationToken: cancellationToken);

            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            role.ConcurrencyStamp = Guid.NewGuid().ToString();

            await _roleCollection.ReplaceOneAsync(x => x.Id.Equals(role.Id), role, cancellationToken: cancellationToken);

            return IdentityResult.Success;
        }

        public virtual async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            await _roleCollection.DeleteOneAsync(x => role.Id.Equals(role.Id), cancellationToken);

            return IdentityResult.Success;
        }

        public virtual async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var id = ConvertIdFromString(roleId);

            return (await _roleCollection.FindAsync(x => x.Id.Equals(id), cancellationToken: cancellationToken)).SingleOrDefault(cancellationToken);
        }

        public virtual async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return (await _roleCollection.FindAsync(x => x.NormalizedName == normalizedRoleName, cancellationToken: cancellationToken)).SingleOrDefault(cancellationToken);
        }

        public virtual Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return Task.FromResult(role.NormalizedName);
        }

        public virtual Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return Task.FromResult(role.Id.ToString());
        }

        public virtual Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return Task.FromResult(role.Name);
        }

        public virtual Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            role.NormalizedName = normalizedName;

            return Task.CompletedTask;
        }

        public virtual Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            role.Name = roleName;

            return Task.CompletedTask;
        }


        #endregion

        #region IQueryableRoleStore

        public IQueryable<TRole> Roles => _roleCollection.AsQueryable();

        #endregion

        #region IRoleClaimStore

        protected virtual TRoleClaim CreateRoleClaim(TRole role, Claim claim)
           => new TRoleClaim { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value };

        public async Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            await _roleClaimsCollection.InsertOneAsync(CreateRoleClaim(role, claim), cancellationToken: cancellationToken);

        }

        public async Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return (await _roleClaimsCollection.FindAsync(x => x.RoleId.Equals(role.Id), cancellationToken: cancellationToken)).ToList(cancellationToken).Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
        }


        public async Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            await _roleClaimsCollection.DeleteOneAsync(x => x.RoleId.Equals(role.Id) && x.ClaimType == claim.Type && x.ClaimValue == claim.Value, cancellationToken: cancellationToken);
        }
        #endregion

        #region IDisposable

        private bool _disposed;

        public void Dispose() => _disposed = true;

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        #endregion

    }
}
