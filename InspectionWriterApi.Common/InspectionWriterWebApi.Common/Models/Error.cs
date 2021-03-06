﻿using Newtonsoft.Json;

namespace InspectionWriterWebApi.Models
{
    [JsonObject]
    public class Error
    {
        public string Description { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }
    }
}
