using System;
using HordeFlow.Core;

namespace HordeFlow.Infrastructure.Repositories
{
    public interface ITenantRepository : IRepository<Guid, Tenant>
    {

    }

    public class TenantRepository : BaseRepository<Guid, Tenant>, ITenantRepository
    {
        public TenantRepository(IRepositoryManager<Guid> repositoryManager) : base(repositoryManager)
        {
        }
    }
}