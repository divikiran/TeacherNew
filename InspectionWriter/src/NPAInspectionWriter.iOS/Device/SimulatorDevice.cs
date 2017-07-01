using NPAInspectionWriter.Device;
using NPAInspectionWriter.Services;
using UIKit;

namespace NPAInspectionWriter.iOS.Device
{
    /// <summary>
    /// Apple device Simulator.
    /// </summary>
    public class Simulator : AppleDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Simulator" /> class.
        /// </summary>
        public Simulator( IAccelerometer accelerometer, IGyroscope gyroscope, IDisplay display, IBattery battery, IMediaPicker mediaPicker, IFileManager fileManager, INetwork network ) :
            base( null, accelerometer, gyroscope, display, battery, mediaPicker, fileManager, network )
        {
            HardwareVersion = "Simulator";
        }

        public static IDisplay GetDeviceDisplay()
        {
            var bounds = UIScreen.MainScreen.Bounds;
            var height = bounds.Height * UIScreen.MainScreen.Scale;
            var width = bounds.Width * UIScreen.MainScreen.Scale;
            var dpi = UIScreen.MainScreen.Scale * 163;
            return new Display( ( int )height, ( int )width, dpi, dpi );
        }

        public override void GetVersion( DeviceInfo deviceInfo )
        {

        }
    }
}
