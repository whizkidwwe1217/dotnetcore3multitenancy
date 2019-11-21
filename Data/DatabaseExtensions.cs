using System;
using HordeFlow.Models;
using HordeFlow.Repositories;
using Lamar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HordeFlow.Data
{
    public struct MultiTenantDbContextOptions
    {
        public bool ThrowWhenTenantIsNotFound { get; set; }
    }

    public static class DatabaseExtensions
    {
        public static ServiceRegistry AddMultiDbContext<TTenant>(this ServiceRegistry services,
            Action<MultiTenantDbContextOptions> configure) where TTenant : ITenant
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

                if (tenant.DatabaseProvider.Equals("SqlServer"))
                    return provider.GetService<SqlServerDbContextConfigurationBuilder>();
                else if (tenant.DatabaseProvider.Equals("MySql"))
                    return provider.GetService<MySqlDbContextConfigurationBuilder>();
                else if (tenant.DatabaseProvider.Equals("Sqlite"))
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

                if (tenant.DatabaseProvider.Equals("SqlServer"))
                    return provider.GetService<TenantSqlServerDbContext>();
                else if (tenant.DatabaseProvider.Equals("MySql"))
                    return provider.GetService<TenantMySqlDbContext>();
                else if (tenant.DatabaseProvider.Equals("Sqlite"))
                    return provider.GetService<TenantSqliteDbContext>();
                else
                    return ThrowOrReturnNull<DbContext, TTenant>(tenant, options.ThrowWhenTenantIsNotFound, "Invalid database provider.");
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