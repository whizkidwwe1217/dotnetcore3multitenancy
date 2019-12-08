using System.Net.Http;
using Xunit;

namespace HordeFlow.IntegrationTests
{
    public abstract class BaseClassFixture : IClassFixture<WebApiFactory<TestStartup>>
    {
        protected readonly WebApiFactory<TestStartup> factory;
        protected HttpClient Client { get; set; }

        public BaseClassFixture(WebApiFactory<TestStartup> factory)
        {
            this.factory = factory;
            Client = this.factory.CreateClient();
        }
    }
}