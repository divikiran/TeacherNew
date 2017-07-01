using Newtonsoft.Json;

namespace InspectionWriterWebApi.Models
{
    [JsonObject]
    public class Setting
    {
        public string SettingName { get; set; }

        public string Value { get; set; }
    }
}
