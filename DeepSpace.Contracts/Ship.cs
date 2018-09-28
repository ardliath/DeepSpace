using Newtonsoft.Json;
using System;

namespace DeepSpace.Contracts
{
    public class Ship
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }    
}
