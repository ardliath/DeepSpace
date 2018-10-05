using Newtonsoft.Json;
using DeepSpace.Core;
using System;
using System.Collections.Generic;
using System.Linq;
namespace DeepSpace.Contracts
{
    public class Ship
    {
        [JsonProperty(PropertyName = "type")]
        public const string Type = "Ship";

        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        public string Name { get; set; }
        public string CommandCode { get; set; }
        public string TransponderCode { get; set; }
        
        public double Shield => ShieldUpgrades.Sum(su => su.ShieldValue);
        public List<IShieldUpgrades> ShieldUpgrades { get; set; }        

        public Location Location { get; set; }
        public Move Move { get; set; }

        public Statistics Statistics { get; set; }
    }    
}
