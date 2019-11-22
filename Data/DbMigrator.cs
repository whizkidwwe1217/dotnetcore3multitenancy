using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HordeFlow.Data
{
    public abstract class DbMigrator : IDbMigrator
    {
        public DbMigrator(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public bool EnsureDeleted { get; set; }
        public DbContext DbContext { get; set; }

        public async Task MigrateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (EnsureDeleted)
                await DropAsync(cancellationToken);

            if (!(await AllMigrationsAppliedAsync(cancellationToken)))
                await DbContext.Database.MigrateAsync(cancellationToken);
            else
                await DbContext.Database.EnsureCreatedAsync(cancellationToken);
        }

        public async Task DropAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await DbContext.Database.EnsureDeletedAsync(cancellationToken);
        }

        public async Task<bool> AllMigrationsAppliedAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var migrations = await DbContext.GetService<IHistoryRepository>()
                .GetAppliedMigrationsAsync();
            var applied = migrations.Select(m => m.MigrationId);

            var total = DbContext.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public IEnumerable<string> GetAppliedMigrations()
        {
            var migrations = DbContext.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(e => e.Key);
            return migrations.ToList();
        }
    }
}