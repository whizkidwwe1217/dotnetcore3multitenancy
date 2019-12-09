using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using HordeFlow.Infrastructure.Controllers;
using System.Threading.Tasks;

namespace UnitTests
{
    public class TenantTest : BaseClassFixture
    {
        public TenantTest(WebApiFactory<TestStartup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task ListTenants()
        {
            var catalog = factory.Services.GetRequiredService<CatalogController>();
            Assert.NotNull(catalog);

            await catalog.Migrate();
        }
    }
}
