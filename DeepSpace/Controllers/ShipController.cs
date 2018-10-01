using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeepSpace.Contracts;
using DeepSpace.Messages;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public string Get(string id)
        {
            var ship = this.ShipManager.GetShip(id);
            if (ship == null)
            {
                return "Ship not found";
            }
            else
            {
                return $"Ship {ship.Name} was found";
            }
        }

        // POST api/ship/create
        [HttpPost]        
        [ActionName("Create")]
        public async Task<CreateShipResponse> Create([FromBody] CreateShipRequest value)
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

        [HttpPost]
        [ActionName("Move")]
        public async Task<MoveShipResponse> Move([FromBody] MoveShipRequest value)
        {
            var move = await this.ShipManager.MoveAsync(value.CommandCode, value.Destination.X, value.Destination.Y, value.Destination.Z);
            var response = new MoveShipResponse
            {
                ArrivalTime = move.ArrivalTime
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
