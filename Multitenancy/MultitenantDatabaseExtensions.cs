using System;
using HordeFlow.Data;
using HordeFlow.Data.Catalog;
using HordeFlow.Repositories;
using Lamar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HordeFlow.Multitenancy
{
    public struct MultiTenantDbContextOptions
    {
        public bool ThrowWhenTenantIsNotFound { get; set; }
        public MultitenancyMode MultitenancyMode { get; set; }
    }

    public static class MultitenantDatabaseExtensions
    {
        public static ServiceRegistry AddMultiDbContext<TTenant>(this ServiceRegistry services) where TTenant : class, ITenant
        {
            services.For(typeof(ICatalogStore<TTenant>)).Use(typeof(SqlServerCatalogStore));
            services.For(typeof(IRepositoryManager<>)).Use(typeof(RepositoryManager<>));

            services.For<IDbContextConfigurationBuilder>().Use(provider =>
            {
                var tenant = provider.GetService<TTenant>();
                var tenantContext = provider.GetService<TenantContext<TTenant>>();
                var config = provider.GetRequiredService<IConfiguration>();
                var mode = config.GetValue<MultitenancyMode>("MultitenancyMode", MultitenancyMode.Single);
                var options = new MultiTenantDbContextOptions
                {
                    MultitenancyMode = mode
                };

                if (tenant != null)
                {
                    if ((options.MultitenancyMode == MultitenancyMode.Single || options.MultitenancyMode == MultitenancyMode.Hybrid) 
                        && tenantContext.Properties.ContainsKey("SINGLE_TENTANT_MIGRATION"))
                    {
                        return ResolveConfigurationBuilder(provider, DatabaseProvider.SqlServer);
                    }
                    else
                    {
                        return ResolveConfigurationBuilder(provider, tenant.DatabaseProvider);
                    }
                }

                return ResolveConfigurationBuilder(provider, DatabaseProvider.SqlServer);
            });

            services.For<DbContext>().Use(provider =>
            {
                var tenant = provider.GetService<TTenant>();
                var tenantContext = provider.GetService<TenantContext<TTenant>>();
                var config = provider.GetRequiredService<IConfiguration>();
                var mode = config.GetValue<MultitenancyMode>("MultitenancyMode", MultitenancyMode.Single);
                var options = new MultiTenantDbContextOptions
                {
                    MultitenancyMode = mode
                };

                if (options.MultitenancyMode == MultitenancyMode.Single || options.MultitenancyMode == MultitenancyMode.Hybrid)
                {
                    if (tenantContext.Properties.ContainsKey("SINGLE_TENANT_MIGRATION"))
                    {
                        return ResolveTenantDbContext<TTenant>(provider, DatabaseProvider.SqlServer, options);
                    }
                }

                // Is catalog
                if (tenant == null)
                    return provider.GetService<SqlServerCatalogDbContext>();

                return ResolveTenantDbContext<TTenant>(provider, tenant.DatabaseProvider, options);

            });

            services.For<IDbMigrator>().Use(provider =>
            {
                var tenant = provider.GetService<TTenant>();
                if (tenant == null)
                    return provider.GetService<CatalogDbMigrator>();
                return provider.GetService<TenantDbMigrator>();
            });

            return services;
        }

        private static IDbContextConfigurationBuilder ResolveConfigurationBuilder(
            IServiceContext provider,
            DatabaseProvider databaseProvider)
        {
            if (databaseProvider == DatabaseProvider.MySql)
                return provider.GetService<MySqlDbContextConfigurationBuilder>();
            else if (databaseProvider == DatabaseProvider.Sqlite)
                return provider.GetService<SqliteDbContextConfigurationBuilder>();
            else
                return provider.GetService<SqlServerDbContextConfigurationBuilder>();
        }

        private static DbContext ResolveTenantDbContext<TTenant>(IServiceContext provider,
            DatabaseProvider databaseProvider,
            MultiTenantDbContextOptions options)
            where TTenant : class, ITenant
        {
            if (databaseProvider == DatabaseProvider.MySql)
                return provider.GetService<TenantMySqlDbContext>();
            else if (databaseProvider == DatabaseProvider.Sqlite)
                return provider.GetService<TenantSqliteDbContext>();
            else
                return provider.GetService<TenantSqlServerDbContext>();
        }

        private static TReturnType ThrowOrReturnNull<TReturnType, TTenant>(TTenant tenant, bool throwWhenTenantIsNotFound, string message) where TTenant : ITenant
        {
            if (throwWhenTenantIsNotFound) throw new System.NullReferenceException(message);
            else return default(TReturnType);
        }
    }
}