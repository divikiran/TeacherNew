using System;
using Newtonsoft.Json;

namespace InspectionWriterWebApi.Models
{
    [JsonObject]
    public class InspectionOption
    {
        [JsonProperty( "optionId" )]
        public Guid OptionId { get; set; }
        
        [JsonProperty( "displayName" )]
        public string DisplayName { get; set; }

        [JsonProperty( "value" )]
        public int Value { get; set; }
    }
}
