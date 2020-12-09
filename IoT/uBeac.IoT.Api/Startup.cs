using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using uBeac.IoT.Api.Facades;
using uBeac.Repositories.Abstractions;
using uBeac.Repositories.MongoDB;
using uBeac.Services;
using uBeac.Services.Abstractions;
using uBeac.Web;

namespace uBeac.IoT.Api
{
    public class Startup : BaseStartup
    {

        public Startup(IHostEnvironment env) : base(env)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddScoped(typeof(IEntityRepository<,>), typeof(EntityGenericRepository<,>));
            services.AddScoped(typeof(IEntityRepository<>), typeof(EntityGenericRepository<>));

            services.AddScoped(typeof(IBaseEntityRepository<,>), typeof(BaseEntityGenericRepository<,>));
            services.AddScoped(typeof(IBaseEntityRepository<>), typeof(BaseEntityGenericRepository<>));


            //services.AddScoped(typeof(IEntityService<,>), typeof(EntityService<,>));
            services.AddScoped(typeof(IEntityService<>), typeof(EntityService<>));

            services.AddScoped(typeof(IBaseEntityService<,>), typeof(BaseEntityService<,>));
            services.AddScoped(typeof(IBaseEntityService<>), typeof(BaseEntityService<>));

            services.AddScoped<ITeamFacade, TeamFacade>();

            services.AddMongo<MongoDbContext>("uBeacDBConnection");

            services.AddAutoMapper(typeof(MappingProfile));
        }

        public override void Configure(IApplicationBuilder app)
        {
            base.Configure(app);
            //if (Environment.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseRouting();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
