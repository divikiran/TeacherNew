using Foundation;

namespace NPAInspectionWriter.iOS.Extensions
{
    public static class NSDictionaryExtensions
    {
        public static void SetValueForKey( this NSDictionary dict, string value, string key )
        {
            dict.SetValueForKey( new NSString( value ), new NSString( key ) );
        }
    }
}
