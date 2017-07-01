using System;

namespace NPAInspectionWriter.Extensions
{
    public static class ParsingExtensions
    {
        public static T TryParse<T>( this T input, object value, T defaultValue )
        {
            try
            {
                input = ( T )Convert.ChangeType( value, typeof( T ) );
            }
            catch( Exception )
            {
                input = defaultValue;
            }

            return input;
        }

        public static int TryParse( this int input, object value, int defaultValue )
        {
            try
            {
                input = int.Parse( value.ToString() );
            }
            catch( Exception )
            {
                input = defaultValue;
            }

            return input;
        }

        public static double TryParse( this double input, object value, double defaultValue )
        {
            try
            {
                input = double.Parse( value.ToString() );
            }
            catch( Exception )
            {
                input = defaultValue;
            }

            return input;
        }
    }
}
