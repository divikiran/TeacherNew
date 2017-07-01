using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InspectionWriterWebApi.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Inspection
    {
        [JsonProperty(PropertyName = "contextAppVersion", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ContextAppVersion { get; set; }

        [JsonProperty(PropertyName = "contextiOSVersion", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ContextiOSVersion { get; set; }

        [JsonProperty(PropertyName = "contextiPadModel", NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ContextiPadModel { get; set; }

        [JsonProperty(PropertyName = "contextUsername", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ContextUsername { get; set; }

        [JsonProperty(PropertyName = "contextLocation", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ContextLocation { get; set; }

        [JsonProperty(PropertyName = "inspectionId", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore, Required = Required.Always)]
        public Guid InspectionId { get; set; }

        [JsonProperty(PropertyName = "inspection", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string InspectionValue { get; set; }

        [JsonProperty(PropertyName = "vehicleId", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore, Required = Required.Always)]
        public Guid VehicleId { get; set; }

        [JsonProperty(PropertyName = "locationId", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? LocationId { get; set; }

        [JsonProperty(PropertyName = "masterId", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore, Required = Required.Always)]
        public Guid MasterId { get; set; }

        [JsonProperty(PropertyName = "master", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore, Required = Required.Default)]
        public string MasterDisplayName { get; set; }

        [JsonProperty(PropertyName = "inspectionType", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string InspectionType { get; set; }

        [JsonProperty(PropertyName = "inspectionTypeId", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore, Required = Required.Always)]
        public int InspectionTypeId { get; set; }

        [JsonProperty(PropertyName = "inspectionUser", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore, Required = Required.Always)]
        public string InspectionUser { get; set; }

        [JsonProperty(PropertyName = "inspectionUserId", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore, Required = Required.Always)]
        public Guid InspectionUserId { get; set; }

        [JsonProperty(PropertyName = "score", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore, Required = Required.Always)]
        public int? Score { get; set; }

        [JsonProperty(PropertyName = "comments", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Comments { get; set; }

        [JsonProperty(PropertyName = "inspectionDate", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore, Required = Required.Always)]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime InspectionDate { get; set; }

        [JsonProperty(PropertyName = "inspectionItems", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<InspectionItem> InspectionItems { get; set; }

        [JsonProperty(PropertyName = "inspectionImages", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<InspectionImage> InspectionImages { get; set; }

        [JsonProperty(PropertyName = "openRepair", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool OpenRepair { get; set; }

        [JsonProperty(PropertyName = "repairComments", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string RepairComments { get; set; }

        [JsonProperty(PropertyName = "newColor", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string NewColor { get; set; }

        [JsonProperty(PropertyName = "newMileage", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string NewMileage { get; set; }

        [JsonProperty(PropertyName = "newVIN", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string NewVIN { get; set; }

        [JsonProperty(PropertyName = "newVehicleModelId", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string NewVehicleModelId { get; set; }

        [JsonProperty(PropertyName = "elapsedTime", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ElapsedTime { get; set; }

        [JsonProperty(PropertyName = "allowEditing", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool AllowEditing { get; set; }

        [JsonProperty(PropertyName = "inspectionMilesHours", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string InspectionMilesHours { get; set; }
        
        [JsonProperty(PropertyName = "inspectionColor", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string InspectionColor { get; set; }
        
        internal string StockNumber { get; set; } // To support LINQ GroupBy()



    }
}