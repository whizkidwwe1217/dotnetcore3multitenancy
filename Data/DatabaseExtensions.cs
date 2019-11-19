using i21Apis.Models;
using Lamar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace i21Apis.Data
{
    public static class DatabaseExtensions
    {
        public static ServiceRegistry AddMultiDbContext(this ServiceRegistry services)
        {
            services.For<IDbContextConfigurationBuilder>().Use(provider =>
            {
                var tenant = provider.GetService<Tenant>();

                if (tenant.DatabaseProvider.Equals("SqlServer"))
                    return provider.GetService<SqlServerDbContextConfigurationBuilder>();
                else if (tenant.DatabaseProvider.Equals("MySql"))
                    return provider.GetService<MySqlDbContextConfigurationBuilder>();
                else if (tenant.DatabaseProvider.Equals("Sqlite"))
                    return provider.GetService<SqliteDbContextConfigurationBuilder>();
                else
                    throw new System.InvalidOperationException("Invalid database provider.");
            });
            
            services.For<DbContext>().Use(provider =>
            {
                var tenant = provider.GetService<Tenant>();

                if (tenant.DatabaseProvider.Equals("SqlServer"))
                {
                    return provider.GetService<TenantSqlServerDbContext>();
                }
                else if (tenant.DatabaseProvider.Equals("MySql"))
                {
                    return provider.GetService<TenantMySqlDbContext>();
                }
                else if (tenant.DatabaseProvider.Equals("Sqlite"))
                {
                    return provider.GetService<TenantSqliteDbContext>();
                }
                else
                    throw new System.InvalidOperationException("Invalid database provider.");
            });

            return services;
        }
    }
}