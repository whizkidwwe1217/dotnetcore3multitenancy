using HordeFlow.Core;
using HordeFlow.Infrastructure.Extensions;
using HordeFlow.Infrastructure.HealthChecks;
using HordeFlow.Infrastructure.Multitenancy;
using HordeFlow.Infrastructure.Repositories;
using HordeFlow.Infrastructure.Swagger;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

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

            services.AddLogging();
            services
            .AddMultitenancy<Tenant, DomainTenantResolver<Tenant>>()
            .AddMultiDbContext<Tenant>()
            .AddHealthChecks()
            .AddCheck<TenantDbHealthCheck>("tenant-db-health", failureStatus: HealthStatus.Degraded);
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
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                // add a custom operation filter which sets default values
                options.OperationFilter<SwaggerDefaultValues>();
            });

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
                endpoints
                .MapHealthChecks(Configuration.GetSection("HealthChecks").GetValue<string>("Endpoint"))
                .RequireHost(Configuration.GetSection("HealthChecks").GetValue<string>("HostFilter"));
            });

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

            ConfigureExtras(app, env, provider);
        }

        protected virtual void ConfigureOtherServices(IServiceCollection services)
        {

        }

        protected virtual void ConfigureExtras(IApplicationBuilder app, IWebHostEnvironment env,
            IApiVersionDescriptionProvider provider)
        {

        }
    }
}
