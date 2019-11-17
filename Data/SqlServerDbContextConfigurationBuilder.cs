using i21Apis.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace i21Apis.Data
{
    public class SqlServerDbContextConfigurationBuilder : IDbContextConfigurationBuilder
    {
        private readonly IConfiguration configuration;
        private readonly Tenant tenant;

        public SqlServerDbContextConfigurationBuilder(IConfiguration configuration, Tenant tenant)
        {
            this.configuration = configuration;
            this.tenant = tenant;
        }

        public void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = tenant.ConnectionString;
            var edition = configuration.GetValue("SQLEdition", "Latest");
            optionsBuilder.UseSqlServer(connectionString, options =>
            {
                options.UseRowNumberForPaging(edition.ToUpper().Equals("SQL2008R2"));
            });
        }
    }
}