using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HordeFlow.Multitenancy
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

                if (IsMigratingCatalog(context.Request.Path.Value))
                {
                    tenantContext = new TenantContext<TTenant>(null);
                    tenantContext.Properties.Add("SINGLE_TENANT_MIGRATION", true);
                    context.SetCurrentTenantContext(tenantContext);
                }
                else
                {
                    var resolver = context.RequestServices.GetService(typeof(ITenantResolver<TTenant>)) as ITenantResolver<TTenant>;
                    if (resolver != null)
                    {
                        logger.LogDebug("Resolving current tenant using {loggerType}.", resolver.GetType().Name);
                        tenantContext = await resolver.ResolveAsync(context);
                    }

                    if (tenantContext != null)
                    {
                        logger.LogDebug("Current tenant resolved. Adding to HttpContext.");
                        if (IsMigratingCatalog(context.Request.Path.Value))
                        {
                            tenantContext.Properties.Add("SINGLE_TENANT_MIGRATION", true);
                        }
                        context.SetCurrentTenantContext(tenantContext);
                    }
                    else
                    {
                        logger.LogDebug("Unable to resolve current tenant.");
                    }
                }
            }

            await next.Invoke(context);
        }

        private bool IsWhitelisted(string url)
        {
            if (url.StartsWith("/health")) return true;
            return false;
        }

        private bool IsMigratingCatalog(string path)
        {
            var regex = new Regex(@"^\/?api/(v|version).+(/?admin/(catalog|tenant)/(migrate|drop))", RegexOptions.IgnoreCase);
            return regex.IsMatch(path);
        }
    }
}
