using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HordeFlow.Core;

namespace HordeFlow.Data.Catalog
{
    public class SqlServerCatalogStore<TTenant> : ICatalogStore<TTenant>
        where TTenant : class
    {
        private readonly SqlServerCatalogDbContext context;

        public SqlServerCatalogStore(SqlServerCatalogDbContext context)
        {
            this.context = context;
        }

        public SqlServerCatalogDbContext Context => context;

        public async Task<List<TTenant>> GetTenantsAsync(Expression<Func<TTenant, bool>> predicate = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (predicate != null)
                return await Context.Set<TTenant>().AsNoTracking().Where(predicate).ToListAsync();
            return await Context.Set<TTenant>().AsNoTracking().ToListAsync();
        }
    }

    public class MySqlCatalogStore<TTenant> : ICatalogStore<TTenant> where TTenant : class
    {
        private readonly MySqlCatalogDbContext context;

        public MySqlCatalogStore(MySqlCatalogDbContext context)
        {
            this.context = context;
        }

        public MySqlCatalogDbContext Context => context;

        public async Task<List<TTenant>> GetTenantsAsync(Expression<Func<TTenant, bool>> predicate = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (predicate != null)
                return await context.Set<TTenant>().AsNoTracking().Where(predicate).ToListAsync();
            return await context.Set<TTenant>().AsNoTracking().ToListAsync();
        }
    }
    public class SqliteCatalogStore<TTenant> : ICatalogStore<TTenant> where TTenant : class
    {
        private readonly SqliteCatalogDbContext context;

        public SqliteCatalogStore(SqliteCatalogDbContext context)
        {
            this.context = context;
        }

        public SqliteCatalogDbContext Context => context;

        public async Task<List<TTenant>> GetTenantsAsync(Expression<Func<TTenant, bool>> predicate = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (predicate != null)
                return await context.Set<TTenant>().AsNoTracking().Where(predicate).ToListAsync();
            return await context.Set<TTenant>().AsNoTracking().ToListAsync();
        }
    }
}