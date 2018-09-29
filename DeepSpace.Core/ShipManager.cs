using DeepSpace.Contracts;
using DeepSpace.Data;
using System;
using System.Threading.Tasks;

namespace DeepSpace.Core
{
    public class ShipManager : IShipManager
    {
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

            await new ShipDataAccess().InsertShipAsync(ship);
            return ship;
        }
    }
}
