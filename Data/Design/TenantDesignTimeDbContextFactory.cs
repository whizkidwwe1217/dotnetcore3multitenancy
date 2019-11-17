using System;
using System.IO;
using i21Apis.Models;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace i21Apis.Data.Design
{
    public class TenantDesignTimeDbContextFactory : IDesignTimeDbContextFactory<TenantDbContext>
    {
        public TenantDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
            var config = GetConfiguration(args);
            var provider = GetEnvironmentVariable("DatabaseProvider");

            Tenant tenant = null;
            IDbContextConfigurationBuilder contextConfigBuilder = null;

            if (provider.Equals("Sqlite"))
            {
                tenant = new Tenant { DatabaseProvider = "Sqlite", ConnectionString = ConnectionStringTemplates.SQLITE };
                contextConfigBuilder = new SqliteDbContextConfigurationBuilder(config, tenant);
            }
            else if (provider.Equals("MySql"))
            {
                tenant = new Tenant { DatabaseProvider = "MySql", ConnectionString = ConnectionStringTemplates.MYSQL };
                contextConfigBuilder = new MySqlDbContextConfigurationBuilder(config, tenant);
            }
            else
            {
                tenant = new Tenant { DatabaseProvider = "SqlServer", ConnectionString = ConnectionStringTemplates.SQLSERVER };
                contextConfigBuilder = new SqlServerDbContextConfigurationBuilder(config, tenant);
            }

            return new TenantDbContext(config, tenant, contextConfigBuilder, optionsBuilder.Options);
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

        private string GetEnvironmentVariable(string name, string errorMessage = "An error has ocurred in getting the environment variable.")
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