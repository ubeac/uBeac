using System;
using System.Collections.Generic;
using uBeac.Repositories.Abstractions;
using uBeac.Common;
using System.Threading.Tasks;
using System.Threading;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;

namespace uBeac.Repositories.MongoDB
{
    public class EntityGenericRepository<TKey, TEntity> : IEntityRepository<TKey, TEntity>, IDisposable
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {

        protected readonly IMongoDatabase Database;
        protected readonly IMongoCollection<TEntity> Collection;
        protected readonly IMongoCollection<BsonDocument> BsonCollection;
        protected readonly IMongoDbContext DbContext;

        protected virtual string CollectionName { get; }

        public EntityGenericRepository(IMongoDbContext dbContext)
        {
            dbContext.ThrowIfNull();
            CollectionName = typeof(TEntity).Name;

            Database = dbContext.Database;
            DbContext = dbContext;
            Collection = Database.GetCollection<TEntity>(CollectionName);
            BsonCollection = Database.GetCollection<BsonDocument>(CollectionName);

            EnsureIndicesCreatedAsync().GetAwaiter().GetResult();
        }

        public virtual async Task Delete(TKey id, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            id.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, id);
            var result = await Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        }

        public async Task DeleteMany(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
        {

            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var filter = Builders<TEntity>.Filter.In(x => x.Id, ids);
            await Collection.DeleteManyAsync(filter, cancellationToken: cancellationToken);

        }

        public async Task<PaginatedList<TEntity>> Filter(FilterCriteria<TEntity> filterCriteria, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            filterCriteria.ThrowIfNull();

            long totalCount;
            var filter = filterCriteria.Query();

            if (filter == null)
                totalCount = await Collection.EstimatedDocumentCountAsync();
            else
                totalCount = await Collection.CountDocumentsAsync(filter, null, cancellationToken);


            if (filterCriteria.PageSize <= 0)
                filterCriteria.PageSize = 20;

            if (filterCriteria.PageNumber <= 0)
                filterCriteria.PageNumber = 1;


            var sorts = new SortDefinitionBuilder<TEntity>();

            if (filterCriteria.Sort != null && filterCriteria.Sort.Count > 0)
            {
                foreach (var sortItem in filterCriteria.Sort)
                {
                    if (sortItem.Value == true)
                        sorts.Ascending(new StringFieldDefinition<TEntity>(sortItem.Key));
                    else
                        sorts.Descending(new StringFieldDefinition<TEntity>(sortItem.Key));
                }
            }

            // todo: implement sorting
            var items = await Collection.FindAsync(filter, new FindOptions<TEntity> { Skip = (filterCriteria.PageNumber - 1) * filterCriteria.PageSize, Limit = filterCriteria.PageSize }, cancellationToken);

            var paginatedList = new PaginatedList<TEntity>(await items.ToListAsync(cancellationToken), filterCriteria.PageNumber, filterCriteria.PageSize, totalCount);

            return paginatedList;
        }

        public async Task<PaginatedList<TEntity>> GetAll(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            return await Task.FromResult(new PaginatedList<TEntity>(Collection.AsQueryable()));
        }

        public async Task<TEntity> GetById(TKey id, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            id.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var result = await Collection.FindAsync(x => x.Id.Equals(id), cancellationToken: cancellationToken);
            return result.FirstOrDefault();
        }

        public async Task<PaginatedList<TEntity>> GetByIds(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            ids.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var filter = Builders<TEntity>.Filter.In(x => x.Id, ids);
            var result = await Collection.FindAsync(filter, cancellationToken: cancellationToken);
            return new PaginatedList<TEntity>(result.Current);
        }

        public async Task Insert(TEntity entity, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            entity.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            await Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }

        public async Task InsertMany(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            entities.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            await Collection.InsertManyAsync(entities, cancellationToken: cancellationToken);
        }

        public async Task Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            entity.ThrowIfNull();
            cancellationToken.ThrowIfCancellationRequested();

            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
            var replaceResult = await Collection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = false });

        }


        #region Initialize DB

        private static bool _initialized = false;
        private static object _initializationLock = new object();
        private static object _initializationTarget;

        protected virtual async Task EnsureIndicesCreatedAsync()
        {
            var obj = LazyInitializer.EnsureInitialized(ref _initializationTarget, ref _initialized, ref _initializationLock, () =>
            {
                return EnsureIndicesCreatedDefinitionsAsync();
            });

            if (obj != null)
            {
                var taskToAwait = (Task)obj;
                await taskToAwait.ConfigureAwait(false);
            }
        }

        protected virtual async Task EnsureIndicesCreatedDefinitionsAsync()
        {
            await Task.FromResult(0);
        }

        #endregion Initialize DB 

        #region Disposable

        private bool _disposed = false;
        public void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }

        #endregion
    }

    public class EntityGenericRepository<TEntity> : EntityGenericRepository<Guid, TEntity>
        where TEntity : class, IEntity
    {
        public EntityGenericRepository(IMongoDbContext dbContext) : base(dbContext)
        {
        }
    }
}
