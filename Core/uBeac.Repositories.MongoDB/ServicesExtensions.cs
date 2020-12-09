using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using uBeac.Repositories.MongoDB;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddMongo<TMongoDbContext>(this IServiceCollection services, string connectionStringName) where TMongoDbContext : class, IMongoDbContext
        {            
            services.AddSingleton(provider =>
            {
                BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
                BsonSerializer.RegisterSerializer(typeof(decimal?), new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
                BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

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
