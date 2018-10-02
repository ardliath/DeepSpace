using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeepSpace.Messages
{
    public class ScanRequest
    {
        [JsonProperty(PropertyName = "commandCode")]
        public string CommandCode { get; set; }
    }
}
