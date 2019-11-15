using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace i21Apis.Multitenancy
{
    public interface ITenantResolver<TTenant> where TTenant : class
    {
        Task<TenantContext<TTenant>> ResolveAsync(HttpContext context);
    }
}
