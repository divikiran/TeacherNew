using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NPAInspectionWriter.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VehicleBrand
    {
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "displayName")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "models", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<VehicleModel> VehicleModels { get; set; }
    }
}