using System.Threading.Tasks;
using i21Apis.Models;
using i21Apis.Multitenancy;
using Microsoft.AspNetCore.Mvc;

namespace i21Apis.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly TenantDbContext context;

        public CatalogController(TenantDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Get()
        {
            var data = new string[] { "Hello World" };
            return await Task.FromResult(Ok(data));
        }
    }
}