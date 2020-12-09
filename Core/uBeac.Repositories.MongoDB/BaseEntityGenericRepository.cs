using System;
using uBeac.Common;
using uBeac.Repositories.Abstractions;

namespace uBeac.Repositories.MongoDB
{
    public class BaseEntityGenericRepository<TKey, TEntity> : EntityGenericRepository<TKey, TEntity>,
        IBaseEntityRepository<TKey, TEntity>
        where TEntity : class, IBaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public BaseEntityGenericRepository(IMongoDbContext dbContext) : base(dbContext)
        {
        }
    }

    public class BaseEntityGenericRepository<TEntity> : BaseEntityGenericRepository<Guid, TEntity>, IBaseEntityRepository<TEntity>
        where TEntity : class, IBaseEntity
    {
        public BaseEntityGenericRepository(IMongoDbContext dbContext) : base(dbContext)
        {
        }
    }
}
