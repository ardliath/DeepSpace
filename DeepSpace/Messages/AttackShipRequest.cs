using Newtonsoft.Json;

namespace DeepSpace.Messages
{
    public class AttackShipRequest
    {
        [JsonProperty(PropertyName = "attackerCommandCode")]
        public string CommandCode { get; set; }

        [JsonProperty(PropertyName = "defenderTransponderCode")]
        public string TransponderCode { get; set; }
    }
}