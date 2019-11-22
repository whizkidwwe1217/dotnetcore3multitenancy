using System.Threading.Tasks;
using HordeFlow.Data;
using Microsoft.AspNetCore.Http;

namespace HordeFlow.Multitenancy
{
    public interface ITenantResolver<TTenant> where TTenant : class
    {
        Task<TenantContext<TTenant>> ResolveAsync(HttpContext context);
    }
}
