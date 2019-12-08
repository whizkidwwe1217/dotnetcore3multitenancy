using System.Threading.Tasks;
using Xunit;

namespace HordeFlow.IntegrationTests
{
    public class TestHealthChecks : BaseClassFixture
    {
        public TestHealthChecks(WebApiFactory<TestStartup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_Be_Healthy()
        {
            var result = await this.Client.GetAsync("https://localhost:8000/health");
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            Assert.Contains("Healthy", content);
        }
    }
}
