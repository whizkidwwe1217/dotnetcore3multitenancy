using System.Threading.Tasks;
using i21Apis.Data;
using i21Apis.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace i21Apis.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [FormatFilter]
    public class TenantController : ControllerBase
    {
        private readonly ICustomerRepository repository;

        public TenantController(ICustomerRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("{format?}")]
        public async Task<IActionResult> Get()
        {
            var data = await repository.ListAsync();
            return Ok(data);
        }
    }
}