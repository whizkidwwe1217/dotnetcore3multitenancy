using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace HordeFlow.IntegrationTests
{
    public class TestServerFixture<TStartup> : IDisposable where TStartup : class
    {
        public TestServer Server { get; }
        public HttpClient Client { get; }

        public TestServerFixture()
        {
            var path = Environment.CurrentDirectory.Replace(@"\bin\Debug\netcoreapp3.1", "");
            Server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseContentRoot(path)
                .UseUrls(new string[] { "https://*:5000", "https://*:5001", "https://*:5002", "https://*:5003" })
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(path)
                    .AddJsonFile("appsettings.Testing.json")
                    .Build()
                )
                .UseStartup<TStartup>());
            //Server.BaseAddress = new Uri("https://localhost:5000");
            Client = Server.CreateClient();
        }

        public void Dispose()
        {
            Server.Dispose();
            Client.Dispose();
        }
    }
}