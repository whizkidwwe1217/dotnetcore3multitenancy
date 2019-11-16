using System.Linq;
using System.Threading.Tasks;
using i21Apis.Models;
using i21Apis.Multitenancy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace i21Apis.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly CatalogDbContext catalog;

        public TenantController(CatalogDbContext catalog)
        {
            this.catalog = catalog;
        }

        public async Task<IActionResult> Get()
        {
            var data = await catalog.Tenant.ToListAsync();
            return Ok(data);
        }
    }
}