using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HordeFlow.Core;
using Microsoft.AspNetCore.Http;

namespace HordeFlow.Infrastructure.Multitenancy
{
    public class InvalidCatalogAccessMiddleware<TTenant> where TTenant : class, ITenant
    {
        private readonly RequestDelegate next;

        public InvalidCatalogAccessMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        private bool IsAccessingCatalogPath(string path)
        {
            // Matches '/api/v1/admin'
            var regex = new Regex(@"^\/?api/(v|version).+/admin/?(/+.?)?", RegexOptions.IgnoreCase);
            return regex.IsMatch(path);
        }

        private bool IsNonApiPathOrWhitelisted(string path)
        {
            // Matches 'api/v1/app/*' and 'api/v1/hubs' or non-api
            var regex = new Regex(@"^(\/?api/(?=((?!/).)*/(app|hubs))(v|version).+)", RegexOptions.IgnoreCase);
            return path.StartsWith("/api") == false || !regex.IsMatch(path);
        }

        public async Task Invoke(HttpContext context)
        {
            var tenant = context.GetTenantContext<TTenant>()?.Tenant;

            // Tenant is attempting to access catalog admin endpoints
            if (tenant != null && IsAccessingCatalogPath(context.Request.Path))
                context.Response.StatusCode = 404;
            // Admin is accessing non-admin/non-catalog endpoints
            else if (tenant == null && !IsAccessingCatalogPath(context.Request.Path) && !IsNonApiPathOrWhitelisted(context.Request.Path))
            {
                context.Response.StatusCode = 404;
            }
            else
            {
                if (tenant == null && !(IsAccessingCatalogPath(context.Request.Path) || IsNonApiPathOrWhitelisted(context.Request.Path)))
                    context.Response.StatusCode = 404;
                else
                    await this.next.Invoke(context);
            }
        }
    }
}