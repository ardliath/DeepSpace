using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeepSpace.Messages
{
    public class MoveShipRequest
    {
        [JsonProperty(PropertyName = "commandCode")]
        public string CommandCode { get; set; }

        [JsonProperty(PropertyName = "destination")]
        public LocationRequestOrResponse Destination { get; set; }
    }
}
