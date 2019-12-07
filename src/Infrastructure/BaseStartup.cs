using HordeFlow.Core;
using HordeFlow.Data;
using HordeFlow.Data.Catalog;
using HordeFlow.Infrastructure.Extensions;
using HordeFlow.Infrastructure.Multitenancy;
using HordeFlow.Infrastructure.Repositories;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HordeFlow.Infrastructure
{
    public abstract class BaseStartup
    {
        public BaseStartup(IConfiguration configuration)
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
                scanner.AddAllTypesOf(typeof(IRepositoryManager<>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IRepositoryManager<>));
                scanner.AddAllTypesOf(typeof(IRepository<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IRepository<,>));
            });

            services.AddDbContext<SqlServerCatalogDbContext>();
            services.AddDbContext<TenantSqlServerDbContext>();
            services.AddDbContext<TenantSqliteDbContext>();
            services.AddDbContext<TenantMySqlDbContext>();

            services.AddLogging();
            services.AddMultitenancy<Tenant, DomainTenantResolver<Tenant>>()
            .AddMultiDbContext<Tenant>();
            // services.AddHealthChecks()
            // .AddCheck<TenantDbHealthCheck>("tenant-db-health", failureStatus: HealthStatus.Degraded);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
            .AddControllers()
            .AddNewtonsoftJson()
            .AddXmlSerializerFormatters()
            .AddCoreAssemblies(Configuration);
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
                // endpoints.MapHealthChecks(Configuration.GetSection("HealthChecks").GetValue<string>("Endpoint"))
                //     .RequireHost(Configuration.GetSection("HealthChecks").GetValue<string>("HostFilter"));
            });
        }
    }
}
