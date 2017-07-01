using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NPAInspectionWriter.Models
{
    [JsonObject]
    public class InspectionCategory
    {
        [JsonProperty( "categoryId" )]
        public Guid CategoryId { get; set; }

        [JsonProperty( "displayName" )]
        public string DisplayName { get; set; }

        [JsonProperty( "weight" )]
        public int Weight { get; set; }

        [JsonProperty( "maxScore" )]
        public int MaxScore { get; set; }

        [JsonProperty( "required" )]
        public bool Required { get; set; }

        [JsonProperty( "options" )]
        public IList<InspectionOption> Options { get; set; }

        [JsonProperty( "previousValue" )]
        public InspectionOption PreviousValue { get; set; }

        [JsonProperty( "currentValue" )]
        public InspectionOption CurrentValue { get; set; }

        [JsonProperty( "comments" )]
        public string Comments { get; set; }
    }
}
