using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using uBeac.Identity.MongoDB;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityMongoDBBuilderExtensions
    {
        public static IdentityBuilder AddMongoDBStores<TUser, TRole>(this IdentityBuilder builder)
          where TUser : IdentityUser<Guid>
          where TRole : IdentityRole<Guid>
        {
            builder.AddMongoDBStores<TUser, TRole, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityUserToken<Guid>, IdentityRoleClaim<Guid>>();
            return builder;
        }

        public static IdentityBuilder AddMongoDBStores<TUser, TRole, TKey>(this IdentityBuilder builder)
            where TUser : IdentityUser<TKey>
            where TRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
        {
            builder.AddMongoDBStores<TUser, TRole, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityUserToken<TKey>, IdentityRoleClaim<TKey>>();
            return builder;
        }

        public static IdentityBuilder AddMongoDBStores<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>(this IdentityBuilder builder)
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TUserRole : IdentityUserRole<TKey>, new()
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TUserToken : IdentityUserToken<TKey>, new()
        where TRoleClaim : IdentityRoleClaim<TKey>, new()
        {

            builder.Services.AddOptions().AddLogging();

            builder.Services.AddSingleton(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                var options = provider.GetRequiredService<IOptions<MongoDBIdentityOptions>>();
                return options.Value != null ? options.Value : new MongoDBIdentityOptions();
            });

            builder.Services.AddSingleton(provider =>
            {
                BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

                var configuration = provider.GetService<IConfiguration>();
                var options = provider.GetRequiredService<MongoDBIdentityOptions>();
                var mongoUrl = new MongoUrl(configuration.GetConnectionString(options.ConnectionStringName));
                var client = new MongoClient(mongoUrl);
                return client.GetDatabase(mongoUrl.DatabaseName);
            });

            builder.Services.AddSingleton(provider =>
            {
                var db = provider.GetRequiredService<IMongoDatabase>();
                var options = provider.GetRequiredService<MongoDBIdentityOptions>();
                return db.GetCollection<TRole>(options.RolesCollection);
            });

            builder.Services.AddSingleton(provider =>
            {
                var db = provider.GetRequiredService<IMongoDatabase>();
                var options = provider.GetRequiredService<MongoDBIdentityOptions>();
                return db.GetCollection<TUser>(options.UsersCollection);
            });

            builder.Services.AddSingleton(provider =>
            {
                var db = provider.GetRequiredService<IMongoDatabase>();
                var options = provider.GetRequiredService<MongoDBIdentityOptions>();
                return db.GetCollection<TUserClaim>(options.UserClaimsCollection);
            });

            builder.Services.AddSingleton(provider =>
            {
                var db = provider.GetRequiredService<IMongoDatabase>();
                var options = provider.GetRequiredService<MongoDBIdentityOptions>();
                return db.GetCollection<TUserRole>(options.UserRolesCollection);
            });

            builder.Services.AddSingleton(provider =>
            {
                var db = provider.GetRequiredService<IMongoDatabase>();
                var options = provider.GetRequiredService<MongoDBIdentityOptions>();
                return db.GetCollection<TUserLogin>(options.UserLoginsCollection);
            });

            builder.Services.AddSingleton(provider =>
            {
                var db = provider.GetRequiredService<IMongoDatabase>();
                var options = provider.GetRequiredService<MongoDBIdentityOptions>();
                return db.GetCollection<TUserToken>(options.UserTokensCollection);
            });

            builder.Services.AddSingleton(provider =>
            {
                var db = provider.GetRequiredService<IMongoDatabase>();
                var options = provider.GetRequiredService<MongoDBIdentityOptions>();
                return db.GetCollection<TRoleClaim>(options.RoleClaimsCollection);
            });
           
            builder.Services.AddScoped<IUserStore<TUser>, MongoUserStore<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>>();
            builder.Services.AddScoped<IRoleStore<TRole>, MongoRoleStore<TRole, TKey, TUserRole, TRoleClaim>>();

            builder.AddRoles<TRole>();

            return builder;
        }

    }
}
