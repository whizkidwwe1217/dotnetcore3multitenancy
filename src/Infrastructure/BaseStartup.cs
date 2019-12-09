using HordeFlow.Core;
using HordeFlow.Infrastructure.Extensions;
using HordeFlow.Infrastructure.Multitenancy;
using HordeFlow.Infrastructure.Repositories;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
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
            });

            services.AddLogging();
            services
            .AddMultitenancy<Tenant, DomainTenantResolver<Tenant>>()
            .AddMultiDbContext<Tenant>()
            .AddApiHealthChecks();

            ConfigureOtherContainerServices(services);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddMvc();
            services
            .AddApiVersioning()
            .AddControllers()
            .AddNewtonsoftJson()
            .AddXmlSerializerFormatters()
            .AddCoreAssemblies(Configuration);

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerApiExplorer();

            ConfigureOtherServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
        IApiVersionDescriptionProvider provider)
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
                endpoints.MapApiHealthChecks(Configuration);
            });

            app.UseSwaggerApiExplorer(provider);

            ConfigureExtras(app, env, provider);
        }

        protected virtual void ConfigureOtherServices(IServiceCollection services) { }
        protected virtual void ConfigureOtherContainerServices(ServiceRegistry services) { }

        protected virtual void ConfigureExtras(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider provider)
        { }
    }
}
