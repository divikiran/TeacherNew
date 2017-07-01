using Newtonsoft.Json;

namespace NPAInspectionWriter.Models
{
    [JsonObject]
    public class Setting
    {
        public string SettingName { get; set; }

        public string Value { get; set; }
    }
}
