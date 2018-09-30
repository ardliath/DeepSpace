using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DeepSpace.Controllers
{
    [Route("api/[controller]")]
    public class InfoController : Controller
    {
        public InfoController(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // GET api/values
        [HttpGet]
        public string Get()
        {
            var endpoint = this.Configuration.GetValue<string>("DatabaseEndpoint");
            return $"The DeepSpace server is online, operational, and connected to {endpoint}."; // basic ping message
        }
    }
}