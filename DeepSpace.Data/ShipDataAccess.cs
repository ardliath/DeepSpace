using DeepSpace.Contracts;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace DeepSpace.Data
{
    public class ShipDataAccess : IShipDataAccess
    {
        public ShipDataAccess(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public Ship GetShip(string commandCode)
        {
            using (var client = CreateDocumentClient())
            {
                return client.CreateDocumentQuery<Ship>(CreateCollectionLink(), new FeedOptions { MaxItemCount = 1 })
                    .Where(s => s.CommandCode == commandCode)
                    .AsEnumerable<Ship>()
                    .SingleOrDefault();
            }
        }

        public Ship GetShipByTransponderCode(string transponderCode)
        {
            using (var client = CreateDocumentClient())
            {
                return client.CreateDocumentQuery<Ship>(CreateCollectionLink(), new FeedOptions { MaxItemCount = 1 })
                    .Where(s => s.TransponderCode == transponderCode)
                    .AsEnumerable<Ship>()
                    .SingleOrDefault();
            }
        }

        public async Task<Ship> InsertShipAsync(Ship ship)
        {
            using (var client = CreateDocumentClient())
            {
                Document shipResponse = await client.CreateDocumentAsync(CreateCollectionLink(), ship);
            }
            return await Task.FromResult(ship);
        }

        private static Uri CreateCollectionLink()
        {
            return UriFactory.CreateDocumentCollectionUri("DeepSpace", "Entities");
        }

        private DocumentClient CreateDocumentClient()
        {
            var url = this.Configuration.GetSection("DatabaseEndpoint").Value;
            var key = this.Configuration.GetSection("DatabaseKey").Value;
            return new DocumentClient(new Uri(url), key);
        }

        public async Task<Ship> UpsertShipAsync(Ship ship)
        {            
            using (var client = CreateDocumentClient())
            {                
                await client.UpsertDocumentAsync(CreateCollectionLink(), ship);
                return await Task.FromResult(ship);
            }            
        }

        public IEnumerable<Ship> ScanForShips(string commandCode, Location location, int scanRange)
        {
            using (var client = CreateDocumentClient())
            {
                decimal minX = location.X - scanRange;
                decimal maxX = location.X + scanRange;
                decimal minY = location.Y - scanRange;
                decimal maxY = location.Y + scanRange;
                decimal minZ = location.Z - scanRange;
                decimal maxZ = location.Z + scanRange;


                return client.CreateDocumentQuery<Ship>(CreateCollectionLink())
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
        }
    }
}
