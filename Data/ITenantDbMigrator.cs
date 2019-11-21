using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HordeFlow.Data
{
    public interface ITenantDbMigrator
    {
        bool EnsureDeleted { get; set; }
        Task<bool> AllMigrationsAppliedAsync(CancellationToken cancellationToken = default(CancellationToken));
        IEnumerable<string> GetAppliedMigrations();
        Task MigrateAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task DropAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}