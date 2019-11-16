using i21Apis.Models;
using i21Apis.Multitenancy;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddLogging();
            services.AddMultitenancy<Tenant, DefaultTenantResolver>();

            services.AddScoped<DbContext>(provider =>
            {
                var tenant = provider.GetRequiredService<Tenant>();

                if (tenant.Engine.Equals("MSSQL"))
                    return provider.GetRequiredService<TenantDbContext>();
                return provider.GetRequiredService<SqliteDbContext>();
            });

            services.Scan(scanner =>
            {
                // scanner.AssembliesFromApplicationBaseDirectory();
                scanner.TheCallingAssembly();
                scanner.WithDefaultConventions();
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
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
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
