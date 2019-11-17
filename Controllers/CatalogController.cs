using System.Threading.Tasks;
using i21Apis.Models;
using i21Apis.Multitenancy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace i21Apis.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly DbContext context;

        public CatalogController(DbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Get()
        {
            var data = new string[] { "Hello World" };
            return await Task.FromResult(Ok(data));
        }
    }
}