using System.Threading;
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
        private readonly ICatalogStore<Tenant> catalog;
        private readonly IDbMigrator migrator;

        public CatalogController(ICatalogStore<Tenant> catalog, IDbMigrator migrator)
        {
            this.catalog = catalog;
            this.migrator = migrator;
        }

        [HttpGet("{format?}")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default(CancellationToken))
        {
            var data = await catalog.GetTenantsAsync(predicate: null, cancellationToken);
            return Ok(data);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Migrate(CancellationToken cancellationToken = default(CancellationToken))
        {
            await migrator.MigrateAsync(cancellationToken);
            return Ok(true);
        }
    }
}