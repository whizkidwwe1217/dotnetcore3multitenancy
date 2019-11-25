using System.IO;
using HordeFlow.Data.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using HordeFlow.Core;

namespace HordeFlow.Data.Design
{
    public class CatalogDesignTimeDbContextFactory : IDesignTimeDbContextFactory<SqlServerCatalogDbContext>
    {
        public SqlServerCatalogDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqlServerCatalogDbContext>();
            return new SqlServerCatalogDbContext(GetConfiguration(args), optionsBuilder.Options);
        }

        protected IConfiguration GetConfiguration(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            return config;
        }
    }
}