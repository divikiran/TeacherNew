using Newtonsoft.Json;

namespace InspectionWriterWebApi.Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class DeviceRegistration
    {
        public string DeviceToken { get; set; }

        public string UserAccountId { get; set; }
    }
}