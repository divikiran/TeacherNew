using Newtonsoft.Json;

namespace NPAInspectionWriter.Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class DeviceRegistration
    {
        public string DeviceToken { get; set; }

        public string UserAccountId { get; set; }
    }
}