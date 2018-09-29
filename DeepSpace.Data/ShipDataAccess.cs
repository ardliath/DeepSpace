using DeepSpace.Contracts;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace DeepSpace.Data
{
    public class ShipDataAccess : IShipDataAccess
    {
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
            var url = ConfigurationManager.AppSettings["DatabaseEndpoint"];
            var key = ConfigurationManager.AppSettings["DatabaseKey"];
            return new DocumentClient(new Uri(url), key);
        }
    }
}
