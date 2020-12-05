using System;
using System.Threading;
using System.Threading.Tasks;
using uBeac.Common;
using uBeac.Repositories.Abstractions;

namespace uBeac.Services
{
    public class BaseEntityService<TKey, TEntity>
        : EntityService<TKey, TEntity>
        where TEntity : class, IBaseEntity<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly IApplicationContext<TKey> _applicationContext;
        public BaseEntityService(IBaseEntityRepository<TKey, TEntity> repository, IApplicationContext<TKey> applicationContext) : base(repository)
        {
            if (_applicationContext is null)
                throw new NullReferenceException("ApplicationContext is null in " + GetType().Name);

            _applicationContext = applicationContext;
        }

        public override async Task<bool> Add(TEntity entity, CancellationToken cancellationToken = default)
        {
            entity.CreateDate = DateTime.Now;
            entity.CreateBy = _applicationContext.UserId;
            return await base.Add(entity, cancellationToken);
        }

        public override async Task<bool> Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            entity.UpdateDate = DateTime.Now;
            entity.UpdateBy = _applicationContext.UserId;
            return await base.Update(entity, cancellationToken);
        }
    }
    public class BaseEntityService<TEntity>
        : EntityService<Guid, TEntity>
        where TEntity : class, IBaseEntity
    {
        public BaseEntityService(IBaseEntityRepository<TEntity> repository) : base(repository)
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
