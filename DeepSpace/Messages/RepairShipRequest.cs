using Newtonsoft.Json;

namespace DeepSpace.Messages
{
    public class RepairShipRequest
    {
        [JsonProperty(PropertyName = "commandCode")]
        public string CommandCode { get; set; }

        [JsonProperty(PropertyName = "health")]
        public double Health { get; set; }
    }
}