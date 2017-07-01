using System;
using Newtonsoft.Json;

namespace InspectionWriterWebApi.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VehicleModel
    {
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "displayName")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "modelYear")]
        public int ModelYear { get; set; }

        public VehicleCategory DefaultVehicleCategory { get; set; }
    }
}
