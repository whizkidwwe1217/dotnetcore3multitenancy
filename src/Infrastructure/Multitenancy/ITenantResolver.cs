using System.Threading.Tasks;
using HordeFlow.Core;
using Microsoft.AspNetCore.Http;

namespace HordeFlow.Infrastructure.Multitenancy
{
    public interface ITenantResolver<TTenant> where TTenant : class
    {
        Task<TenantContext<TTenant>> ResolveAsync(HttpContext context);
    }
}
