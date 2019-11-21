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
        private readonly ITenantDbMigrator migrator;

        public TenantController(ITenantDbMigrator repository)
        {
            this.migrator = repository;
        }

        public async Task<IActionResult> Get(CancellationToken cancellationToken = default(CancellationToken))
        {
            await migrator.MigrateAsync(cancellationToken);
            return Ok(true);
        }
    }
}