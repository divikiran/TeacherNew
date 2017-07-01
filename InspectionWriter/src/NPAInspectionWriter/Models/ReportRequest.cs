using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NPAInspectionWriter.Models
{
    [JsonObject( MemberSerialization.OptIn )]
    public class ReportRequest
    {
        [JsonProperty( PropertyName = "printerId", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore )]
        public Guid? PrinterId { get; set; }

        [JsonProperty( PropertyName = "vehicleId", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore )]
        public Guid VehicleId { get; set; }

        [JsonProperty( PropertyName = "locationId", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore )]
        public Guid LocationId { get; set; }

        [JsonProperty( PropertyName = "reportFile", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore )]
        [JsonConverter( typeof( StringEnumConverter ) )]
        public ReportFile ReportFile { get; set; }

    }
}
