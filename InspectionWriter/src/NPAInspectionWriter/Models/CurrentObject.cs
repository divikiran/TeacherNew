using SQLite.Net.Attributes;

namespace NPAInspectionWriter.Models
{
    public class CurrentObject
    {
        [PrimaryKey]
        public string Key { get; set; }

        public string ObjectValue { get; set; }
    }
}
