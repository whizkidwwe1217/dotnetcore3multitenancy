using HordeFlow.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HordeFlow.Data
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

        public DbContextOptionsBuilder Build(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = tenant.ConnectionString;
            optionsBuilder.UseSqlite(connectionString);
            return optionsBuilder;
        }
    }
}