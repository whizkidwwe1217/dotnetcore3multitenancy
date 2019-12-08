using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace HordeFlow.IntegrationTests
{
    public abstract class BaseIntegrationTest<TStartup> : IClassFixture<TestServerFixture<TStartup>> where TStartup : class
    {
        public readonly HttpClient client;
        public readonly TestServer server;

        public BaseIntegrationTest(TestServerFixture<TStartup> testServerFixture)
        {
            client = testServerFixture.Client;
            server = testServerFixture.Server;
        }
    }
}