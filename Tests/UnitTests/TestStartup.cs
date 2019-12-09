using HordeFlow.Core;
using HordeFlow.Data;
using HordeFlow.Infrastructure;
using HordeFlow.Infrastructure.Multitenancy;
using Lamar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests
{
    public class TestStartup : BaseStartup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void ConfigureOtherContainerServices(ServiceRegistry services)
        {
            services.For<ICatalogStore<Tenant>>().Use<SimpleCatalogStore>();
        }
    }
}