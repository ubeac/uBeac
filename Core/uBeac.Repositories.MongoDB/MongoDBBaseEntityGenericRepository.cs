using System;
using uBeac.Common;
using uBeac.Repositories.Abstractions;

namespace uBeac.Repositories.MongoDB
{
    class MongoDBBaseEntityGenericRepository
    {
    }
    public class MongoDBBaseEntityGenericRepository<TKey, TEntity> : MongoDBEntityGenericRepository<TKey, TEntity>,
        IBaseEntityRepository<TKey, TEntity>
        where TEntity : class, IBaseEntity<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        public MongoDBBaseEntityGenericRepository(IMongoDbContext dbContext) : base(dbContext)
        {
        }
    }

    public class MongoDBBaseEntityGenericRepository<TEntity> : MongoDBBaseEntityGenericRepository<Guid, TEntity>,
        IBaseEntityRepository<TEntity>
        where TEntity : class, IBaseEntity, new()
    {
        public MongoDBBaseEntityGenericRepository(IMongoDbContext dbContext) : base(dbContext)
        {
        }
    }
}
