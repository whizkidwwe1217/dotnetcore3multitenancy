using System.Threading;
using System.Threading.Tasks;
using HordeFlow.Data;
using HordeFlow.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HordeFlow.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly IDbMigrator migrator;

        public TenantController(IDbMigrator repository)
        {
            this.migrator = repository;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Migrate(CancellationToken cancellationToken = default(CancellationToken))
        {
            await migrator.MigrateAsync(cancellationToken);
            return Ok(true);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Drop(CancellationToken cancellationToken = default(CancellationToken))
        {
            await migrator.DropAsync(cancellationToken);
            return Ok(true);
        }
    }
}