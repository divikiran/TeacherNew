using System;
using System.Collections.Generic;

namespace NPAInspectionWriter.DTO
{
    public class InspectionItemDTO
    {
        public Guid? InspectionItemID { get; set; }
        public Guid? InspectionID { get; set; }
        public string InspectionItemName { get; set; }
        public Guid? InspectionCategoryID { get; set; }
        public Guid? InspectionOptionID { get; set; }
        public string InspectionCategory { get; set; }
        public string ItemScore { get; set; }
        public string ItemComments { get; set; }
        public bool IncludeInHeader { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public double MaxScore { get; set; }
        public double Weight { get; set; }
        public double Score { get; set; }

        public List<InspectionOptionDTO> InspectionOptions { get; set; }
    }
}
