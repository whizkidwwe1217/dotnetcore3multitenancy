using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HordeFlow.Core;

namespace HordeFlow.Data
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

        public DbContextOptionsBuilder Build(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = tenant?.ConnectionString;
            var mode = configuration.GetValue<string>("MultitenancyMode", "Single");
            var migrationsAssembly = configuration.GetValue<string>("MigrationsAssembly", "Migrations");

            if (mode == "Single" || mode == "Hybrid")
            {
                connectionString = configuration.GetConnectionString("Catalog");
                if (mode == "Hybrid" && tenant != null && tenant.IsDedicated)
                    connectionString = tenant.ConnectionString;
            }

            var edition = configuration.GetValue("SQLEdition", "Latest");
            optionsBuilder.UseSqlServer(connectionString, options =>
            {
                options.UseRowNumberForPaging(edition.ToUpper().Equals("SQL2008R2"));
                options.MigrationsAssembly(migrationsAssembly);
            });

            return optionsBuilder;
        }
    }
}