using HordeFlow.Core;
using Microsoft.EntityFrameworkCore;

namespace HordeFlow.Infrastructure.Repositories
{
    public interface IRepositoryManager<TKey>
    {
        DbContext DbContext { get; set; }
        Tenant Tenant { get; set; }
    }
}