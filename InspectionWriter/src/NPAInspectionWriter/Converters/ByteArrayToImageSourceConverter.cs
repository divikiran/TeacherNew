using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace NPAInspectionWriter.Converters
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value == null )
            {
                return null;
            }
            byte[] bytes = value as byte[];
            return ImageSource.FromStream( () => new MemoryStream( bytes ) );
        }
        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
