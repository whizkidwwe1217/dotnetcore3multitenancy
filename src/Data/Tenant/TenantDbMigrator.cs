using Microsoft.EntityFrameworkCore;
using HordeFlow.Core;

namespace HordeFlow.Data
{
    public class TenantDbMigrator : DbMigrator
    {
        private readonly Tenant tenant;

        public TenantDbMigrator(DbContext dbContext, Tenant tenant, bool ensureDeleted = false)
            : base(dbContext)
        {
            this.tenant = tenant;
            EnsureDeleted = ensureDeleted;
        }
    }
}