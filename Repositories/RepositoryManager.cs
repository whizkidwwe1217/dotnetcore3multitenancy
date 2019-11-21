using HordeFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace HordeFlow.Repositories
{
    public class RepositoryManager<TKey> : IRepositoryManager<TKey>
    {
        public RepositoryManager(DbContext dbContext, Tenant tenant)
        {
            DbContext = dbContext;
            Tenant = tenant;
        }

        public DbContext DbContext { get; set; }
        public Tenant Tenant { get; set; }
    }
}