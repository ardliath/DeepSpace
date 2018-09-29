using DeepSpace.Contracts;
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
                TransponderCode = Guid.NewGuid().ToString()
            };

            return ship;
        }
    }
}
