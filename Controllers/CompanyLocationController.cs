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
    public class CompanyLocationController : ControllerBase
    {
        private readonly Tenant tenant;
        private readonly TenantDbContext dbContext;

        public CompanyLocationController(Tenant tenant, TenantDbContext dbContext)
        {
            this.tenant = tenant;
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Get()
        {
            var companyLocations = await dbContext.Set<tblSMCompanyLocation>().ToListAsync();
            return await Task.FromResult(Ok(companyLocations));
        }
    }
}