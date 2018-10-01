using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeepSpace.Messages
{
    public class CreateShipResponse : CreateShipRequest
    {
        [JsonProperty(PropertyName = "commandCode")]
        public string CommandCode { get; set; }

        [JsonProperty(PropertyName = "transponderCode")]
        public string TransponderCode { get; set; }

        [JsonProperty(PropertyName = "location")]
        public LocationRequestOrResponse Location { get; set; }

        [JsonProperty(PropertyName = "health")]
        public double Health { get; set; }
    }
}
