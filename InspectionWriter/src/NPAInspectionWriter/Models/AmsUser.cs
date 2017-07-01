using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NPA.CodeGen;
using Location = NPAInspectionWriter.Models.Location;

namespace NPAInspectionWriter.Models
{
    [JsonObject]
    public class AmsUser
    {
        [JsonRequired]
        [JsonProperty("userAccountId")]
        public Guid UserAccountId { get; set; }

        [JsonRequired]
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonIgnore]
        public UserLevel AmsUserLevel { get; set; }

        private int userLevel;
        [JsonProperty("userLevel")]
        public int UserLevel { get { return AmsUserLevel?.UserLevelCode ?? userLevel; } set { userLevel = value; } }

        [JsonIgnore]
        public Location LocationEx { get; set; }

        private string location;
        [JsonProperty("location")]
        public string Location
        {
            get { return LocationEx?.Name ?? location; }
            set { location = value; }
        }

        private Guid locationId;
        [JsonProperty("locationId")]
        public Guid LocationId
        {
            get { return LocationEx?.LocationId ?? locationId; }
            set { locationId = value; }
        }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "currentAppVersion", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore, Required = Required.Always)]
        public string CurrentAppVersion { get; set; }

        [JsonProperty(PropertyName = "currentAppVersionRequiredDate", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore, Required = Required.Always)]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CurrentAppVersionRequiredDate { get; set; }

        [JsonProperty(PropertyName = "canCreateCr")]
        public bool CanCreateInspections { get; set; }

        private string _authToken;
        [JsonProperty(PropertyName = "authToken", NullValueHandling = NullValueHandling.Ignore)]
        public string AuthToken
        {
            get { return _authToken; }
            set { _authToken = value; }
        }

        [JsonProperty(PropertyName = "sessionToken", NullValueHandling = NullValueHandling.Ignore)]
        public string SessionToken { get; set; }
        public string LastErrorMessage { get; set; }
        public bool IsLoggedIn { get; internal set; }
        public DateTime LoginExpires { get; internal set; }
    }
}
