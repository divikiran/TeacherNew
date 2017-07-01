using CoreMedia;
using Foundation;

namespace NPAInspectionWriter.iOS.Extensions
{
    public static class NSObjectExtensions
    {
        public static NSObject ToNSObject( this object obj )
        {
            return NSObject.FromObject( obj );
        }

        //public static int ToInt( this NSObject obj )
        //{
        //    if( obj == null ) return default( int );

        //    int result = 0;
        //    int.TryParse( obj.ToString(), out result );
        //    return result;
        //}

        //public static bool ToBool( this NSObject obj )
        //{
        //    if( obj == null ) return default( bool );

        //    bool result = false;
        //    bool.TryParse( obj.ToString(), out result );
        //    return result;
        //}

        public static int AsInt( this NSObject nsObj )
        {
            var num = ( NSNumber )nsObj;
            return num.Int32Value;
        }

        public static float AsFloat( this NSObject nsObj )
        {
            var num = ( NSNumber )nsObj;
            return num.FloatValue;
        }

        public static bool AsBool( this NSObject nsObj )
        {
            return ( ( NSNumber )nsObj ).BoolValue;
        }

        public static CMTime AsCMTime( this NSObject nsObj )
        {
            return ( ( NSValue )nsObj ).CMTimeValue;
        }
    }
}
