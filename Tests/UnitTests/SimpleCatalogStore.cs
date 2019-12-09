using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using HordeFlow.Core;
using HordeFlow.Data;

namespace UnitTests
{
    public class SimpleCatalogStore : ICatalogStore<Tenant>
    {
        IList<Tenant> Tenants => new List<Tenant>
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

        public async Task<List<Tenant>> GetTenantsAsync(Expression<Func<Tenant, bool>> predicate = null,
            CancellationToken cancellationToken = default)
        {
            if (predicate != null)
            {
                return await Task.FromResult(Tenants.Where(predicate.Compile()).ToList());
            }
            return await Task.FromResult(Tenants.ToList());
        }
    }
}