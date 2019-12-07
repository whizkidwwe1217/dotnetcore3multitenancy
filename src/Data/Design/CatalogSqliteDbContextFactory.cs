using HordeFlow.Data.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HordeFlow.Data.Design
{
    public class CatalogSqliteDesignTimeDbContextFactory : CatalogDesignTimeDbContextFactory<SqliteCatalogDbContext>
    {
        protected override SqliteCatalogDbContext CreateDbContext(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqliteCatalogDbContext>();
            return new SqliteCatalogDbContext(configuration, optionsBuilder.Options);
        }
    }
}