using System;
using Newtonsoft.Json;

namespace InspectionWriterWebApi.Models
{
    [JsonObject]
    public class InspectionSearchRequest : SearchRequest
    {
        public Guid? InspectionId { get; set; }

        public Guid UserId { get; set; }
    }
}
