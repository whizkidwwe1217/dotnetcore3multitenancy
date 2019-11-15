using System.Collections.Generic;
using System.Threading.Tasks;
using i21Apis.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace i21Apis.Multitenancy
{
    public class DefaultTenantResolver : ITenantResolver<Tenant>
    {
        private readonly CatalogDbContext catalog;

        public DefaultTenantResolver(CatalogDbContext catalog)
        {
            this.catalog = catalog;
        }

        public List<Tenant> GetTenants()
        {
            var tenants = new List<Tenant>() {
                new Tenant { Name = "iRely", ConnectionString = "Data Source=.\\SQL2017;Initial Catalog=i21;User=sa;Password=masterkey;", HostName = "localhost:8000" },
                new Tenant { Name = "MCP", ConnectionString = "Data Source=.\\SQL2017;Initial Catalog=mcp;User=sa;Password=masterkey;", HostName = "localhost:8001" },
                new Tenant { Name = "JDE", ConnectionString = "Data Source=.\\SQL2017;Initial Catalog=jde;User=sa;Password=masterkey;", HostName = "localhost:8002" }
            };

            return tenants;
        }

        public async Task<TenantContext<Tenant>> ResolveAsync(HttpContext context)
        {
            var hostname = context.Request.Host.Value.ToLower();
            var host = context.Request.Host;
            Tenant tenant = await catalog.Set<Tenant>().Where(e => e.HostName.Equals(hostname)).FirstOrDefaultAsync();
            return await Task.FromResult(new TenantContext<Tenant>(tenant));
        }
    }
}