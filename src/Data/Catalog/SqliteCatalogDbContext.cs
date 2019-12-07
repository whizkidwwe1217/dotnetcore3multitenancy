using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HordeFlow.Data.Catalog
{
    public class SqliteCatalogDbContext : CatalogDbContext<SqliteCatalogDbContext>
    {
        public SqliteCatalogDbContext(IConfiguration configuration, DbContextOptions<SqliteCatalogDbContext> options)
            : base(configuration, options)
        {
        }

        protected override void Configure(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = configuration.GetConnectionString("Catalog");
            var edition = configuration.GetValue("SQLEdition", "Latest");
            var migrationsAssembly = configuration.GetValue<string>("MigrationsAssembly", "Migrations");

            optionsBuilder.UseSqlite(connectionString, options =>
            {
                options.MigrationsAssembly(migrationsAssembly);
            });
        }
    }
}