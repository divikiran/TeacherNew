using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NPAInspectionWriter.iOS.Models
{
    [JsonObject]
    public sealed class OSSCredit
    {
        [JsonProperty( "projectName" )]
        public string ProjectName { get; set; }

        [JsonProperty( "version" )]
        public string Version { get; set; }

        [JsonProperty( "projectUrl" )]
        public string ProjectUrl { get; set; }

        [JsonProperty( "licenseType" )]
        [JsonConverter( typeof( StringEnumConverter ) )]
        public OSSLicense LicenseType { get; set; }

        [JsonProperty( "licenseUrl" )]
        public string LicenseUrl { get; set; }
    }
}
