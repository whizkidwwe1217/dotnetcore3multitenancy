using i21Apis.Data;
using i21Apis.Data.Design;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace i21Apis.Design
{
    public class MultitenantDesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            services.AddScoped<IDesignTimeDbContextFactory<TenantDbContext>>(provider =>
            {
                return provider.GetRequiredService<TenantDesignTimeDbContextFactory>();
            });
        }
    }
}