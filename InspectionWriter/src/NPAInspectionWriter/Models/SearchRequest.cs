using System;
using Newtonsoft.Json;

namespace NPAInspectionWriter.Models
{
    [JsonObject]
    public class SearchRequest
    {
        public Guid? VehicleId { get; set; }

        public string Vin { get; set; }

        public string StockNumber { get; set; }
    }
}
