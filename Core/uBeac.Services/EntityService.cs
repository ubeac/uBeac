using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using uBeac.Common;
using uBeac.Repositories.Abstractions;
using uBeac.Services.Abstractions;

namespace uBeac.Services
{
    public class EntityService<TKey, TEntity>
       : IEntityService<TKey, TEntity>
       where TEntity : class, IEntity<TKey>
       where TKey : IEquatable<TKey>
    {
        protected readonly IEntityRepository<TKey, TEntity> Repository;

        public EntityService(IEntityRepository<TKey, TEntity> repository)
        {
            Repository = repository;
        }

        public virtual async Task<bool> Add(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await Repository.Insert(entity, cancellationToken);
            return true;
        }

        public virtual async Task<bool> Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await Repository.Update(entity, cancellationToken);
            return true;
        }

        public virtual async Task<bool> Delete(TKey id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await Repository.Delete(id, cancellationToken);
            return true;
        }

        public virtual async Task<PaginatedList<TEntity>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Repository.GetAll(cancellationToken);
        }

        public virtual async Task<TEntity> GetById(TKey id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await Repository.GetById(id, cancellationToken);
        }

        public virtual async Task<PaginatedList<TEntity>> GetByIds(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return (await Repository.GetByIds(ids, cancellationToken));
        }

        public virtual async Task<PaginatedList<TEntity>> Filter(FilterCriteria<TEntity> filterCriteria, CancellationToken cancellationToken = default)
        {
            return await Repository.Filter(filterCriteria, cancellationToken);
        }
    }
    public class EntityService<TEntity>
        : EntityService<Guid, TEntity>
        where TEntity : class, IEntity
    {
        public EntityService(IEntityRepository<TEntity> repository) : base(repository)
        {
        }
        public override async Task<bool> Add(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (entity.Id != Guid.Empty)
            {
                throw new Exception(string.Format("Exception while adding {0}, Id has been set to {1}", entity.GetType().Name, entity.Id.ToString()));
            }

            return await base.Add(entity, cancellationToken);
        }
    }
}
