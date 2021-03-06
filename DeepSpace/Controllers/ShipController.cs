﻿using System;
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
            //await this.ShipManager.MoveAsync("3fdf7642-bb23-452f-b403-ffb016617bd2", 1, 1, 1);
            return new string[] { "value1", "value2" };
        }

        // GET api/ship/details/5
        [HttpGet("{id}")]
        [ActionName("Details")]
        public async Task<string> Get(string id)
        {
            var ship = await this.ShipManager.GetShipAsync(id);
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
            var ship = await this.ShipManager.CreateShipAsync(value.Name);
            var response = new CreateShipResponse
            {
                Name = ship.Name,
                CommandCode = ship.CommandCode,
                TransponderCode = ship.TransponderCode,
                Health = ship.Statistics.BaseHealth,
                Location = new LocationRequestOrResponse
                {
                    X = ship.Location.X,
                    Y = ship.Location.Y,
                    Z = ship.Location.Z
                }
            };

            return response;
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

        [HttpPost]
        [ActionName("Scan")]
        public async Task<ScanResponse> Scan([FromBody] ScanRequest value)
        {
            var shipsInRange = await this.ShipManager.ScanAsync(value.CommandCode);
            var response = new ScanResponse
            {
                Ships = shipsInRange.Select(s =>
                new ShipSummary
                {
                    Name = s.Name,
                    TransponderCode = s.TransponderCode,
                    Location = s.Location == null
                        ? null
                        : new LocationRequestOrResponse
                        {
                            X = s.Location.X,
                            Y = s.Location.Y,
                            Z = s.Location.Z
                        }
                })
            };

            return response;
        }

        [HttpPost]
        [ActionName("Attack")]
        public async Task<string> Attack([FromBody] AttackShipRequest value)
        {
            var shipsInRange = await this.ShipManager.ScanAsync(value.CommandCode);
            var defendingShip = await this.ShipManager.GetShipAsync(value.TransponderCode);            
            if (!shipsInRange.Contains(defendingShip)) return "Attack failed. Ship out of range.";
            await this.ShipManager.AttackShipAsync(value.CommandCode, value.TransponderCode);
            return "Attack Complete";

        }        

        [HttpPost]
        [ActionName("Repair/hull")]
        public async Task<string>  Repair([FromBody] RepairShipRequest value)
        {
            await ShipManager.RepairAsync(value.CommandCode);
            return "Ship repaired";
        }

        [HttpPost]
        [ActionName("Repair/shield")]
        public async Task<string> Repair([FromBody] RestoreShipRequest value)
        {
            await ShipManager.RestoreAsync(value.CommandCode);
            return "Ship restored";
        }
    }
}
