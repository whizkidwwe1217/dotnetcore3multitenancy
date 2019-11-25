using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HordeFlow.Core;

namespace HordeFlow.Data
{
    public class TenantSqlServerDbContext : TenantDbContext<TenantSqlServerDbContext>
    {
        public TenantSqlServerDbContext(DbContextOptions<TenantSqlServerDbContext> options)
            : base(options) { }

        public TenantSqlServerDbContext(IConfiguration configuration, Tenant tenant,
            IDbContextConfigurationBuilder builder, DbContextOptions<TenantSqlServerDbContext> options)
            : base(configuration, tenant, builder, options) { }
    }
}