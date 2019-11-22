using HordeFlow.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HordeFlow.Data.Design
{
    public class TenantSqlServerDesignTimeDbContextFactory : TenantDesignTimeDbContextFactory<TenantSqlServerDbContext>
    {
        protected override TenantSqlServerDbContext CreateDbContext(IConfiguration configuration, string provider)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TenantSqlServerDbContext>();
            var tenant = new Tenant { DatabaseProvider = DatabaseProvider.SqlServer, ConnectionString = ConnectionStringTemplates.SQLSERVER };
            var builder = new SqlServerDbContextConfigurationBuilder(configuration, tenant);
            return new TenantSqlServerDbContext(configuration, tenant, builder, optionsBuilder.Options);
        }
    }
}