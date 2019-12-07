using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HordeFlow.Data.Design
{
    public abstract class CatalogDesignTimeDbContextFactory<TCatalogDbContext>
        : IDesignTimeDbContextFactory<TCatalogDbContext>
        where TCatalogDbContext : DbContext
    {
        public TCatalogDbContext CreateDbContext(string[] args)
        {
            var config = GetConfiguration(args);
            return CreateDbContext(config);
        }

        protected abstract TCatalogDbContext CreateDbContext(IConfiguration configuration);

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