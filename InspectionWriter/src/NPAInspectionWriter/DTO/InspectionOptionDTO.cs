using System;

namespace NPAInspectionWriter.DTO
{
    public class InspectionOptionDTO : BaseDTO
    {
        public Guid? InspectionOptionID { get; set; }
        public string InspectionOptionName { get; set; }
        public Guid? InspectionCategoryID { get; set; }
        public string InspectionCategory { get; set; }
        public string ScoreValue { get; set; }
    }
}
