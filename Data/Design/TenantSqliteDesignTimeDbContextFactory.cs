using HordeFlow.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HordeFlow.Data.Design
{
    public class TenantSqliteDesignTimeDbContextFactory : TenantDesignTimeDbContextFactory<TenantSqliteDbContext>
    {
        protected override TenantSqliteDbContext CreateDbContext(IConfiguration configuration, string provider)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TenantSqliteDbContext>();
            var tenant = new Tenant { DatabaseProvider = "Sqlite", ConnectionString = ConnectionStringTemplates.SQLITE };
            var builder = new SqliteDbContextConfigurationBuilder(configuration, tenant);
            return new TenantSqliteDbContext(configuration, tenant, builder, optionsBuilder.Options);
        }
    }
}