using System.Linq;
using System.Threading.Tasks;
using i21Apis.Data;
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
        private readonly DbContext db;

        public CustomerController(Tenant tenant, DbContext context)
        {
            this.tenant = tenant;
            this.db = context;
        }

        public async Task<IActionResult> Get()
        {
            var customers = await db.Set<tblARCustomer>().ToListAsync();
            return await Task.FromResult(Ok(customers));
        }
    }
}