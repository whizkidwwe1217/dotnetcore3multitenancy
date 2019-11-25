using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HordeFlow.Core;

namespace HordeFlow.Data
{
    public class TenantSqliteDbContext : TenantDbContext<TenantSqliteDbContext>
    {
        public TenantSqliteDbContext(DbContextOptions<TenantSqliteDbContext> options)
            : base(options) { }

        public TenantSqliteDbContext(IConfiguration configuration, Tenant tenant,
            IDbContextConfigurationBuilder builder, DbContextOptions<TenantSqliteDbContext> options)
            : base(configuration, tenant, builder, options) { }

    }
}