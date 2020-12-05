using Microsoft.EntityFrameworkCore;
using System;
using uBeac.Common;

namespace uBeac.Repositories.EF
{
    public class BaseEntityGenericRepository<TKey, TEntity> : EntityGenericRepository<TKey, TEntity>
         where TEntity : class, IBaseEntity<TKey>, new()
         where TKey : IEquatable<TKey>
    {
        public BaseEntityGenericRepository(DbContext context) : base(context)
        {
        }
    }

    public class BaseEntityGenericRepository<TEntity> : BaseEntityGenericRepository<Guid, TEntity>
        where TEntity : class, IBaseEntity, new()
    {
        public BaseEntityGenericRepository(DbContext context) : base(context)
        {
        }
    }
}
