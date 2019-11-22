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
        public static ServiceRegistry AddMultiDbContext<TTenant>(this ServiceRegistry services,
            Action<MultiTenantDbContextOptions> configure) where TTenant : class, ITenant
        {
            var options = new MultiTenantDbContextOptions();
            configure(options);
            services.For(typeof(ICatalogStore<TTenant>)).Use(typeof(SqlServerCatalogStore));
            services.For(typeof(IRepositoryManager<>)).Use(typeof(RepositoryManager<>));

            services.For<IDbContextConfigurationBuilder>().Use(provider =>
            {
                var tenant = provider.GetService<TTenant>();
                if (tenant == null)
                {
                    return ThrowOrReturnNull<IDbContextConfigurationBuilder, TTenant>(tenant, options.ThrowWhenTenantIsNotFound, "Tenant not found.");
                }

                if (tenant.DatabaseProvider == DatabaseProvider.SqlServer)
                    return provider.GetService<SqlServerDbContextConfigurationBuilder>();
                else if (tenant.DatabaseProvider == DatabaseProvider.MySql)
                    return provider.GetService<MySqlDbContextConfigurationBuilder>();
                else if (tenant.DatabaseProvider == DatabaseProvider.Sqlite)
                    return provider.GetService<SqliteDbContextConfigurationBuilder>();
                else
                    return ThrowOrReturnNull<IDbContextConfigurationBuilder, TTenant>(tenant, options.ThrowWhenTenantIsNotFound, "Invalid database provider.");
            });

            services.For<DbContext>().Use(provider =>
            {
                var tenant = provider.GetService<TTenant>();

                // Configuration heirarchy: appsettings -> configuration options
                var config = provider.GetRequiredService<IConfiguration>();
                if (options.MultitenancyMode == MultitenancyMode.Single)
                {
                    return provider.GetService<SqlServerCatalogDbContext>();
                }
                else
                {
                    // Is catalog
                    if (tenant == null)
                        return provider.GetService<SqlServerCatalogDbContext>();

                    if (options.MultitenancyMode == MultitenancyMode.Multi)
                        return provider.GetService<SqlServerCatalogDbContext>();

                    // Hybrid
                    if (tenant.IsDedicated)
                        return provider.GetService<SqlServerCatalogDbContext>();

                    return ResolveTenantDbContext<TTenant>(provider, tenant, options);
                }
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

        private static DbContext ResolveTenantDbContext<TTenant>(IServiceContext provider, TTenant tenant,
            MultiTenantDbContextOptions options) where TTenant : class, ITenant
        {
            if (tenant == null)
            {
                return ThrowOrReturnNull<DbContext, TTenant>(tenant, options.ThrowWhenTenantIsNotFound, "Tenant not found.");
            }

            if (tenant.DatabaseProvider == DatabaseProvider.SqlServer)
                return provider.GetService<TenantSqlServerDbContext>();
            else if (tenant.DatabaseProvider == DatabaseProvider.MySql)
                return provider.GetService<TenantMySqlDbContext>();
            else if (tenant.DatabaseProvider == DatabaseProvider.Sqlite)
                return provider.GetService<TenantSqliteDbContext>();
            else
                return ThrowOrReturnNull<DbContext, TTenant>(tenant, options.ThrowWhenTenantIsNotFound, "Invalid database provider.");
        }

        private static TReturnType ThrowOrReturnNull<TReturnType, TTenant>(TTenant tenant, bool throwWhenTenantIsNotFound, string message) where TTenant : ITenant
        {
            if (throwWhenTenantIsNotFound) throw new System.NullReferenceException(message);
            else return default(TReturnType);
        }
    }
}