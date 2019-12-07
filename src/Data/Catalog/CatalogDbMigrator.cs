using Microsoft.EntityFrameworkCore;

namespace HordeFlow.Data.Catalog
{
    public class CatalogDbMigrator : DbMigrator
    {
        public CatalogDbMigrator(DbContext dbContext) : base(dbContext)
        {
        }
    }
}