using HordeFlow.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace HordeFlow.Hris
{
    public class Startup : BaseStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
