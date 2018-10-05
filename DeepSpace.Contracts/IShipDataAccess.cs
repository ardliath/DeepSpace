using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeepSpace.Contracts
{
    public interface IShipDataAccess
    {
        Task<Ship> InsertShipAsync(Ship ship);
        Task<Ship> UpsertShipAsync(Ship ship);
        Ship GetShip(string commandCode);
        Ship GetShipByTransponderCode(string transponderCode);
        IEnumerable<Ship> ScanForShips(string commandCode, Location location, int scanRange);
    }
}
