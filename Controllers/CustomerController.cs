using System.Linq;
using System.Threading.Tasks;
using HordeFlow.Data;
using HordeFlow.Models;
using HordeFlow.Multitenancy;
using HordeFlow.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HordeFlow.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository repository;

        public CustomerController(ICustomerRepository repository)
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