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
    }
}
