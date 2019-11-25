using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
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