using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HordeFlow.Data;
using HordeFlow.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace HordeFlow.Multitenancy
{
    public class CachedDomainTenantResolver : MemoryCacheTenantResolver<ITenant>
    {
        private readonly ICatalogStore<ITenant> store;
        public CachedDomainTenantResolver(ICatalogStore<ITenant> catalog, IMemoryCache cache, ILoggerFactory loggerFactory) : base(cache, loggerFactory)
        {
            this.store = catalog ?? throw new System.ArgumentNullException(nameof(catalog));
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

        protected override IEnumerable<string> GetTenantIdentifiers(TenantContext<ITenant> context)
        {
            string[] identifiers = new string[0];
            if (context.Tenant == null)
                return new string[0];
            return new string[] { context.Tenant.HostName };
        }

        protected async override Task<TenantContext<ITenant>> ResolveAsync(HttpContext context)
        {
            var hostname = context.Request.Host.Value.ToLower();
            var host = context.Request.Host;
            var pos = hostname.IndexOf(".");
            ITenant tenant = null;

            if (pos != -1)
            {
                var identifier = hostname.Substring(0, pos);
                if (!string.IsNullOrEmpty(identifier))
                {
                    var tenants = await store.GetTenantsAsync(e => e.HostName.ToLower().Equals(identifier.ToLower()));
                    tenant = tenants.First();
                }
            }

            return await Task.FromResult(new TenantContext<ITenant>(tenant));
        }
    }
}