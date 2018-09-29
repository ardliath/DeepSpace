using Newtonsoft.Json;

namespace DeepSpace.Messages
{
    public class LocationRequestOrResponse
    {
        [JsonProperty(PropertyName = "x")]
        public decimal X { get; set; }

        [JsonProperty(PropertyName = "y")]
        public decimal Y { get; set; }

        [JsonProperty(PropertyName = "z")]
        public decimal Z { get; set; }
    }
}