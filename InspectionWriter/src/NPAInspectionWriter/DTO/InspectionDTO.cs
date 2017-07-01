using System;
using System.Collections.Generic;

namespace NPAInspectionWriter.DTO
{
    public class InspectionDTO : BaseDTO
    {
        public Guid? VehicleID { get; set; }
        public Guid? ProductSegmentID { get; set; }
        public Guid? VehicleCategoryID { get; set; }
        public Guid? InspectionID { get; set; }
        public Guid? InspectionMasterID { get; set; }
        public Guid? InspectionUserID { get; set; }
        public Guid? InspectionLocationID { get; set; }
        public DateTime InspectionDate { get; set; }
        public string InspectionNumber { get; set; }
        public string InspectionUser { get; set; }
        public string InspectionComments { get; set; }
        public int InspectionType { get; set; }
        public float InspectionScore { get; set; }

        public List<InspectionItemDTO> InspectionItems { get; set; } = new List<InspectionItemDTO>();

        public int CalcScore()
        {
            double tempScore = 0;
            double totalScore = 0;
            double totalWeight = 0;
            double maxInspectionScore = 100;

            foreach( var ii in this.InspectionItems )
            {
                tempScore = ( ii.Score / ii.MaxScore ) * ii.Weight;
                totalScore += tempScore;
                totalWeight += ii.Weight;
            }
            totalScore = ( totalScore / totalWeight ) * maxInspectionScore;
            return ( int )Math.Round( totalScore );
        }
    }
}
