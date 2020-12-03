using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using uBeac.Common;
using uBeac.Repositories.Abstractions.Abstractions;

namespace uBeac.Repositories
{
    public abstract class BaseEntityRepository<TKey, TEntity> : IBaseEntityRepository<TKey, TEntity>
         where TEntity : class, IEntity<TKey>, new()
         where TKey : IEquatable<TKey>
    {

        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> Entities;
        public BaseEntityRepository(DbContext context)
        {
            Context = context;
            Entities = context.Set<TEntity>();
        }

        public virtual async Task Insert(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Entities.AddAsync(entity);
        }

        public virtual async Task InsertMany(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Entities.AddRangeAsync(entities);
        }

        public virtual async Task Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Run(() => Context.Entry(entity).State = EntityState.Modified);
        }

        public virtual async Task<int> SaveChanges(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await Context.SaveChangesAsync();
        }

        public virtual async Task Delete(TKey id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var entity = new TEntity { Id = id };
            await Task.FromResult(Context.Entry(entity).State = EntityState.Deleted);
        }

        public virtual async Task DeleteMany(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var id in ids)
                await Delete(id, cancellationToken);
        }

        public virtual async Task<PaginatedList<TEntity>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var items = new PaginatedList<TEntity>(Query(cancellationToken).AsEnumerable());
            return await Task.FromResult(items);
        }

        public virtual async Task<TEntity> GetById(TKey id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Query(s => s.Id.Equals(id), cancellationToken).SingleOrDefaultAsync();
        }

        public virtual async Task<PaginatedList<TEntity>> GetByIds(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var items = await Query(s => ids.Contains(s.Id)).ToListAsync(cancellationToken);
            return new PaginatedList<TEntity>(items);
        }

        protected virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Entities.AsNoTracking().Where(predicate);
        }
        protected virtual IQueryable<TEntity> Query(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Entities.AsNoTracking();
        }

        public virtual async Task<PaginatedList<TEntity>> Filter(FilterCriteria<TEntity> filterCriteria, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            int totalCount;
            var filter = filterCriteria.Query();

            if (filter == null)
                totalCount = await Query(cancellationToken).CountAsync();
            else
                totalCount = await Query(filter).CountAsync();

            if (filterCriteria.PageSize <= 0)
                filterCriteria.PageSize = 20;

            if (filterCriteria.PageNumber <= 0)
                filterCriteria.PageNumber = 1;

            var query = Query(cancellationToken);

            if (filterCriteria.Sort != null && filterCriteria.Sort.Count > 0)
            {
                foreach (var sortItem in filterCriteria.Sort)
                {
                    if (sortItem.Value == true)
                        query = query.OrderBy(sortItem.Key);
                    else
                        query = query.OrderByDescending(sortItem.Key);
                }
            }

            if (filter != null)
                query = query.Where(filter);

            var items = await query.Skip((filterCriteria.PageNumber - 1) * filterCriteria.PageSize).Take(filterCriteria.PageSize).ToListAsync();

            var paginatedList = new PaginatedList<TEntity>(items, filterCriteria.PageNumber, filterCriteria.PageSize, totalCount);

            return paginatedList;
        }
    }

    public abstract class BaseEntityRepository<TEntity> : BaseEntityRepository<int, TEntity> where TEntity : class, IEntity, new()
    {
        public BaseEntityRepository(DbContext context) : base(context)
        {
        }
    }
}
