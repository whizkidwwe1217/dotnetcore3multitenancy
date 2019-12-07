using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HordeFlow.Core;

namespace HordeFlow.Data.Design
{
    public class TenantSqliteDesignTimeDbContextFactory : TenantDesignTimeDbContextFactory<TenantSqliteDbContext>
    {
        protected override TenantSqliteDbContext CreateDbContext(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TenantSqliteDbContext>();
            var tenant = new Tenant { DatabaseProvider = DatabaseProvider.Sqlite, ConnectionString = ConnectionStringTemplates.SQLITE };
            var builder = new SqliteDbContextConfigurationBuilder(configuration, tenant);
            return new TenantSqliteDbContext(configuration, tenant, builder, optionsBuilder.Options);
        }
    }
}