using i21Apis.Data;
using i21Apis.HealthChecks;
using i21Apis.Models;
using i21Apis.Multitenancy;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace i21Apis
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
            services.AddMultitenancy<Tenant, CachedDomainTenantResolver>();
            services.AddMultiDbContext<Tenant>();

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

            app.UseMultitenancy<Tenant>();
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
