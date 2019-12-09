using System;
using HordeFlow.Core;

namespace HordeFlow.Infrastructure.Repositories
{
    public interface ICatalogRepository : IRepository<Tenant, Guid>
    {

    }

    public class CatalogRepository : BaseRepository<Tenant, Guid>, ICatalogRepository
    {
        public CatalogRepository(IRepositoryManager<Guid> repositoryManager) : base(repositoryManager)
        {
        }
    }
}