using System.Globalization;
using NPAInspectionWriter.Converters;
using NPAInspectionWriter.AppData;

namespace NPAInspectionWriter.Converters
{
    public class PreviousValueConverter : StringFormatBaseConverter
    {
        public override string DefaultValue { get; } = AppResources.NotRated;

        public override string Convert( string value, CultureInfo culture ) =>
            string.Format( AppResources.PreviousValueStringFormat, value );
    }
}
