using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HordeFlow.Data.Catalog
{
    public class SqlServerCatalogDbContext : CatalogDbContext<SqlServerCatalogDbContext>
    {
        public SqlServerCatalogDbContext(IConfiguration configuration, DbContextOptions<SqlServerCatalogDbContext> options)
            : base(configuration, options)
        {
        }

        protected override void Configure(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = configuration.GetConnectionString("Catalog");
            var edition = configuration.GetValue("SQLEdition", "Latest");
            var migrationsAssembly = configuration.GetValue<string>("MigrationsAssembly", "Migrations");

            optionsBuilder.UseSqlServer(connectionString, options =>
            {
                if (edition.ToUpper().Equals("SQL2008R2"))
                    options.UseRowNumberForPaging(true);
                options.MigrationsAssembly(migrationsAssembly);
            });
        }
    }
}