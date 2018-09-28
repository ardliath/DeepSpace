using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DeepSpace.Controllers
{
    [Route("api/[controller]")]
    public class InfoController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            return "The DeepSpace server is online and operational."; // basic ping message
        }
    }
}