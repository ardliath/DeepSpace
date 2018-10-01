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

        public Task<Ship> GetShipAsync(string commandCode)
        {
            throw new NotImplementedException();
        }

        public async Task<Move> MoveAsync(string commandCode, decimal x, decimal y, decimal z)
        {
            var ship = await this.GetShipAsync(commandCode);
            var time = new TimeSpan(0, 1, 0);
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
    }
}
