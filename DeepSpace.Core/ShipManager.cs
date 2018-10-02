using DeepSpace.Contracts;
using DeepSpace.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeepSpace.Core
{
    /// <summary>
    /// All the business logic for creating/managing ships
    /// </summary>
    public class ShipManager : IShipManager
    {
       public ShipManager(IShipDataAccess shipDataAccess)
        {
            this.ShipDataAccess = shipDataAccess;
        }

        public IShipDataAccess ShipDataAccess { get; }

        public async Task<Ship> CreateShipAsync(string name)
        {
            var ship = new Ship
            {
                Name = name,
                CommandCode = Guid.NewGuid().ToString(),
                TransponderCode = Guid.NewGuid().ToString(),
                Location = new Location // this would be better at semi-random coordinates
                {
                    X = 0,
                    Y = 0,
                    Z = 0
                },
                Statistics = new Statistics
                {
                    Speed = 1,
                    ScanRange = 1
                }
            };

            await this.ShipDataAccess.InsertShipAsync(ship);
            return ship;
        }

        public async Task<Ship> GetShipAsync(string commandCode)
        {
            var ship = this.ShipDataAccess.GetShip(commandCode);
            if (this.UpdateMovements(ship))
            {
                await this.ShipDataAccess.UpsertShipAsync(ship);
            }
            return await Task.FromResult(ship);
        }

        public async Task<Move> MoveAsync(string commandCode, decimal x, decimal y, decimal z)
        {
            var ship = await this.GetShipAsync(commandCode);
            if(this.UpdateMovements(ship))
            {
                await this.ShipDataAccess.UpsertShipAsync(ship);
            }

            var time = new TimeSpan(0, 1, 0); // not all journeys should take a minute! They should be calculated based on the ship's speed
            var destination = new Location { X = x, Y = y, Z = z };

            var now = DateTime.UtcNow;
            
            var move = new Move
            {
                StartTime = now,
                ArrivalTime = now.Add(time),
                Duration = time,
                From = ship.Location,
                To = destination
            };
            ship.Location = null;
            ship.Move = move;
            await this.ShipDataAccess.UpsertShipAsync(ship);

            return move;
        }

        public IEnumerable<Ship> Scan(string commandCode)
        {
            var ship = this.ShipDataAccess.GetShip(commandCode);
            var nearbyShips = this.ShipDataAccess.ScanForShips(ship.CommandCode, ship.Location, ship.Statistics.ScanRange);
            return nearbyShips;
        }

        private bool UpdateMovements(Ship ship)
        {
            var now = DateTime.UtcNow;
            if(ship.Move != null && ship.Move.ArrivalTime < now)
            {
                ship.Location = ship.Move.To;
                ship.Move = null;

                return true;
            }
            return false;
        }
    }
}
