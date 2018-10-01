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
        Ship GetShip(string commandCode);

        void Repair(string commandCode, double health);
        void Restore(string commandCode);
        void ReceiveDamage(string commandCode, double health);
    }
}
