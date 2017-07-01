using System;
using System.Globalization;
using Xamarin.Forms;

namespace NPAInspectionWriter.Converters
{
    /// <summary>
    /// Creates an <see cref="ImageSource"/> from the a string
    /// that is either a file or a URI.
    /// </summary>
    public class ImageSourceConverter : TypeConverter
    {
        /// <summary>
        /// Checks to see if the type attempted to be converted from is a string.
        /// </summary>
        /// <param name="sourceType">The type that is attempting to be converted.</param>
        /// <returns>Returns true if the sourceType is a <see cref="string"/>.</returns>
        public override bool CanConvertFrom( Type sourceType )
        {
            return sourceType == typeof( string );
        }

        /// <summary>
        /// Converts the string value into a <see cref="ImageSource"/>  either from a file or URI.
        /// </summary>
        /// <param name="value">The string value to convert.</param>
        /// <returns>Returns a <see cref="ImageSource"/> loaded from the value</returns>
        public override object ConvertFromInvariantString( string value )
        {
            if( string.IsNullOrWhiteSpace( value ) ) return null;

            Uri result;
            if( !Uri.TryCreate( value, UriKind.Absolute, out result ) || !(result.Scheme != "file" ) )
            {
                return ImageSource.FromFile( value );
            }

            return ImageSource.FromUri( result );
        }
    }
}
