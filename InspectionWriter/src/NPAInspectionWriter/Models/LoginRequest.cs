using Newtonsoft.Json;

namespace NPAInspectionWriter.Models
{
    [JsonObject]
    public class LoginRequest
    {
        [JsonProperty(Required = Required.Default, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string AppVersion { get; set; }

		public LoginRequest() { }

		public LoginRequest(string userName, string password)
		{
			UserName = userName;
			Password = password;
		}

		public string UserName { get; set; }
		
        public string Password { get; set; }
    }
}
