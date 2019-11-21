using System.Threading.Tasks;
using HordeFlow.Data;
using HordeFlow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HordeFlow.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CompanyLocationController : ControllerBase
    {
        private readonly Tenant tenant;
        private readonly DbContext dbContext;

        public CompanyLocationController(Tenant tenant, DbContext dbContext)
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