using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using HordeFlow.Infrastructure.Controllers;

namespace UnitTests
{
    public class TenantTest : BaseClassFixture
    {
        public TenantTest(WebApiFactory<TestStartup> factory) : base(factory)
        {
        }

        [Fact]
        public void ListTenants()
        {
            var catalog = factory.Services.GetRequiredService<CatalogController>();
            Assert.NotNull(catalog);
        }
    }
}
