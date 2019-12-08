using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HordeFlow.IntegrationTests
{
    public class TestMultitenancy : BaseClassFixture, IAsyncLifetime
    {
        public TestMultitenancy(WebApiFactory<TestStartup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_Migrate()
        {
            var result = await this.Client.PostAsync("https://localhost:8000/api/v1/admin/catalog/migrate", new StringContent(""));
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            Assert.Contains("Healthy", content);
        }

        public Task InitializeAsync() => PostAsJsonAsync("https://localhost:8000/api/v1/admin/catalog/migrate", new StringContent(""));
        public Task DisposeAsync() => Client.PostAsync("https://localhost:8000/api/v1/admin/catalog/drop", new StringContent(""));

    }
}
