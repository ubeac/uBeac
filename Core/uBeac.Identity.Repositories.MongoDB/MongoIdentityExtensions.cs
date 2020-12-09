using Microsoft.AspNetCore.Identity;
using System;
using uBeac.Common.Identity;
using uBeac.Identity.Repositories.Abstractions;
using uBeac.Identity.Repositories.MongoDB;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MongoIdentityExtensions
    {

        public static IdentityBuilder AddIdentityMongoDbProvider<TUser, TRole, TKey>(this IServiceCollection services, string connectionStringName, Action<IdentityOptions> setupIdentityAction)
            where TKey : IEquatable<TKey>
            where TUser : BaseIdentityUser<TKey>
            where TRole : BaseIdentityRole<TKey>
        {

            var builder = services.AddIdentity<TUser, TRole>(setupIdentityAction ?? (x => { }));

            builder.AddRoleStore<RoleRepository<TKey, TRole>>()
                .AddUserStore<UserRepository<TKey, TUser, TRole>>()
                .AddUserManager<UserManager<TUser>>()
                .AddRoleManager<RoleManager<TRole>>()
                .AddDefaultTokenProviders();

            // Identity Services
            services.AddTransient<IRoleRepository<TKey, TRole>, RoleRepository<TKey, TRole>>();
            services.AddTransient<IUserRepository<TKey, TUser, TRole>, UserRepository<TKey, TUser, TRole>>();

            services.AddMongo<AuthMongoDbContext>(connectionStringName);

            return builder;
        }

        public static IdentityBuilder AddIdentityMongoDbProvider<TUser, TRole>(this IServiceCollection services, string connectionStringName, Action<IdentityOptions> setupIdentityAction)
            where TUser : BaseIdentityUser
            where TRole : BaseIdentityRole
        {
            return services.AddIdentityMongoDbProvider<TUser, TRole, Guid>(connectionStringName, setupIdentityAction);
        }

    }
}
