using Newtonsoft.Json;

namespace DeepSpace.Controllers
{
    public class RestoreShipRequest
    {
        [JsonProperty(PropertyName = "commandCode")]
        public string CommandCode { get; set; }
    }
}