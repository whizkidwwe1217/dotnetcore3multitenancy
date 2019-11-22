using System;
using HordeFlow.Data;
using HordeFlow.Repositories;
using Lamar;
using Microsoft.EntityFrameworkCore;
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

            services.For(typeof(IRepositoryManager<>)).Use(typeof(RepositoryManager<>));
            services.For<IDbContextConfigurationBuilder>().Use(provider =>
            {
                var tenant = provider.GetRequiredService<TTenant>();
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
                var tenant = provider.GetRequiredService<TTenant>();
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
            });
            services.For<ICatalogStore<TTenant>>().Use(provider =>
            {
                return provider.GetService<ICatalogStore<TTenant>>();
            });

            return services;
        }

        private static TReturnType ThrowOrReturnNull<TReturnType, TTenant>(TTenant tenant, bool throwWhenTenantIsNotFound, string message) where TTenant : ITenant
        {
            if (throwWhenTenantIsNotFound) throw new System.NullReferenceException(message);
            else return default(TReturnType);
        }
    }
}