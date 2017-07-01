using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace InspectionWriterWebApi.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class InspectionMaster
    {
        [JsonProperty(PropertyName = "masterId")]
        public Guid MasterId { get; set; }

        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "defaultForCategory", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool DefaultForCategory { get; set; }

        [JsonIgnore]
        public Guid VehicleCategoryId { get; set; }

        [JsonProperty(PropertyName = "maxInspectionScore", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int MaxInspectionScore { get; set; }

        [JsonProperty(PropertyName = "hidden", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Hidden { get; set; }

        [JsonProperty(PropertyName = "requiredCategoryIds", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<Guid> RequiredCategoryIds { get; set; }

        [JsonProperty(PropertyName = "categories", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<InspectionCategory> Categories { get; set; }
    }
}
