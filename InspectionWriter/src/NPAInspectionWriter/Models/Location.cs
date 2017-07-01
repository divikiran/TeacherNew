using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace NPAInspectionWriter.Models
{
    [JsonObject]
    public class Location
    {
        public Guid LocationId { get; set; }

        [JsonProperty(PropertyName = "Location")]
        public string Name { get; set; }
    }
}
