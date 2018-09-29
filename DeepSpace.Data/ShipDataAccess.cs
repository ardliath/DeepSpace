using DeepSpace.Contracts;
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
                await client.CreateDocumentAsync(UriFactory.CreateDocumentUri("ahgcosmos", "Entities", ship.TransponderCode), ship);
            }
        }
    }
}
