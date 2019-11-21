using HordeFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace HordeFlow.Repositories
{
    public interface IRepositoryManager<TKey>
    {
        DbContext DbContext { get; set; }
        Tenant Tenant { get; set; }
    }
}