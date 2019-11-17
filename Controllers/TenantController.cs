using System.Threading.Tasks;
using i21Apis.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace i21Apis.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [FormatFilter]
    public class TenantController : ControllerBase
    {
        private readonly CatalogDbContext catalog;

        public TenantController(CatalogDbContext catalog)
        {
            this.catalog = catalog;
        }

        [HttpGet("{format?}")]
        public async Task<IActionResult> Get()
        {
            var data = await catalog.Tenant.ToListAsync();
            return Ok(data);
        }
    }
}