using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Lamar.Microsoft.DependencyInjection;

namespace i21Apis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseLamar()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseUrls(new string[] { "https://*.hris.hordeflow.com", "https://*:8001", "https://*:8002", "https://*:8003" });
                });
    }
}
