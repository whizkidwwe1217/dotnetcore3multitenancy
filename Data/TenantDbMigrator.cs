using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using i21Apis.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace i21Apis.Data
{
    public class TenantDbMigrator : ITenantDbMigrator
    {
        private readonly Tenant tenant;
        private readonly DbContext context;

        public TenantDbMigrator(Tenant tenant, DbContext context, bool ensureDeleted = false)
        {
            this.tenant = tenant;
            this.context = context;
            EnsureDeleted = ensureDeleted;
        }

        public bool EnsureDeleted { get; set; }

        public async Task MigrateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (EnsureDeleted)
                await DropAsync(cancellationToken);

            if (!(await AllMigrationsAppliedAsync(cancellationToken)))
                await context.Database.MigrateAsync(cancellationToken);
            else
                await context.Database.EnsureCreatedAsync(cancellationToken);
        }

        public async Task DropAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await context.Database.EnsureDeletedAsync(cancellationToken);
        }

        public async Task<bool> AllMigrationsAppliedAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var migrations = await context.GetService<IHistoryRepository>()
                .GetAppliedMigrationsAsync();
            var applied = migrations.Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public IEnumerable<string> GetAppliedMigrations()
        {
            var migrations = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(e => e.Key);
            return migrations.ToList();
        }
    }
}