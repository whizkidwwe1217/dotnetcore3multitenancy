using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HordeFlow.Data.Design
{
    public abstract class TenantDesignTimeDbContextFactory<TDbContext> : IDesignTimeDbContextFactory<TDbContext>
        where TDbContext : DbContext
    {
        public TDbContext CreateDbContext(string[] args)
        {
            var config = GetConfiguration(args);
            var provider = GetEnvironmentVariable("DatabaseProvider");
            return CreateDbContext(config, provider);
        }

        protected abstract TDbContext CreateDbContext(IConfiguration configuration, string provider);

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

        protected string GetEnvironmentVariable(string name, string errorMessage = "An error has ocurred in getting the environment variable.")
        {
            var value = Environment.GetEnvironmentVariable(name);

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception(errorMessage);
            }

            return value;
        }
    }
}