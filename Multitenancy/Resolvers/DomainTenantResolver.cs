using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using HordeFlow.Data;

namespace HordeFlow.Multitenancy
{
    public class DomainTenantResolver : ITenantResolver<ITenant>
    {
        private readonly ICatalogStore<ITenant> store;

        public DomainTenantResolver(ICatalogStore<ITenant> catalog)
        {
            this.store = catalog;
        }

        public async Task<TenantContext<ITenant>> ResolveAsync(HttpContext context)
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
            var tenants = await store.GetTenantsAsync(e => e.HostName.ToLower().Equals(hostname.ToLower()));
            var tenant = tenants.First();

            return await Task.FromResult(new TenantContext<ITenant>(tenant));
        }
    }
}