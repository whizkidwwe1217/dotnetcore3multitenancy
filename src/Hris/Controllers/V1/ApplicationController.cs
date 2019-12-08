using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace HordeFlow.Hris.Controllers
{
    [ApiVersion("2.0")]
    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        [HttpGet]
        public List<string> List()
        {
            return new List<string>() { "Hellow", "World" };
        }

        [HttpGet]
        [MapToApiVersion("1")]
        [Route("Potatoes")]
        public string GetPotatoes() => "Space Potatoes v1";

        [HttpGet]
        [MapToApiVersion("2")]
        [Route("Potatoes")]
        public string GetPotatoesV2() => "Space Potatoes v2";
    }
}