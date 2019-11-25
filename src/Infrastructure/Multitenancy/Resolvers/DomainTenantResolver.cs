using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using HordeFlow.Core;
using System;
using HordeFlow.Data;

namespace HordeFlow.Infrastructure.Multitenancy
{
    public class DomainTenantResolver<TTenant> : ITenantResolver<TTenant> where TTenant : class, ITenant
    {
        private readonly ICatalogStore<TTenant> store;

        public DomainTenantResolver(ICatalogStore<TTenant> catalog)
        {
            this.store = catalog;
        }

        public async Task<TenantContext<TTenant>> ResolveAsync(HttpContext context)
        {
            var hostname = context.Request.Host.Value.ToLower();
            var host = context.Request.Host;
            // var pos = hostname.IndexOf(".");
            // Tenant tenant = null;

            // if (pos != -1)
            // {
            //     var identifier = hostname.Substring(0, pos);
            //     if (!string.IsNullOrEmpty(identifier))
            //     {
            //         tenant = await catalog.Set<Tenant>()
            //             .Where(e => e.HostName.ToLower().Equals(identifier.ToLower()))
            //             .FirstOrDefaultAsync();
            //     }
            // }
            TTenant tenant = null;
            try
            {
                var tenants = await store.GetTenantsAsync(e => e.HostName.ToLower().Equals(hostname.ToLower()));
                tenant = tenants.FirstOrDefault();
            }
            catch (Exception ex)
            {
                tenant = null;
            }

            return await Task.FromResult(new TenantContext<TTenant>(tenant));
        }
    }
}