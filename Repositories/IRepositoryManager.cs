using i21Apis.Models;
using Microsoft.EntityFrameworkCore;

namespace i21Apis.Repositories
{
    public interface IRepositoryManager<TKey>
    {
        DbContext DbContext { get; set; }
        Tenant Tenant { get; set; }
    }
}