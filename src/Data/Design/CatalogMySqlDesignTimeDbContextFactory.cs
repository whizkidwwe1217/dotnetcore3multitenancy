using HordeFlow.Data.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HordeFlow.Data.Design
{
    public class CatalogMySqlDesignTimeDbContextFactory : CatalogDesignTimeDbContextFactory<MySqlCatalogDbContext>
    {
        protected override MySqlCatalogDbContext CreateDbContext(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MySqlCatalogDbContext>();
            return new MySqlCatalogDbContext(configuration, optionsBuilder.Options);
        }
    }
}