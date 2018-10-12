using DeepSpace.Contracts;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

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
            return GetShipFromDisc(commandCode);
        }

        public Ship GetShipByTransponderCode(string transponderCode)
        {
            return this.ListAllShips().SingleOrDefault(s => s.TransponderCode.Equals(transponderCode));
        }

        public Task<Ship> InsertShipAsync(Ship ship)
        {
            return UpsertShipAsync(ship);
        }

        public IEnumerable<Ship> ScanForShips(string commandCode, Location location, int scanRange)
        {
            // this is a clone of the real method in cosmos data access, less than ideal!

            decimal minX = location.X - scanRange;
            decimal maxX = location.X + scanRange;
            decimal minY = location.Y - scanRange;
            decimal maxY = location.Y + scanRange;
            decimal minZ = location.Z - scanRange;
            decimal maxZ = location.Z + scanRange;

            return this.ListAllShips()
                .Where(s => s.CommandCode != commandCode // don't scan yourself!
                    && s.Location != null // and they're not moving
                    && s.Location.X >= minX
                    && s.Location.X <= maxX
                    && s.Location.Y >= minY
                    && s.Location.Y <= maxY
                    && s.Location.Z >= minZ
                    && s.Location.Z <= maxZ)
                .AsEnumerable<Ship>()
                .ToList();
        }

        public async Task<Ship> UpsertShipAsync(Ship ship)
        {
            if(ship.ID == null) ship.ID = Guid.NewGuid().ToString();

            var data = JsonConvert.SerializeObject(ship);
            var path = GetShipFilename(ship);
            File.WriteAllText(path, data);
            return ship;
        }

        private string GetShipFilename(Ship ship)
        {
            return this.GetShipFilename(ship.CommandCode);
        }

        private string GetShipFilename(string commandCode)
        {
            var filename = $"{commandCode}.json";
            return Path.Combine(GetFolder().FullName, filename);
        }

        private DirectoryInfo GetFolder()
        {
            var folder = this.Configuration.GetSection("StorageFolder")?.Value;
            if(folder == null)
            {
                throw new InvalidOperationException("To use the FileBasedDataAccess class you must have an AppSetting 'StorageFolder' configured with the folder path to use");
            }
            return new DirectoryInfo(folder);
        }

        private IEnumerable<Ship> ListAllShips()
        {            
            var files = this.GetFolder().GetFiles("*.json");
            return files.Select(f =>
            {
                var data = File.ReadAllText(f.FullName);
                return DeserialiseShip(data);
            });
        }

        private Ship GetShipFromDisc(string commandCode)
        {
            var path = this.GetShipFilename(commandCode);
            var data = File.ReadAllText(path);
            return DeserialiseShip(data);
        }

        private Ship DeserialiseShip(string data)
        {
            return JsonConvert.DeserializeObject<Ship>(data);
        }
    }
}
