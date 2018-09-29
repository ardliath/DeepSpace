using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeepSpace.Contracts;
using DeepSpace.Core;
using DeepSpace.Messages;
using Microsoft.AspNetCore.Mvc;

namespace DeepSpace.Controllers
{
    [Route("api/[controller]/[Action]")]
    public class ShipController : Controller
    {
        // GET api/ship/details
        [HttpGet]
        [ActionName("Details")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/ship/details/5
        [HttpGet("{id}")]
        [ActionName("Details")]
        public string Get(int id)
        {
            return $"ID was {id}";
        }

        // POST api/ship/create
        [HttpPost]        
        [ActionName("Create")]
        public CreateShipResponse Post([FromBody] CreateShipRequest value)
        {
            var ship = new ShipManager().CreateShip(value.Name);
            var response = new CreateShipResponse
            {
                Name = ship.Name,
                CommandCode = ship.CommandCode,
                TransponderCode = ship.TransponderCode
            };

            return response;
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
