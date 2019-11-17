using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace i21Apis.Multitenancy
{
    public class TenantResolutionMiddleware<TTenant> where TTenant : class
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public TenantResolutionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next;
            this.logger = loggerFactory.CreateLogger<TenantResolutionMiddleware<TTenant>>();
        }

        public async Task Invoke(HttpContext context)
        {
            var hostname = context.Request.Host.Value.ToLower();
            var host = context.Request.Host;

            if (!IsWhitelisted(context.Request.Path))
            {
                TenantContext<TTenant> tenantContext = null;
                var resolver = context.RequestServices.GetService(typeof(ITenantResolver<TTenant>)) as ITenantResolver<TTenant>;
                if (resolver != null)
                {
                    logger.LogDebug("Resolving current tenant using {loggerType}.", resolver.GetType().Name);
                    tenantContext = await resolver.ResolveAsync(context);
                }

                if (tenantContext != null)
                {
                    logger.LogDebug("Current tenant resolved. Adding to HttpContext.");
                    context.SetCurrentTenantContext(tenantContext);
                }
                else
                {
                    logger.LogDebug("Unable to resolve current tenant.");
                }
            }

            await next.Invoke(context);
        }

        private bool IsWhitelisted(string url)
        {
            if (url.StartsWith("/health")) return true;
            return false;
        }
    }
}
