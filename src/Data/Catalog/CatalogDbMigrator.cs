using HordeFlow.Core;
using Microsoft.EntityFrameworkCore;

namespace HordeFlow.Data.Catalog
{
    public class CatalogDbMigrator : DbMigrator
    {
        public CatalogDbMigrator(DbContext context) : base(context)
        {
        }
    }
}