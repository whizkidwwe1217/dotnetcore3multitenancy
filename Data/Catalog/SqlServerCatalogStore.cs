using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using HordeFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace HordeFlow.Data.Catalog
{
    public class SqlServerCatalogStore : ICatalogStore<Tenant>
    {
        private readonly SqlServerCatalogDbContext context;

        public SqlServerCatalogStore(SqlServerCatalogDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Tenant>> GetTenantsAsync(Expression<Func<Tenant, bool>> predicate = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await context.Set<Tenant>().ToListAsync();
        }
    }
}