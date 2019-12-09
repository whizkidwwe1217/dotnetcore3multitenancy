using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HordeFlow.Core;
using HordeFlow.Infrastructure.Multitenancy;
using Microsoft.AspNetCore.Http;

namespace UnitTests
{
    public class SimpleTenantResolver : ITenantResolver<Tenant>
    {
        List<Tenant> Tenants => new List<Tenant>
        {
            new Tenant
            {
                Name = "Tenant 1",
                HostName = "localhost:8001",
                DatabaseProvider = DatabaseProvider.Sqlite,
                IsDedicated = false,
                ConnectionString = "Tenant1.db"
            },
            new Tenant
            {
                Name = "Tenant 2",
                HostName = "localhost:8002",
                DatabaseProvider = DatabaseProvider.Sqlite,
                IsDedicated = false,
                ConnectionString = "Tenant2.db"
            }
        };

        public SimpleTenantResolver()
        {
        }

        public async Task<TenantContext<Tenant>> ResolveAsync(HttpContext context)
        {
            var tenantContext = context.GetTenantContext<Tenant>();
            var hostname = context.Request.Host.Value.ToLower();
            var host = context.Request.Host;
            var tenant = Tenants.Find(e => e.HostName.Equals(hostname));
            return await Task.FromResult(new TenantContext<Tenant>(tenant));
        }
    }
}