using HordeFlow.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests
{
    public class TestStartup : BaseStartup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}