using DeepSpace.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DeepSpace.Data
{
    /// <summary>
    /// A local file based ship repository for people who don't have access to azureS
    /// </summary>
    public class FileBasedDataAccess : IShipDataAccess
    {
        public FileBasedDataAccess(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public Ship GetShip(string commandCode)
        {
            throw new NotImplementedException();
        }

        public Ship GetShipByTransponderCode(string transponderCode)
        {
            throw new NotImplementedException();
        }

        public Task<Ship> InsertShipAsync(Ship ship)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Ship> ScanForShips(string commandCode, Location location, int scanRange)
        {
            throw new NotImplementedException();
        }

        public Task<Ship> UpsertShipAsync(Ship ship)
        {
            throw new NotImplementedException();
        }

        private DirectoryInfo GetFolder()
        {
            var folder = this.Configuration.GetSection("StorageFolder").Value;
            return new DirectoryInfo(folder);
        }
    }
}
