using System.Threading.Tasks;
using i21Apis.Models;
using i21Apis.Multitenancy;
using Microsoft.AspNetCore.Mvc;

namespace i21Apis.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly Tenant tenant;

        public TenantController(Tenant tenant)
        {
            this.tenant = tenant;
        }

        public async Task<IActionResult> Get()
        {
            var data = new string[] { "Hello World", tenant?.Name };
            return await Task.FromResult(Ok(data));
        }
    }
}