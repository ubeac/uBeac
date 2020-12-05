//using System;
//using System.Collections.Generic;
//using System.Text;
//using uBeac.Repositories.Abstractions;
//using uBeac.Common;
//using System.Threading.Tasks;
//using System.Threading;

//namespace uBeac.Repositories.MongoDB
//{
//    public class MongoDBRepository<TKey, TEntity> : IBaseEntityRepository<TKey, TEntity>
//        where TEntity : class, IEntity<TKey>
//        where TKey : IEquatable<TKey>
//    {
//        public Task Delete(TKey id, CancellationToken cancellationToken = default)
//        {
//            throw new NotImplementedException();
//        }

//        public Task DeleteMany(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<PaginatedList<TEntity>> Filter(FilterCriteria<TEntity> filterCriteria, CancellationToken cancellationToken = default)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<PaginatedList<TEntity>> GetAll(CancellationToken cancellationToken = default)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<TEntity> GetById(TKey id, CancellationToken cancellationToken = default)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<PaginatedList<TEntity>> GetByIds(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
//        {
//            throw new NotImplementedException();
//        }

//        public Task Insert(TEntity entity, CancellationToken cancellationToken = default)
//        {
//            throw new NotImplementedException();
//        }

//        public Task InsertMany(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<int> SaveChanges(CancellationToken cancellationToken = default)
//        {
//            throw new NotImplementedException();
//        }

//        public Task Update(TEntity entity, CancellationToken cancellationToken = default)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
