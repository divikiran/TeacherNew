using Newtonsoft.Json;

namespace NPAInspectionWriter.Models
{
    [JsonObject]
    public class InspectionType
    {
        public InspectionType()
        {
            
        }

        public InspectionType(NPA.CodeGen.InspectionType inspectionType)
        {
            InspectionTypeId = inspectionType.InspectionTypeId;
            DisplayName = inspectionType.DisplayName;
        }

        [JsonProperty( "inspectionTypeId" )]
        public int InspectionTypeId { get; set; }

        [JsonProperty( "displayName" )]
        public string DisplayName { get; set; }
    }
}
