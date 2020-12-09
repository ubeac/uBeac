using System;
using System.Threading;
using System.Threading.Tasks;
using uBeac.Common;
using uBeac.Repositories.Abstractions;
using uBeac.Services.Abstractions;

namespace uBeac.Services
{
    public class BaseEntityService<TKey, TEntity> : EntityService<TKey, TEntity>, IBaseEntityService<TKey, TEntity>
        where TEntity : class, IBaseEntity<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly IApplicationContext<TKey> _applicationContext;
        public BaseEntityService(IBaseEntityRepository<TKey, TEntity> repository, IApplicationContext<TKey> applicationContext) : base(repository)
        {
            applicationContext.ThrowIfNull();
            _applicationContext = applicationContext;
        }

        public override async Task Add(TEntity entity, CancellationToken cancellationToken = default)
        {
            entity.CreateDate = DateTime.Now;
            entity.UpdateDate = entity.CreateDate;
            entity.CreateBy = _applicationContext.UserId;
            entity.UpdateBy = _applicationContext.UserId;
            await base.Add(entity, cancellationToken);
        }

        public override async Task Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            entity.UpdateDate = DateTime.Now;
            entity.UpdateBy = _applicationContext.UserId;
            await base.Update(entity, cancellationToken);
        }
    }
    public class BaseEntityService<TEntity> : BaseEntityService<Guid, TEntity>, IBaseEntityService<TEntity>
        where TEntity : class, IBaseEntity, new()
    {
        public BaseEntityService(IBaseEntityRepository<TEntity> repository, IApplicationContext applicationContext) : base(repository, applicationContext)
        {
        }
        public override async Task Add(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (entity.Id != Guid.Empty)
            {
                throw new Exception(string.Format("Exception while adding {0}, Id has been set to {1}", entity.GetType().Name, entity.Id.ToString()));
            }

            await base.Add(entity, cancellationToken);
        }
    }
}
