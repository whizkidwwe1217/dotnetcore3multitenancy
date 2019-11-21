using System.Threading.Tasks;
using HordeFlow.Data;
using HordeFlow.Models;
using HordeFlow.Multitenancy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HordeFlow.Controllers
{
    [ApiController]
    [Route("api/v1/admin/[controller]")]
    [FormatFilter]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogDbContext catalog;

        public CatalogController(CatalogDbContext catalog)
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