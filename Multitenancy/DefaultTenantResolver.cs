using System.Collections.Generic;
using System.Threading.Tasks;
using i21Apis.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using i21Apis.Data;

namespace i21Apis.Multitenancy
{
    public class DefaultTenantResolver : ITenantResolver<Tenant>
    {
        private readonly CatalogDbContext catalog;

        public DefaultTenantResolver(CatalogDbContext catalog)
        {
            this.catalog = catalog;
        }

        public async Task<TenantContext<Tenant>> ResolveAsync(HttpContext context)
        {
            var hostname = context.Request.Host.Value.ToLower();
            var host = context.Request.Host;

            Tenant tenant = await catalog.Set<Tenant>().Where(e => e.HostName.Contains(hostname)).FirstOrDefaultAsync();
            return await Task.FromResult(new TenantContext<Tenant>(tenant));
        }
    }
}