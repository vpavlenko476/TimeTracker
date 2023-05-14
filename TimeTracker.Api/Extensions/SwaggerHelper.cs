using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TimeTracker.Api.Extensions
{
    public static class SwaggerHelper
    {
        /// <summary>
        /// Добавление свагера с авторизацией и другими типовыми настройками
        /// </summary>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services,
            IConfiguration config,
            bool useCustomSchemaIds, Action<SwaggerGenOptions> setupAction, params Assembly[] assemblies)
        {
            services.AddSwaggerGen(options =>
            {
                //добаляем комментарии из сборок
                foreach (var assembly in assemblies ?? Enumerable.Empty<Assembly>())
                {
                    var assemblyXmlFile = $"{assembly.GetName().Name}.xml";
                    var assemblyXmlPath = Path.Combine(AppContext.BaseDirectory, assemblyXmlFile);
                    options.IncludeXmlComments(assemblyXmlPath);
                }

                if (useCustomSchemaIds)
                {
                    options.CustomSchemaIds(type => type.ToString());
                }

                setupAction?.Invoke(options);
            });

            return services;
        }

        /// <summary>
        /// Подключение UI свагера
        /// </summary>
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app, IConfiguration config,
            string name)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(options =>
            {
                options.SerializeAsV2 = true;
                options.RouteTemplate = "/swagger/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint($"/swagger/v1/swagger.json", name); });
            return app;
        }
    }
}