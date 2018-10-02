using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeepSpace.Messages
{
    public class ScanResponse
    {

        [JsonProperty(PropertyName = "ships")]
        public IEnumerable<ShipSummary> Ships { get; set; }
    }
}
