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
            var url = ConfigurationManager.AppSettings["DatabaseEndpoint"];
            var key = ConfigurationManager.AppSettings["DatabaseKey"];
            using (var client = new DocumentClient(new Uri(url), key))
            {
                Document shipResponse = await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("DeepSpace", "Entities"), ship);
            }
        }
    }
}
