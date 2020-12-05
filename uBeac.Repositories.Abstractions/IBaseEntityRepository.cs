using System;
using uBeac.Common;

namespace uBeac.Repositories.Abstractions
{
    public interface IBaseEntityRepository<TKey, TEntity> : IEntityRepository<TKey, TEntity>
       where TEntity : class, IBaseEntity<TKey>
       where TKey : IEquatable<TKey>
    {
    }
    public interface IBaseEntityRepository<TEntity> : IBaseEntityRepository<Guid, TEntity>
        where TEntity : class, IBaseEntity
    {
    }
}
