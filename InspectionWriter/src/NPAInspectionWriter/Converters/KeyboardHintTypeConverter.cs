using System;
using System.Globalization;
using NPAInspectionWriter.Helpers;
using Xamarin.Forms;

namespace NPAInspectionWriter.Converters
{
    public class KeyboardHintTypeConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value == null ) return null;

            return KeyboardHints.GetHints( ( KeyboardHintType )value );

        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
