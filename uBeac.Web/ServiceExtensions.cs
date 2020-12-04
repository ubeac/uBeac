using System.Linq;
using System.Reflection;
using uBeac.Repositories;
using uBeac.Repositories.Abstractions;
using uBeac.Services;
using uBeac.Services.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {

            services.AddScoped(typeof(IBaseEntityRepository<,>), typeof(BaseEntityRepository<,>));
            services.AddScoped(typeof(IBaseEntityRepository<>), typeof(BaseEntityRepository<>));

            var assem = Assembly.GetEntryAssembly();

            var repositories = assem.GetTypes().Where(t => !string.IsNullOrEmpty(t.Namespace) && !t.IsInterface && !t.IsAbstract && t.IsClass && t.Namespace.EndsWith(".Repositories") && t.Name.EndsWith("Repository")).ToList();
            foreach (var repository in repositories)
            {
                var irepository = assem.GetTypes().Where(i => i.Namespace == repository.Namespace && i.IsInterface && i.Name == "I" + repository.Name).SingleOrDefault();
                if (irepository == null)
                    throw new System.Exception("Repository " + repository.Name + " does not implement any interface derived from IBaseRepository");

                services.AddScoped(irepository, repository);
            }
            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {

            services.AddScoped(typeof(IBaseEntityService<,>), typeof(BaseEntityService<,>));
            services.AddScoped(typeof(IBaseEntityService<>), typeof(BaseEntityService<>));

            var assem = Assembly.GetEntryAssembly();

            var sers = assem.GetTypes().Where(t => !string.IsNullOrEmpty(t.Namespace) && !t.IsInterface && !t.IsAbstract && t.IsClass && t.Namespace.EndsWith(".Services") && t.Name.EndsWith("Service")).ToList();
            foreach (var service in sers)
            {
                var iservice = assem.GetTypes().Where(i => i.Namespace == service.Namespace && i.IsInterface && i.Name == "I" + service.Name).SingleOrDefault();
                if (iservice == null)
                    throw new System.Exception("Service " + service.Name + " does not implement any interface derived from IBaseService");

                services.AddScoped(iservice, service);
            }
            return services;
        }
    }
}
