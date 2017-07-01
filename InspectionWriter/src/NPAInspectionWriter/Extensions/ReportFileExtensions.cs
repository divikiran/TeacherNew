using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.Models;

namespace NPAInspectionWriter.Extensions
{
    public static class ReportFileExtensions
    {
        /// <summary>
        /// Gets the Display text for the specified Enum Value
        /// </summary>
        /// <param name="reportFile"></param>
        /// <returns>Display Text</returns>
        public static string GetDisplayText( this ReportFile reportFile )
        {
            return reportFile.GetAttribute<DisplayTextAttribute>().Text;
        }

        /// <summary>
        /// Gets the Report File Name {fileName.rpt}
        /// </summary>
        /// <param name="reportFile"></param>
        /// <returns>Report File Name</returns>
        public static string GetReportFileName( this ReportFile reportFile )
        {
            return reportFile.GetAttribute<ReportFileNameAttribute>().FileName;
        }
    }
}
