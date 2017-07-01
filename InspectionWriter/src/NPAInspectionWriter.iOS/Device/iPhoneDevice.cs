using System.ComponentModel;
using NPAInspectionWriter.Device;
using NPAInspectionWriter.Extensions;
using NPAInspectionWriter.Services;

namespace NPAInspectionWriter.iOS.Device
{
    /// <summary>
    /// Apple iPhone.
    /// </summary>
    public class iPhoneDevice : AppleDevice
    {


        /// <summary>
        /// Initializes a new instance of the <see cref="iPhoneDevice" /> class.
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="accelerometer"></param>
        /// <param name="gyroscope"></param>
        /// <param name="display"></param>
        /// <param name="battery"></param>
        /// <param name="mediaPicker"></param>
        /// <param name="bluetoothHub"></param>
        /// <param name="fileManager"></param>
        /// <param name="network"></param>
        public iPhoneDevice( DeviceInfo deviceInfo, IAccelerometer accelerometer, IGyroscope gyroscope, IDisplay display, IBattery battery, IMediaPicker mediaPicker, IFileManager fileManager, INetwork network ) :
            base( deviceInfo, accelerometer, gyroscope, display, battery, mediaPicker, fileManager, network )
        {
            System.Diagnostics.Debug.WriteLineIf( Version == PhoneType.Unknown, $"Unknown iPhone Device: {deviceInfo.MajorVersion} : {deviceInfo.MinorVersion}" );

            HardwareVersion = Version.GetAttribute<DescriptionAttribute>()?.Description;
        }

        /// <summary>
        /// Gets the version of iPhone.
        /// </summary>
        /// <value>The version.</value>
        public PhoneType Version { get; private set; }

        public static IDisplay GetDeviceDisplay( DeviceInfo deviceInfo )
        {
            PhoneType phoneType = deviceInfo.GetVersionByDeviceInfo<PhoneType>();
            IDisplay display;
            switch( phoneType )
            {
                case PhoneType.iPhone6:
                case PhoneType.iPhone6s:
                case PhoneType.iPhone7CDMA_GSM:
                case PhoneType.iPhone7GSM:
                    display = new Display( 1334, 750, 326, 326 );
                    break;
                case PhoneType.iPhone6Plus:
                case PhoneType.iPhone6sPlus:
                case PhoneType.iPhone7PlusCDMA_GSM:
                case PhoneType.iPhone7PlusGSM:
                    display = new Display( 2208, 1242, 401 * 1242 / 1080, 401 * 2208 / 1920 );
                    break;
                default:
                    if( deviceInfo.MajorVersion > 4 )
                    {
                        display = new Display( 1136, 640, 326, 326 );
                    }
                    else if( deviceInfo.MajorVersion > 2 )
                    {
                        display = new Display( 960, 640, 326, 326 );
                    }
                    else
                    {
                        display = new Display( 480, 320, 163, 163 );
                    }
                    break;
            }
            return display;
        }

        public override void GetVersion( DeviceInfo deviceInfo )
        {
            Version = deviceInfo.GetVersionByDeviceInfo<PhoneType>();
        }
    }
}
