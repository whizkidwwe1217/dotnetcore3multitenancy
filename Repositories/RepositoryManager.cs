using i21Apis.Models;
using Microsoft.EntityFrameworkCore;

namespace i21Apis.Repositories
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