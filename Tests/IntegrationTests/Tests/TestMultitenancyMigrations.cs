using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HordeFlow.Core;
using HordeFlow.Infrastructure.Repositories;
using Newtonsoft.Json;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests
{
    [Order(2)]
    public class TestMultitenancyMigrations : BaseClassFixture, IAsyncLifetime
    {
        public TestMultitenancyMigrations(WebApiFactory<TestStartup> factory) : base(factory)
        {
            
        }

        [Fact, Order(0)]
        public async Task Should_Create_Tenants()
        {
            var tenant1 = new Tenant
            {
                Name = "Tenant 1",
                HostName = "localhost:8001",
                DatabaseProvider = DatabaseProvider.Sqlite,
                ConnectionString = "Data Source=tenant1.test.db"
            };
            
            var tenant2 = new Tenant
            {
                Name = "Tenant 2",
                HostName = "localhost:8002",
                DatabaseProvider = DatabaseProvider.Sqlite,
                ConnectionString = "Data Source=tenant2.test.db"
            };

            var result = await PostAsJsonAsync($"{catalogHost}/admin/catalog", tenant1);
            result.EnsureSuccessStatusCode();

            result = await PostAsJsonAsync($"{catalogHost}/admin/catalog", tenant2);
            result.EnsureSuccessStatusCode();

            result = await Client.GetAsync($"{catalogHost}/admin/catalog");
            var content = await result.Content.ReadAsStringAsync();
            result.EnsureSuccessStatusCode();
            var json = JsonConvert.DeserializeObject<List<Tenant>>(content);
            Assert.Equal(2, json.Count);

            var found = json.Where(e => e.Name == "Tenant 1").ToList();
            Assert.NotEmpty(found);

            var host = "https://localhost:8001/api/v1";
            result = await Client.PostAsync($"{host}/tenant/migrate", new StringContent(""));
            content = await result.Content.ReadAsStringAsync();
            result.EnsureSuccessStatusCode();

            // var repo = factory.Services.GetService<ITenantRepository>();

            var repo = factory.Services.GetRequiredService<ITenantRepository>();
            Assert.NotNull(repo);
            var tenants = await repo.ListAsync();
            Assert.NotEmpty(tenants);
            Assert.Equal(2, tenants.Count);
        }

        // [Fact, Order(1)]
        // public async Task Should_Get_Tenant()
        // {
        //     var host = "https://localhost:8001/api/v1";
        //     var result = await Client.PostAsync($"{host}/tenant/migrate", new StringContent(""));
        //     var content = await result.Content.ReadAsStringAsync();
        //     result.EnsureSuccessStatusCode();
        // }
    }
}
