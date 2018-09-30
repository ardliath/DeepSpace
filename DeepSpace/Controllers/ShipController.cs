using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeepSpace.Contracts;
using DeepSpace.Messages;
using Microsoft.AspNetCore.Mvc;

namespace DeepSpace.Controllers
{
    [Route("api/[controller]/[Action]")]
    public class ShipController : Controller
    {
        public ShipController(IShipManager shipManager)
        {            
            this.ShipManager = shipManager;
        }
        
        public IShipManager ShipManager { get; }

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
        public async Task<CreateShipResponse> Post([FromBody] CreateShipRequest value)
        {
            try
            {
                var ship = await this.ShipManager.CreateShipAsync(value.Name);
                var response = new CreateShipResponse
                {
                    Name = ship.Name,
                    CommandCode = ship.CommandCode,
                    TransponderCode = ship.TransponderCode,
                    Location = new LocationRequestOrResponse
                    {
                        X = ship.Location.X,
                        Y = ship.Location.Y,
                        Z = ship.Location.Z
                    }
                };

                return response;
            }
            catch (Exception ex)
            {
                return new CreateShipResponse
                {
                    Name = $"{ex.Message} - {ex.StackTrace}"  // let's not even start talking about why this is a bad idea!
                };
            }
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
