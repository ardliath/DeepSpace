using DeepSpace.Contracts;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Configuration;

namespace DeepSpace.Data
{
    public class ShipDataAccess : IShipDataAccess
    {
        public async void InsertShipAsync(Ship ship)
        {            
            
            using (var client = CreateDocumentClient())
            {
                Document shipResponse = await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("DeepSpace", "Entities"), ship);
            }
        }

        private DocumentClient CreateDocumentClient()
        {
            var url = ConfigurationManager.AppSettings["DatabaseEndpoint"];
            var key = ConfigurationManager.AppSettings["DatabaseKey"];
            return new DocumentClient(new Uri(url), key);
        }
    }
}
