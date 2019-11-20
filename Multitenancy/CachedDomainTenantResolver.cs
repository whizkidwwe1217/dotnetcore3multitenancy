using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using i21Apis.Data;
using i21Apis.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace i21Apis.Multitenancy
{
    public class CachedDomainTenantResolver : MemoryCacheTenantResolver<Tenant>
    {
        private readonly CatalogDbContext catalog;
        public CachedDomainTenantResolver(CatalogDbContext catalog, IMemoryCache cache, ILoggerFactory loggerFactory) : base(cache, loggerFactory)
        {
            this.catalog = catalog ?? throw new System.ArgumentNullException(nameof(catalog));
        }

        private bool IsPathIsInCatalogWhitelist(string path)
        {
            var regex = new Regex(@"^\/?api/(v|version).+(/?admin/tenantcatalog/(migrate|drop))", RegexOptions.IgnoreCase);
            return regex.IsMatch(path);
        }

        protected override string GetContextIdentifier(HttpContext context)
        {
            return context.Request.Host.Value.ToLower();
        }

        protected override IEnumerable<string> GetTenantIdentifiers(TenantContext<Tenant> context)
        {
            string[] identifiers = new string[0];
            if (context.Tenant == null)
                return new string[0];
            return new string[] { context.Tenant.HostName };
        }

        protected async override Task<TenantContext<Tenant>> ResolveAsync(HttpContext context)
        {
            var hostname = context.Request.Host.Value.ToLower();
            var host = context.Request.Host;
            var pos = hostname.IndexOf(".");
            Tenant tenant = null;

            if (pos != -1)
            {
                var identifier = hostname.Substring(0, pos);
                if (!string.IsNullOrEmpty(identifier))
                {
                    tenant = await catalog.Set<Tenant>()
                        .Where(e => e.HostName.ToLower().Equals(identifier.ToLower()))
                        .FirstOrDefaultAsync();
                }
            }

            return await Task.FromResult(new TenantContext<Tenant>(tenant));
        }
    }
}