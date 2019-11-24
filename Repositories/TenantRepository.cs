using System;
using HordeFlow.Models;

namespace HordeFlow.Repositories
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