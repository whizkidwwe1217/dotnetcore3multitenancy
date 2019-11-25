using System;
using HordeFlow.Core;
using HordeFlow.Infrastructure.Repositories;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HordeFlow.Infrastructure.Multitenancy
{
    public static class MultitenancyExtensions
    {
        public const string CURRENT_TENANT_CONTEXT_KEY = "HordeFlow.Infrastructure.Multitenancy.CurrentTenantContext";
        public const string CURRENT_TENANT_CONTAINER_KEY = "HordeFlow.Infrastructure.Multitenancy.CurrentTenantContainer";

        public static void SetCurrentTenantContext<TTenant>(this HttpContext context, TenantContext<TTenant> tenantContext) where TTenant : class
        {
            if (context.Items.ContainsKey(CURRENT_TENANT_CONTEXT_KEY))
            {
                context.Items[CURRENT_TENANT_CONTEXT_KEY] = tenantContext;
            }
            else
            {
                context.Items.Add(CURRENT_TENANT_CONTEXT_KEY, tenantContext);
            }
        }

        public static TenantContext<TTenant> GetTenantContext<TTenant>(this HttpContext context) where TTenant : class
        {
            object tenantContext;
            if (context.Items.TryGetValue(CURRENT_TENANT_CONTEXT_KEY, out tenantContext))
            {
                return tenantContext as TenantContext<TTenant>;
            }

            return null;
        }

        public static ServiceRegistry AddMultitenancy<TTenant, TResolver>(this ServiceRegistry services)
            where TResolver : class, ITenantResolver<TTenant>
            where TTenant : class
        {
            services.AddTransient<ITenantResolver<TTenant>, TResolver>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(prov => prov.GetService<IHttpContextAccessor>()?.HttpContext?.GetTenantContext<TTenant>());
            services.AddScoped(prov => prov.GetService<TenantContext<TTenant>>()?.Tenant);

            // Ensure caching is available for caching resolvers
            var resolverType = typeof(TResolver);
            if (typeof(MemoryCacheTenantResolver<TTenant>).IsAssignableFrom(resolverType))
            {
                services.AddMemoryCache();
            }

            return services;
        }

        public static IApplicationBuilder UseMultitenancy<TTenant>(this IApplicationBuilder app,
            Action<MultitenancyOptions> configure) where TTenant : class, ITenant
        {
            var options = new MultitenancyOptions();
            configure(options);

            app.UseMiddleware<TenantResolutionMiddleware<TTenant>>();

            if (options.PreventInvalidCatalogAccess)
            {
                app.UseMiddleware<InvalidCatalogAccessMiddleware<TTenant>>();
            }


            return app;
        }
    }
}