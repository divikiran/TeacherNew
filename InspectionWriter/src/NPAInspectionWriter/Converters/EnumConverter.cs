using System;
using System.Diagnostics;
using System.Globalization;
using NPAInspectionWriter.Logging;
using Xamarin.Forms;

namespace NPAInspectionWriter.Converters
{
    public class EnumConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            try
            {
                return ( value.ToString() );
            }
            catch( Exception e )
            {
                StaticLogger.ExceptionLogger?.Invoke( e );
                return null;
            }
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            var enumString = ( value as string );
            return Enum.Parse( targetType, enumString );
        }
    }
}
