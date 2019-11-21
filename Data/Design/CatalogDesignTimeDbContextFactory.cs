using System.IO;
using HordeFlow.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HordeFlow.Data.Design
{
    public class CatalogDesignTimeDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
            return new CatalogDbContext(GetConfiguration(args), optionsBuilder.Options);
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