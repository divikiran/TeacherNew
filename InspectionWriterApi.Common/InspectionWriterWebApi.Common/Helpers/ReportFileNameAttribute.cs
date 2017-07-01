using System;

namespace InspectionWriterWebApi.Helpers
{
    public class ReportFileNameAttribute : Attribute
    {
        public string FileName { get; set; }

        public ReportFileNameAttribute( string fileName )
        {
            FileName = fileName;
        }
    }
}
