using MongoDB.Driver;

namespace uBeac.Identity.MongoDB
{
    public class IdentityMongoDatabase
    {
        public IMongoDatabase Database { get; }
        public IdentityMongoDatabase(IMongoDatabase database)
        {
            Database = database;
        }
    }
}
