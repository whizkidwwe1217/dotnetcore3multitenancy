using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using HordeFlow.Infrastructure.Controllers;
using System.Threading.Tasks;
using HordeFlow.Infrastructure.Repositories;
using HordeFlow.Core;
using System.Threading;
using HordeFlow.Infrastructure.Multitenancy;
using Microsoft.AspNetCore.Http;

namespace UnitTests
{
    [TestCaseOrderer("UnitTests.PriorityOrderer", "UnitTests")]
    public class TenantTest : BaseClassFixture
    {
        private ICatalogRepository repository;

        public TenantTest(WebApiFactory<TestStartup> factory) : base(factory)
        {
            this.repository = factory.Services.GetService<ICatalogRepository>();
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            Assert.NotNull(repository);
            await repository.AddAsync(new Tenant
            {
                Name = "Tenant 1",
                HostName = "localhost:8001",
                IsDedicated = false,
                DatabaseProvider = DatabaseProvider.Sqlite,
                ConnectionString = "Data Source=tenant.db"
            });
            await repository.SaveAsync();
        }

        [Fact]
        public async Task Should_Have_Tenants()
        {
            var tenants = await repository.ListAsync();
            Assert.NotEmpty(tenants);
        }

        [Fact]
        public async Task Should_Have_Tenant1()
        {
            var tenant = await repository.GetAsync(e => e.Name.Equals("Tenant 1"));
            Assert.NotNull(tenant);
            Assert.Equal("localhost:8001", tenant.HostName);
        }

        [Fact, TestPriority(1)]
        public async Task Should_Migrate_Tenant1()
        {
            var repo = factory.Services.GetRequiredService<ITenantRepository>();
            var result = await repo.MigrateAsync();
            Assert.True(result);
        }

        [Fact, TestPriority(2)]
        public async Task Should_Resolve_Tenant1()
        {
            var repo = factory.Services.GetRequiredService<ITenantResolver<Tenant>>();
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("localhost:8001");
            var result = await repo.ResolveAsync(context);
            Assert.NotNull(result.Tenant);
        }

        [Fact, TestPriority(2)]
        public async Task Should_Not_Resolve_Tenant1()
        {
            var repo = factory.Services.GetRequiredService<ITenantResolver<Tenant>>();
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("localhost:8003");
            var result = await repo.ResolveAsync(context);
            Assert.Null(result.Tenant);
        }
    }
}
