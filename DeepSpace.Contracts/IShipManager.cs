using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeepSpace.Contracts
{
    public interface IShipManager
    {
        Task<Ship> CreateShipAsync(string name);
        Task<Move> MoveAsync(string commandCode, decimal x, decimal y, decimal z);
        Task<Ship> GetShipAsync(string commandCode);
        Task<IEnumerable<Ship>> ScanAsync(string commandCode);
        Task AddShieldUpgradeAsync(string commandCode, IShieldUpgrades upgrade);
        Task RepairAsync(string commandCode);
        Task AttackShipAsync(string commandCode, string transponderCode);
        Task RestoreAsync(string commandCode);
    }
}
