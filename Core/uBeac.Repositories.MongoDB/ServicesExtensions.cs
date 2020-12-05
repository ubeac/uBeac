using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using uBeac.Repositories.MongoDB;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddMongo<TMongoDbContext>(this IServiceCollection services, string connectionStringName) where TMongoDbContext : class, IMongoDbContext
        {
            services.AddSingleton<BsonSerializerRegistrar>();

            services.AddSingleton(provider =>
            {
                var bsonSerializerRegistrarx = provider.GetService<BsonSerializerRegistrar>();
                var configuration = provider.GetService<IConfiguration>();
                var mongoUrl = new MongoUrl(configuration.GetConnectionString(connectionStringName));
                var client = new MongoClient(mongoUrl);
                var mongoDB = client.GetDatabase(mongoUrl.DatabaseName);
                return ActivatorUtilities.CreateInstance<TMongoDbContext>(provider, connectionStringName, mongoDB);
            });

            return services;

        }
    }
}
