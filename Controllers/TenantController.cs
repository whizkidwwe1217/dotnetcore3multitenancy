using System.Threading;
using System.Threading.Tasks;
using i21Apis.Data;
using i21Apis.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace i21Apis.Controllers
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