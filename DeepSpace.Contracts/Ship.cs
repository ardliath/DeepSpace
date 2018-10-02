using Newtonsoft.Json;

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

        public Location Location { get; set; }
        public Move Move { get; set; }

        public Statistics Statistics { get; set; }
    }    
}
