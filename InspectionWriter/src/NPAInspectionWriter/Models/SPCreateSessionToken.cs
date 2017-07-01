﻿using System;
using Newtonsoft.Json;

namespace NPAInspectionWriter.Models
{
    public class SPCreateSessionToken
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("entityLinkId")]
        public Guid EntityLinkID { get; set; }

        [JsonProperty("entityLinkTypeId")]
        public int EntityLinkTypeID { get; set; }
    }
}
