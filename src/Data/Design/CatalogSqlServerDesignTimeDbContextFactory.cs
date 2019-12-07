using HordeFlow.Data.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HordeFlow.Data.Design
{
    public class CatalogSqlServerDesignTimeDbContextFactory : CatalogDesignTimeDbContextFactory<SqlServerCatalogDbContext>
    {
        protected override SqlServerCatalogDbContext CreateDbContext(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqlServerCatalogDbContext>();
            return new SqlServerCatalogDbContext(configuration, optionsBuilder.Options);
        }
    }
}