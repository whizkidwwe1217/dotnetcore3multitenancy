using i21Apis.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace i21Apis.Data
{
    public class SqliteDbContextConfigurationBuilder : IDbContextConfigurationBuilder
    {
        private readonly IConfiguration configuration;
        private readonly Tenant tenant;

        public SqliteDbContextConfigurationBuilder(IConfiguration configuration, Tenant tenant)
        {
            this.configuration = configuration;
            this.tenant = tenant;
        }

        public void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = tenant.ConnectionString;
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}