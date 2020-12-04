using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var title = configuration.GetSection("App:Name").Value;
            var version = configuration.GetSection("App:Version").Value;

            if (string.IsNullOrEmpty(title))
                throw new Exception("There is no App:Name section in config files!");

            if (string.IsNullOrEmpty(version))
                throw new Exception("There is no App:Version section in config files!");

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = version });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                var securityKeyScheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                };

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        securityKeyScheme, new List<string>()
                    }
                });

            });

            //services.AddSwaggerGenNewtonsoftSupport();

            return services;

        }
        public static IApplicationBuilder UserSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            var title = configuration.GetSection("App:Name").Value;
            var version = configuration.GetSection("App:Version").Value;

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.DisplayRequestDuration();
                c.SwaggerEndpoint("/swagger/v1/swagger.json", title + ", Version " + version);
                c.RoutePrefix = "doc";
            });

            return app;
        }
    }
}

