using DeepSpace.Contracts;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
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

        public async Task<Ship> InsertShipAsync(Ship ship)
        {
            using (var client = CreateDocumentClient())
            {
                Document shipResponse = await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("DeepSpace", "Entities"), ship);
            }
            return await Task.FromResult(ship);
        }

        private DocumentClient CreateDocumentClient()
        {
            var url = this.Configuration.GetSection("DatabaseEndpoint").Value;
            var key = this.Configuration.GetSection("DatabaseKey").Value;
            return new DocumentClient(new Uri(url), key);
        }
    }
}
