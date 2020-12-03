using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using uBeac.Common;
using uBeac.Repositories.Abstractions;
using uBeac.Services.Abstractions;

namespace uBeac.Services
{
    public abstract class BaseEntityService<TKey, TEntity> 
        : IBaseEntityService<TKey, TEntity> 
        where TEntity : class, IEntity<TKey> 
        where TKey : IEquatable<TKey>
    {
        protected readonly IBaseEntityRepository<TKey, TEntity> Repository;

        public BaseEntityService(IBaseEntityRepository<TKey, TEntity> repository)
        {
            Repository = repository;
        }

        public virtual async Task<bool> Add(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await Repository.Insert(entity, cancellationToken);
            var result = (await Repository.SaveChanges(cancellationToken)) > 0;
            return result;
        }

        public virtual async Task<bool> Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await Repository.Update(entity, cancellationToken);
            var result = (await Repository.SaveChanges(cancellationToken)) > 0;
            return result;
        }

        public virtual async Task<bool> Delete(TKey id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await Repository.Delete(id, cancellationToken);
            var result = (await Repository.SaveChanges(cancellationToken)) > 0;
            return result;
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
    public abstract class BaseEntityService<TEntity> 
        : BaseEntityService<int, TEntity>
        where TEntity : class, IEntity
    {
        public BaseEntityService(IBaseEntityRepository<TEntity> repository) : base(repository)
        {
        }
        public override async Task<bool> Add(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (entity.Id != 0)
            {
                throw new Exception(string.Format("Exception while adding {0}, Id has been set to {1}", entity.GetType().Name, entity.Id.ToString()));
            }

            return await base.Add(entity, cancellationToken);
        }
    }
}
