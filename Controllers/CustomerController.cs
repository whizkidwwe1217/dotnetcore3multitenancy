using System.Linq;
using System.Threading.Tasks;
using i21Apis.Models;
using i21Apis.Multitenancy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace i21Apis.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly Tenant tenant;
        private readonly DbContext dbContext;

        public CustomerController(Tenant tenant, DbContext dbContext)
        {
            this.tenant = tenant;
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Get()
        {
            var customers = await dbContext.Set<tblARCustomer>().ToListAsync();
            return await Task.FromResult(Ok(customers));
        }
    }
}