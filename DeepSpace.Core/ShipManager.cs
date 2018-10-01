using DeepSpace.Contracts;
using DeepSpace.Data;
using System;
using System.Threading.Tasks;

namespace DeepSpace.Core
{
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
                }
            };

            await this.ShipDataAccess.InsertShipAsync(ship);
            return ship;
        }

        public Ship GetShip(string commandCode)
        {
            return this.ShipDataAccess.GetShip(commandCode);
        }

        public async Task<Move> MoveAsync(string commandCode, decimal x, decimal y, decimal z)
        {
            var ship = this.GetShip(commandCode);
            var time = new TimeSpan(0, 1, 0); // not all journeys should take a minute!
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

            return move;
        }

        public void ReceiveDamage(string commandCode, double damage)
        {
            var ship =  ShipDataAccess.GetShip(commandCode);
            ship.UpdateHealth(-damage);
        }

        public void Repair(string commandCode, double health)
        {
            var ship = ShipDataAccess.GetShip(commandCode);
            ship.UpdateHealth(health);
        }

        public void Restore(string commandCode)
        {
            var ship = ShipDataAccess.GetShip(commandCode);
            ship.Restore();
        }
    }
}
