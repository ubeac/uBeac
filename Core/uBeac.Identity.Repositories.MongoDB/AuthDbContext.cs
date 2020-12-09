using MongoDB.Driver;
using uBeac.Repositories.MongoDB;

namespace uBeac.Identity.Repositories.MongoDB
{
    public class AuthMongoDbContext : MongoDbContext
    {
        public AuthMongoDbContext(string connectionString, IMongoDatabase database) : base(connectionString, database)
        {
        }
    }
}
