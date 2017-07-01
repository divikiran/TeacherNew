using System;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms;

namespace NPAInspectionWriter.Converters
{
    public class EnumToHumanizedStringConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            try
            {
                return ( value.ToString() );
            }
            catch( Exception e )
            {
                Debug.WriteLine( e );
                return string.Empty;
            }
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            var enumString = ( value as string );
            return Enum.Parse( targetType, enumString );
        }
    }
}
