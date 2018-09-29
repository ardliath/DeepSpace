using DeepSpace.Contracts;
using DeepSpace.Data;
using System;

namespace DeepSpace.Core
{
    public class ShipManager : IShipManager
    {
        public Ship CreateShip(string name)
        {
            var ship = new Ship
            {
                Name = name,
                CommandCode = Guid.NewGuid().ToString(),
                TransponderCode = Guid.NewGuid().ToString(),
                Location = new Location
                {
                    X = 0,
                    Y = 0,
                    Z = 0
                }
            };

            new ShipDataAccess().InsertShipAsync(ship);
            return ship;
        }
    }
}
