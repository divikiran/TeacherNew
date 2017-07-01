using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NPAInspectionWriter.Helpers
{
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum KeyboardHintType
    {
        None,
        General,
        Tire
    }
}
