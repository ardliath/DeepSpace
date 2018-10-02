using Newtonsoft.Json;

namespace DeepSpace.Messages
{
    public class ShipSummary
    {
        [JsonProperty(PropertyName = "transponderCode")]
        public string TransponderCode { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "location")]
        public LocationRequestOrResponse Location { get; set; }
    }
}