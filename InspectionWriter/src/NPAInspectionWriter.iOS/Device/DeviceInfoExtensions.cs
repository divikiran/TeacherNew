using System;

namespace NPAInspectionWriter.iOS.Device
{
    public static class DeviceInfoExtensions
    {
        public static T GetVersionByDeviceInfo<T>( this DeviceInfo deviceInfo )
        {
            var type = typeof( T );
            foreach( var field in type.GetFields() )
            {
                var attribute = Attribute.GetCustomAttribute( field, typeof( DeviceInfoAttribute ) ) as DeviceInfoAttribute;
                if( attribute?.MajorRevision == deviceInfo.MajorVersion && attribute?.MinorRevision == deviceInfo.MinorVersion )
                {
                    return ( T )field.GetValue( null );
                }
            }
            return default( T );
        }
    }
}
