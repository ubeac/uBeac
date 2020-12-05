using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SqlServerExtensions
    {
        public static IServiceCollection AddSqlServer<T>(this IServiceCollection services) where T : DbContext
        {
            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<T>((serviceProvider, options) => options.UseSqlServer(typeof(T).Name));

            return services;
        }
    }
}
