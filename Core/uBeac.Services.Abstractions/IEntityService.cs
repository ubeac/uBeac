using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using uBeac.Common;

namespace uBeac.Services.Abstractions
{
    public interface IEntityService<TKey, TEntity> : IService
              where TEntity : class, IEntity<TKey>
              where TKey : IEquatable<TKey>
    {
        Task<bool> Add(TEntity entity, CancellationToken cancellationToken = default);
        Task<bool> Update(TEntity entity, CancellationToken cancellationToken = default);
        Task<bool> Delete(TKey id, CancellationToken cancellationToken = default);
        Task<PaginatedList<TEntity>> GetAll(CancellationToken cancellationToken = default);
        Task<TEntity> GetById(TKey id, CancellationToken cancellationToken = default);
        Task<PaginatedList<TEntity>> GetByIds(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);
        Task<PaginatedList<TEntity>> Filter(FilterCriteria<TEntity> filterCriteria, CancellationToken cancellationToken = default);
    }
    public interface IEntityService<TEntity> : IEntityService<Guid, TEntity>
        where TEntity : class, IEntity
    {
    }
}
