using System.ComponentModel;
using NPAInspectionWriter.Device;
using NPAInspectionWriter.Extensions;
using NPAInspectionWriter.Services;

namespace NPAInspectionWriter.iOS.Device
{
    /// <summary>
    /// Apple iPod.
    /// </summary>
    public class iPodDevice : AppleDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="iPodDevice" /> class.
        /// </summary>
        public iPodDevice( DeviceInfo deviceInfo, IAccelerometer accelerometer, IGyroscope gyroscope, IDisplay display, IBattery battery, IMediaPicker mediaPicker, IFileManager fileManager, INetwork network ) :
            base( deviceInfo, accelerometer, gyroscope, display, battery, mediaPicker, fileManager, network )
        {
            System.Diagnostics.Debug.WriteLineIf( Version == PodVersion.Unknown, $"Unknown iPod Device: {deviceInfo.MajorVersion} : {deviceInfo.MinorVersion}" );

            HardwareVersion = Version.GetAttribute<DescriptionAttribute>().Description;
        }

        /// <summary>
        /// Gets the version of iPod.
        /// </summary>
        /// <value>The version.</value>
        public PodVersion Version { get; private set; }

        public static IDisplay GetDeviceDisplay( DeviceInfo deviceInfo )
        {
            IDisplay display;

            if( deviceInfo.MajorVersion > 4 )
            {
                display = new Display( 1136, 640, 326, 326 );
            }
            else if( deviceInfo.MajorVersion > 3 )
            {
                display = new Display( 960, 640, 326, 326 );
            }
            else
            {
                display = new Display( 480, 320, 163, 163 );
            }

            return display;
        }

        public override void GetVersion( DeviceInfo deviceInfo )
        {
            Version = deviceInfo.GetVersionByDeviceInfo<PodVersion>();
        }
    }
}
