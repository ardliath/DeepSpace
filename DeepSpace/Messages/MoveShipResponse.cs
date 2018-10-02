using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeepSpace.Messages
{
    public class MoveShipResponse
    {
        [JsonProperty(PropertyName = "arrivalTime")]
        public DateTime ArrivalTime { get; set; }
    }
}
