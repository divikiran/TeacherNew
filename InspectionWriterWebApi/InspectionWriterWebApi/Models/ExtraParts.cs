using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace InspectionWriterWebApi.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ExtraParts
    {
        [JsonProperty(PropertyName = "vehicleId", Required = Required.Always)]
        public string VehicleId { get; set; }

        [JsonProperty(PropertyName = "hasExtraParts", Required = Required.Always)]
        public string HasExtraParts { get; set; }

        [JsonProperty(PropertyName = "extraPartsList")]
        public string ExtraPartsList { get; set; }
    }
}