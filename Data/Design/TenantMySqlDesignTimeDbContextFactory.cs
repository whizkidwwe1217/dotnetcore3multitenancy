using HordeFlow.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HordeFlow.Data.Design
{
    public class TenantMySqlDesignTimeDbContextFactory : TenantDesignTimeDbContextFactory<TenantMySqlDbContext>
    {
        protected override TenantMySqlDbContext CreateDbContext(IConfiguration configuration, string provider)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TenantMySqlDbContext>();
            var tenant = new Tenant { DatabaseProvider = DatabaseProvider.MySql, ConnectionString = ConnectionStringTemplates.MYSQL };
            var builder = new MySqlDbContextConfigurationBuilder(configuration, tenant);
            return new TenantMySqlDbContext(configuration, tenant, builder, optionsBuilder.Options);
        }
    }
}