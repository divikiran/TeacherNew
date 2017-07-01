using System;
using Newtonsoft.Json;

namespace InspectionWriterWebApi.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Printer
    {
        [JsonProperty(PropertyName = "printerId", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid PrinterId { get; set; }

        [JsonProperty(PropertyName = "printerName", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string PrinterName { get; set; }

    }
}