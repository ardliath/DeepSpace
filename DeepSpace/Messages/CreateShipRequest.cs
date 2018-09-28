using Newtonsoft.Json;

namespace DeepSpace.Messages
{
    public class CreateShipRequest
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}