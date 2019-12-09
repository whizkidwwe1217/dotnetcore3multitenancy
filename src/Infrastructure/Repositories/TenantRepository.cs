using System;
using System.Threading;
using System.Threading.Tasks;
using HordeFlow.Core;
using HordeFlow.Core.Common;
using HordeFlow.Data;

namespace HordeFlow.Infrastructure.Repositories
{
    public interface ITenantRepository : IRepository<Tenant, Guid>
    {
        Task<bool> MigrateAsync(CancellationToken cancellationToken = default(CancellationToken));
    }

    public class TenantRepository : BaseRepository<Tenant, Guid>, ITenantRepository
    {
        private readonly IDbMigrator migrator;

        public TenantRepository(IRepositoryManager<Guid> repositoryManager,
            IDbMigrator migrator) : base(repositoryManager)
        {
            this.migrator = migrator;
        }

        public async Task<bool> MigrateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                await migrator.MigrateAsync();
                return true;
            }
            catch (Exception ex)
            {
                RepositoryUtils.ThrowException(this, ex.Message, ex);
            }

            return false;
        }
    }
}