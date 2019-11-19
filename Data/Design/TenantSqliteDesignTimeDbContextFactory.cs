using i21Apis.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace i21Apis.Data.Design
{
    public class TenantSqliteDesignTimeDbContextFactory : TenantDesignTimeDbContextFactory<TenantSqliteDbContext>
    {
        protected override TenantSqliteDbContext CreateDbContext(IConfiguration configuration, string provider)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TenantSqliteDbContext>();
            var tenant = new Tenant { DatabaseProvider = "Sqlite", ConnectionString = ConnectionStringTemplates.MYSQL };
            var builder = new MySqlDbContextConfigurationBuilder(configuration, tenant);
            return new TenantSqliteDbContext(configuration, tenant, builder, optionsBuilder.Options);
        }
    }
}