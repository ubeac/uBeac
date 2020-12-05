using Microsoft.EntityFrameworkCore;
using System;
using uBeac.Common;
using uBeac.Repositories.Abstractions;

namespace uBeac.Repositories.EF
{
    public class BaseEntityGenericRepository<TKey, TEntity> : EntityGenericRepository<TKey, TEntity>,
        IBaseEntityRepository<TKey, TEntity>
        where TEntity : class, IBaseEntity<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        public BaseEntityGenericRepository(DbContext context) : base(context)
        {
        }
    }

    public class BaseEntityGenericRepository<TEntity> : BaseEntityGenericRepository<Guid, TEntity>,
        IBaseEntityRepository<TEntity>
        where TEntity : class, IBaseEntity, new()
    {
        public BaseEntityGenericRepository(DbContext context) : base(context)
        {
        }
    }
}
