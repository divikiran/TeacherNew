using System;
namespace NPAInspectionWriter.Models
{
    public class InspectionDisplayList
    {
        public InspectionDisplayList(Inspection inspection)
        {
            GroupText = $"INSPECTION FOR {inspection.StockNumber}";
            Text = inspection.InspectionType;
            DetailsText = $"{inspection.InspectionDate.ToString("MMMM dd, yyyy hh:mm tt")} - Score {inspection.Score} ({inspection.InspectionUser})";
            //DateTime.Now.ToString("");
            this.inspection = inspection;
        }

        public string GroupText
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public string DetailsText
        {
            get;
            set;
        }

        public Inspection inspection { get; set; }
    }
}
