using System.Globalization;
using NPAInspectionWriter.Converters;
using NPAInspectionWriter.AppData;

namespace NPAInspectionWriter.Converters
{
    public class CurrentScoreConverter : StringFormatBaseConverter
    {
        public override string DefaultValue { get; } = "0";

        public override string Convert( string value, CultureInfo culture ) =>
            $"{AppResources.CurrentInspectionScore}: {value}";
    }
}
