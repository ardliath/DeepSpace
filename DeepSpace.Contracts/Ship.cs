using Newtonsoft.Json;

namespace DeepSpace.Contracts
{
    public class Ship
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        public string Name { get; set; }
        public string CommandCode { get; set; }
        public string TransponderCode { get; set; }

        public Location Location { get; set; }
        public Move Move { get; set; }
    }    
}
