using Microsoft.EntityFrameworkCore;
using HordeFlow.Core;

namespace HordeFlow.Data.Catalog
{
    public class CatalogDbMigrator : DbMigrator
    {
        public CatalogDbMigrator(DbContext dbContext) : base(dbContext)
        {
        }
    }
}