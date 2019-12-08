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
            return path.StartsWith("/api") == false || regex.IsMatch(path);
        }

        public async Task Invoke(HttpContext context)
        {
            var tenant = context.GetTenantContext<TTenant>()?.Tenant;
            var path = context.Request.Path;

            if (tenant != null)
            {
                if (IsAccessingCatalogPath(path))
                    context.Response.StatusCode = 403;
                else
                    await next.Invoke(context);
            }
            else
            {
                if (IsAccessingCatalogPath(path) || IsNonApiPathOrWhitelisted(path))
                    await next.Invoke(context);
                else
                    context.Response.StatusCode = 404;
            }
        }
    }
}