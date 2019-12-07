using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace HordeFlow.Data.Catalog
{
    public class MySqlCatalogDbContext : CatalogDbContext<MySqlCatalogDbContext>
    {
        public MySqlCatalogDbContext(IConfiguration configuration, DbContextOptions<MySqlCatalogDbContext> options)
            : base(configuration, options)
        {
        }

        protected override void Configure(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = configuration.GetConnectionString("Catalog");
            var edition = configuration.GetValue("SQLEdition", "Latest");
            var migrationsAssembly = configuration.GetValue<string>("MigrationsAssembly", "Migrations");

            optionsBuilder.UseMySql(connectionString, options =>
            {
                options.ServerVersion(new Version(5, 7, 17), ServerType.MySql); // replace with your Server Version and Type
                options.MigrationsAssembly(migrationsAssembly);
            });
        }
    }
}