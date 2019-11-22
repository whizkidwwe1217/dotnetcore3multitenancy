using HordeFlow.Data;
using HordeFlow.Data.Catalog;
using HordeFlow.HealthChecks;
using HordeFlow.Models;
using HordeFlow.Multitenancy;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace HordeFlow
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ServiceRegistry services)
        {
            services.Scan(scanner =>
            {
                scanner.TheCallingAssembly();
                scanner.WithDefaultConventions();
            });

            services.AddLogging();
            services.AddMultitenancy<ITenant, DomainTenantResolver>()
            .AddMultiDbContext<Tenant>(options =>
            {
                options.ThrowWhenTenantIsNotFound = true;
            });
            services.For<ICatalogStore<ITenant>>().Use<SqlServerCatalogStore>();
            services.AddHealthChecks()
            .AddCheck<TenantDbHealthCheck>("tenant-db-health", failureStatus: HealthStatus.Degraded);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
            .AddControllers()
            .AddXmlSerializerFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMultitenancy<Tenant>(options =>
            {
                options.PreventInvalidCatalogAccess = true;
            });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks(Configuration.GetSection("HealthChecks").GetValue<string>("Endpoint"))
                    .RequireHost(Configuration.GetSection("HealthChecks").GetValue<string>("HostFilter"));
            });
        }
    }
}
