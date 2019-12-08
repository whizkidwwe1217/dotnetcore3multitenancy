using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

        public async Task<HttpResponseMessage> PostAsJsonAsync<TModel>(string requestUrl, TModel model)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            return await Client.PostAsync(requestUrl, stringContent);
        }
    }
}