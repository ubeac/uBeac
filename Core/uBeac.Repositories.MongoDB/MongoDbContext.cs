using MongoDB.Driver;

namespace uBeac.Repositories.MongoDB
{
    public interface IMongoDbContext
    {
        IMongoDatabase Database { get; }
        string ConnectionString { get; }
    }

    public abstract class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly string _connectionString;

        public MongoDbContext(string connectionString, IMongoDatabase database)
        {
            _database = database;
            _connectionString = connectionString;
        }

        public IMongoDatabase Database => _database;
        public string ConnectionString => _connectionString;

    }
}
