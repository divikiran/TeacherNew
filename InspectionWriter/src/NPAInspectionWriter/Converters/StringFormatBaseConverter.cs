using System;
using System.Globalization;
using Xamarin.Forms;

namespace NPAInspectionWriter.Converters
{
    public abstract class StringFormatBaseConverter : IValueConverter
    {
        public abstract string DefaultValue { get; }


        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            var stringValue = DefaultValue;

            if( value != null )
                stringValue = value.ToString();

            return Convert( stringValue, culture );
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }

        public abstract string Convert( string value, CultureInfo culture );
    }
}
