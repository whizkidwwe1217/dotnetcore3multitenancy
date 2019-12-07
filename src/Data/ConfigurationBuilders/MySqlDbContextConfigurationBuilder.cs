using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using HordeFlow.Core;

namespace HordeFlow.Data
{
    public class MySqlDbContextConfigurationBuilder : IDbContextConfigurationBuilder
    {
        private readonly IConfiguration configuration;
        private readonly Tenant tenant;

        public MySqlDbContextConfigurationBuilder(IConfiguration configuration, Tenant tenant)
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

            optionsBuilder.UseMySql(connectionString,
                options =>
                {
                    options.ServerVersion(new Version(5, 7, 17), ServerType.MySql); // replace with your Server Version and Type
                    options.MigrationsAssembly(migrationsAssembly);
                });

            return optionsBuilder;
        }
    }
}