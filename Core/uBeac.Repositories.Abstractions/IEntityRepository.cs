using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using uBeac.Common;

namespace uBeac.Repositories.Abstractions
{
    public interface IEntityRepository<TKey, TEntity> : IRepository
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        Task Insert(TEntity entity, CancellationToken cancellationToken = default);
        Task InsertMany(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        Task Update(TEntity entity, CancellationToken cancellationToken = default);
        Task Delete(TKey id, CancellationToken cancellationToken = default);
        Task DeleteMany(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);
        Task<PaginatedList<TEntity>> GetAll(CancellationToken cancellationToken = default);
        Task<TEntity> GetById(TKey id, CancellationToken cancellationToken = default);
        Task<PaginatedList<TEntity>> GetByIds(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);
        Task<int> SaveChanges(CancellationToken cancellationToken = default);
        Task<PaginatedList<TEntity>> Filter(FilterCriteria<TEntity> filterCriteria, CancellationToken cancellationToken = default);
    }
    public interface IEntityRepository<TEntity> : IEntityRepository<Guid, TEntity>
        where TEntity : class, IEntity
    {
    }
}
