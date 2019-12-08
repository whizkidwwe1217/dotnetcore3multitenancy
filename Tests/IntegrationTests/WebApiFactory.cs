using System;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Lamar.Microsoft.DependencyInjection;

namespace HordeFlow.IntegrationTests
{
    public class WebApiFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .UseLamar()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var path = Environment.CurrentDirectory.Replace(@"\bin\Debug\netcoreapp3.1", "");
                    webBuilder
                        .UseEnvironment("Testing")
                        .UseContentRoot(path)
                        .UseUrls(new string[]
                        {
                            "https://*:8000", "https://*:8001",
                            "https://*:8002", "https://*:8003"
                        })
                        .UseConfiguration(new ConfigurationBuilder()
                            .SetBasePath(path)
                            .AddJsonFile("appsettings.Testing.json")
                            .Build()
                        )
                        .UseStartup<TStartup>();
                });
        }
    }
}