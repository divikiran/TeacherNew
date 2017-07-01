using System;
using Newtonsoft.Json;

namespace InspectionWriterWebApi.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Message
    {
        [Obsolete( "Usage of the Message Type is not recommended at this type. It is currently planed for removal", error: true )]
        [JsonProperty(PropertyName = "messageType", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string MessageType { get; set; }

        [JsonProperty(PropertyName = "to", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string To { get; set; }

        [JsonProperty(PropertyName = "from", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string From { get; set; }

        [JsonProperty(PropertyName = "subject", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Subject { get; set; }

        [JsonProperty(PropertyName = "body", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Body { get; set; }
    }
}
