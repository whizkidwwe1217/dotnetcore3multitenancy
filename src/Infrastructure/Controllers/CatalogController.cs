using System;
using System.Threading;
using System.Threading.Tasks;
using HordeFlow.Data;
using HordeFlow.Core;
using HordeFlow.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace HordeFlow.Infrastructure.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/admin/[controller]")]
    [FormatFilter]
    public class CatalogController : ControllerBase
    {
        private readonly IRepository<Tenant, Guid> repository;
        private readonly IDbMigrator migrator;

        public CatalogController(IRepository<Tenant, Guid> repository, IDbMigrator migrator)
        {
            this.repository = repository;
            this.migrator = migrator;
        }

        [HttpGet("{format?}")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default(CancellationToken))
        {
            //var data = await catalog.GetTenantsAsync(predicate: null, cancellationToken);
            var data = await repository.ListAsync(cancellationToken);
            return Ok(data);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Migrate(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                await migrator.MigrateAsync(cancellationToken);
                return Ok(true);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Drop(CancellationToken cancellationToken = default(CancellationToken))
        {
            await migrator.DropAsync(cancellationToken);
            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Tenant tenant, CancellationToken cancellationToken = default(CancellationToken))
        {
            await repository.AddAsync(tenant, cancellationToken);
            await repository.SaveAsync(cancellationToken);
            return Ok(tenant);
        }
    }
}