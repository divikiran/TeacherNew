using System;

namespace NPAInspectionWriter.DTO
{
    public class TitleDTO : BaseDTO
    {
        public Guid? VehicleID { get; set; }
        public Guid? ProductSegmentID { get; set; }
        public Guid? VehicleDocID { get; set; }
        public string VehicleDoc { get; set; }
        public string VehicleDocState { get; set; }
        public bool VehicleDocRequired { get; set; }
        public DateTime? DatePosted { get; set; }
        public Guid? TitleFrontImageDocID { get; set; }
        public string TitleFrontImageUrl { get; set; }
        public Guid? TitleBackImageDocID { get; set; }
        public string TitleBackImageUrl { get; set; }
    }
}
