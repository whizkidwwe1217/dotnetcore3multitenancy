using HordeFlow.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace UnitTests
{
    public class TestStartup : BaseStartup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}