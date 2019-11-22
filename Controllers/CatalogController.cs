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
        private readonly ICatalogStore<ITenant> catalog;

        public CatalogController(ICatalogStore<ITenant> catalog)
        {
            this.catalog = catalog;
        }

        [HttpGet("{format?}")]
        public async Task<IActionResult> Get()
        {
            var data = await catalog.GetTenantsAsync();
            return Ok(data);
        }
    }
}