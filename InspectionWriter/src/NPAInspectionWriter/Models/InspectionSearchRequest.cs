using System;
using Newtonsoft.Json;

namespace NPAInspectionWriter.Models
{
    [JsonObject]
    public class InspectionSearchRequest : SearchRequest
    {
        public Guid? InspectionId { get; set; }

        public Guid UserId { get; set; }
    }
}
