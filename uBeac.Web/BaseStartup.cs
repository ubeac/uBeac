using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using uBeac.Common;
using uBeac.Configuration;
using uBeac.Web.Middlewares;

namespace uBeac.Web
{
    public abstract class BaseStartup
    {
        public IConfigurationRoot Configuration { get; }
        public IHostEnvironment Environment { get; }

        public BaseStartup(IHostEnvironment env)
        {
            Environment = env;
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonConfig(env);
            Configuration = configBuilder.Build();
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //services.AddMvcCore();
            //.AddDataAnnotations()
            //.AddApiExplorer()
            //.AddFormatterMappings()
            //.AddCors();
            //.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssembly(Assembly.GetEntryAssembly()));

            //services.AddCoreServices();

            services.AddAuthorization();

            services.AddHttpContextAccessor();
            //services.RegisterRepositories();
            //services.RegisterServices();

            services.AddMemoryCache();
            services.AddSwagger(Configuration);
            services.AddScoped<IApplicationContext, ApplicationContext>();
            services.AddSingleton<IConfiguration>(Configuration);

        }

        public virtual void Configure(IApplicationBuilder app)
        {

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseDeveloperExceptionPage();
            //if (Environment.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{

            //}
            //app.UseExceptionHandler("/error");

            // global cors policy
            // todo: set CORS in release environment
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());


            app.UserSwagger(Configuration);

            app.UseMiddleware<UnhandledExceptionMiddleware>();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


    }
}
