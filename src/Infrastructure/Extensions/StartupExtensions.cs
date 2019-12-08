using System.Linq;
using System.Reflection;
using HordeFlow.Infrastructure.HealthChecks;
using HordeFlow.Infrastructure.Swagger;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HordeFlow.Infrastructure.Extensions
{
    public static class StartupExtensions
    {
        public static IMvcBuilder AddCoreAssemblies(this IMvcBuilder builder, IConfiguration configuration)
        {
            var assemblies = ModuleExtensions.GetReferencingAssemblies(typeof(HordeFlow.Core.IModule).Namespace); // TODO: Needs enhancements
            // var assembly = assemblies.First();
            // builder.AddApplicationPart(assembly);
            foreach (Assembly assembly in assemblies)
            {
                builder.AddApplicationPart(assembly);
            }
            builder.AddApplicationPart(typeof(BaseStartup).Assembly);
            return builder;
        }

        public static ServiceRegistry AddApiHealthChecks(this ServiceRegistry services)
        {
            services
            .AddHealthChecks()
            .AddCheck("ping1", new PingHealthCheck("www.google.com", 100))
            .AddCheck<TenantDbHealthCheck>("tenant-db-health", failureStatus: HealthStatus.Degraded);

            return services;
        }

        public static IEndpointRouteBuilder MapApiHealthChecks(this IEndpointRouteBuilder endpoints, IConfiguration configuration)
        {
            var options = new HealthCheckOptions();
            options.ResponseWriter = async (c, r) =>
            {

                c.Response.ContentType = "application/json";

                var result = JsonConvert.SerializeObject(new
                {
                    status = r.Status.ToString(),
                    errors = r.Entries.Select(e => new { key = e.Key, value = e.Value.Status.ToString() })
                });

                await c.Response.WriteAsync(result);
            };
            endpoints
            .MapHealthChecks(configuration.GetSection("HealthChecks").GetValue<string>("Endpoint"), options)
            .RequireHost(configuration.GetSection("HealthChecks").GetValue<string>("HostFilter"));

            return endpoints;
        }

        public static IServiceCollection AddSwaggerApiExplorer(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                // add a custom operation filter which sets default values
                options.OperationFilter<SwaggerDefaultValues>();
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerApiExplorer(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }

                options.RoutePrefix = string.Empty;
            });

            return app;
        }
    }
}