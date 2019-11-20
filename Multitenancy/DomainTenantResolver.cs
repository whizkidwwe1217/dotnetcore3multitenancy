using System.Collections.Generic;
using System.Threading.Tasks;
using i21Apis.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using i21Apis.Data;

namespace i21Apis.Multitenancy
{
    public class DomainTenantResolver : ITenantResolver<Tenant>
    {
        private readonly CatalogDbContext catalog;

        public DomainTenantResolver(CatalogDbContext catalog)
        {
            this.catalog = catalog;
        }

        public async Task<TenantContext<Tenant>> ResolveAsync(HttpContext context)
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
            Tenant tenant = await catalog.Set<Tenant>().Where(e => e.HostName.Contains(hostname)).FirstOrDefaultAsync();

            return await Task.FromResult(new TenantContext<Tenant>(tenant));
        }
    }
}