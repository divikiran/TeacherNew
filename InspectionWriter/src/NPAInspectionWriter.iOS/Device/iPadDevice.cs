using NPAInspectionWriter.Extensions;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.Services;
using NPAInspectionWriter.Device;

namespace NPAInspectionWriter.iOS.Device
{
    /// <summary>
    /// Apple iPad.
    /// </summary>
    public class iPadDevice : AppleDevice
    {
        public iPadDevice( DeviceInfo deviceInfo, IAccelerometer accelerometer, IGyroscope gyroscope, IDisplay display, IBattery battery, IMediaPicker mediaPicker, IFileManager fileManager, INetwork network ) :
            base( deviceInfo, accelerometer, gyroscope, display, battery, mediaPicker, fileManager, network )
        {

            System.Diagnostics.Debug.WriteLine( Version == IPadVersion.Unknown, $"Unknown iPad device: {deviceInfo.MajorVersion} : {deviceInfo.MinorVersion}" );

            HardwareVersion = Version.GetAttribute<DescriptionAttribute>()?.Description;
        }

        /// <summary>
        /// Gets the version of the iPad.
        /// </summary>
        /// <value>The version.</value>
        public IPadVersion Version { get; private set; }

        public static IDisplay GetDeviceDisplay( DeviceInfo deviceInfo )
        {
            Display display;
            double dpi;
            switch( deviceInfo.MajorVersion )
            {
                case 1:
                    display = new Display( 1024, 768, 132, 132 );
                    break;
                case 2:
                    dpi = deviceInfo.MinorVersion > 4 ? 163 : 132;
                    display = new Display( 1024, 768, dpi, dpi );
                    break;
                case 3:
                    display = new Display( 2048, 1536, 264, 264 );
                    break;
                case 4:
                    dpi = deviceInfo.MinorVersion > 3 ? 326 : 264;
                    display = new Display( 2048, 1536, dpi, dpi );
                    break;
                case 5:
                case 6:
                    display = new Display( 2048, 1536, 326, 326 );
                    break;
                default:
                    display = new Display( 0, 0, 0, 0 );
                    break;
            }
            return display;
        }

        public override void GetVersion( DeviceInfo deviceInfo )
        {
            Version = deviceInfo.GetVersionByDeviceInfo<IPadVersion>();
        }
    }
}
