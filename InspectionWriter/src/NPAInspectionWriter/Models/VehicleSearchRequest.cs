using System;
using Newtonsoft.Json;
using NPA.CodeGen;

namespace NPAInspectionWriter.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VehicleSearchRequest
    {
        [JsonProperty(PropertyName = "vehicleIds", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] VehicleIds { get; set; }

        [JsonProperty(PropertyName = "vehicleId", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? VehicleId { get; set; }

        [JsonProperty(PropertyName = "vin", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Vin { get; set; }

        [JsonProperty(PropertyName = "stockNumber", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string StockNumber { get; set; }

        [JsonProperty(PropertyName = "locationId", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? LocationId { get; set; }

        [JsonProperty(PropertyName = "brandId", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? BrandId { get; set; }

        [JsonProperty(PropertyName = "model", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Model { get; set; }

        [JsonProperty(PropertyName = "fromYear", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? FromYear { get; set; }

        [JsonProperty(PropertyName = "toYear", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ToYear { get; set; }

        //[JsonProperty(PropertyName = "getAllStockNos", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        //public bool? IncludePriorStockNumbers { get; set; }

        [JsonProperty(PropertyName = "userLevel", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int RequestingUserLevel { get; set; }

        public UserLevel UserLevel
        {
            get
            {
                return UserLevel.FromUserLevelCode(RequestingUserLevel);
            }
        }
    }
}
