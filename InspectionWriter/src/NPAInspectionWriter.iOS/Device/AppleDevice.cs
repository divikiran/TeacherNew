using System;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Foundation;
using NPAInspectionWriter.Device;
using NPAInspectionWriter.Services;
using ObjCRuntime;
using UIKit;

namespace NPAInspectionWriter.iOS.Device
{
    /// <summary>
    /// Apple device base class.
    /// </summary>
    public abstract class AppleDevice : IDevice
    {
        /// <summary>
        /// The iPhone expression.
        /// </summary>
        protected const string PhoneExpression = "iPhone([1-9]),([1-4])";

        /// <summary>
        /// The iPod expression.
        /// </summary>
        protected const string PodExpression = "iPod([1-9]),([1])";

        /// <summary>
        /// The iPad expression.
        /// </summary>
        protected const string PadExpression = "iPad([1-9]),([1-9])";

        /// <summary>
        /// Generic CPU/IO.
        /// </summary>
        private const int CtlHw = 6;

        /// <summary>
        /// Total memory.
        /// </summary>
        private const int HwPhysmem = 5;

        /// <summary>
        /// The device.
        /// </summary>
        private static IDevice device;

        private static bool? s_isiOS7OrNewer;

        private static bool? s_isiOS8OrNewer;

        private static bool? s_isiOS9OrNewer;

        private static bool? s_isiOS10OrNewer;

        private static readonly long DeviceTotalMemory = GetTotalMemory();

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleDevice" /> class.
        /// </summary>
        protected AppleDevice( DeviceInfo deviceInfo, IAccelerometer accelerometer, IGyroscope gyroscope, IDisplay display, IBattery battery, IMediaPicker mediaPicker, IFileManager fileManager, INetwork network )
        {
            Accelerometer = accelerometer;

            Display = display;
            Battery = battery;
            //BluetoothHub = bluetoothHub;
            FileManager = fileManager;
            MediaPicker = mediaPicker;
            Network = network;

            GetVersion( deviceInfo );
            Name = UIDevice.CurrentDevice.Name;
            FirmwareVersion = UIDevice.CurrentDevice.SystemVersion;

            CurrentDevice = this;
        }

        public static bool IsiOS7OrNewer
        {
            get
            {
                if( !s_isiOS7OrNewer.HasValue )
                    s_isiOS7OrNewer = UIDevice.CurrentDevice.CheckSystemVersion( 7, 0 );
                return s_isiOS7OrNewer.Value;
            }
        }

        public static bool IsiOS8OrNewer
        {
            get
            {
                if( !s_isiOS8OrNewer.HasValue )
                    s_isiOS8OrNewer = UIDevice.CurrentDevice.CheckSystemVersion( 8, 0 );
                return s_isiOS8OrNewer.Value;
            }
        }

        public static bool IsiOS9OrNewer
        {
            get
            {
                if( !s_isiOS9OrNewer.HasValue )
                    s_isiOS9OrNewer = UIDevice.CurrentDevice.CheckSystemVersion( 9, 0 );
                return s_isiOS9OrNewer.Value;
            }
        }

        public static bool IsiOS10OrNewer
        {
            get
            {
                if( !s_isiOS10OrNewer.HasValue )
                    s_isiOS10OrNewer = UIDevice.CurrentDevice.CheckSystemVersion( 10, 0 );
                return s_isiOS10OrNewer.Value;
            }
        }


        public abstract void GetVersion( DeviceInfo deviceInfo );

        /// <summary>
        /// Gets the runtime device for Apple's devices.
        /// </summary>
        /// <value>The current device.</value>
        public static IDevice CurrentDevice { get; internal set; }

        /// <summary>
        /// Sysctlbynames the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="output">The output.</param>
        /// <param name="oldLen">The old length.</param>
        /// <param name="newp">The newp.</param>
        /// <param name="newlen">The newlen.</param>
        /// <returns>System.Int32.</returns>
        [DllImport( Constants.SystemLibrary )]
        internal static extern int sysctlbyname(
            [MarshalAs( UnmanagedType.LPStr )] string property,
            IntPtr output,
            IntPtr oldLen,
            IntPtr newp,
            uint newlen );

        [DllImport( Constants.SystemLibrary )]
        static internal extern int sysctl(
            [MarshalAs( UnmanagedType.LPArray )] int[] name,
            uint namelen,
            out uint oldp,
            ref int oldlenp,
            IntPtr newp,
            uint newlen );


        /// <summary>
        /// Gets the system property.
        /// </summary>
        /// <param name="property">Property to get.</param>
        /// <returns>The system property value.</returns>
        public static string GetSystemProperty( string property )
        {
            var pLen = Marshal.AllocHGlobal( sizeof( int ) );
            sysctlbyname( property, IntPtr.Zero, pLen, IntPtr.Zero, 0 );
            var length = Marshal.ReadInt32( pLen );
            var pStr = Marshal.AllocHGlobal( length );
            sysctlbyname( property, pStr, pLen, IntPtr.Zero, 0 );
            return Marshal.PtrToStringAnsi( pStr );
        }

        #region IDevice implementation

        /// <summary>
        /// Gets Unique Id for the device.
        /// </summary>
        /// <value>The id for the device.</value>
        public virtual string Id
        {
            get
            {
                return UIDevice.CurrentDevice.IdentifierForVendor.AsString();
            }
        }

        /// <summary>
        /// Gets or sets the display information for the device.
        /// </summary>
        /// <value>The display.</value>
        public IDisplay Display { get; protected set; }

        /// <summary>
        /// Gets or sets the battery for the device.
        /// </summary>
        /// <value>The battery.</value>
        public IBattery Battery { get; protected set; }

        /// <summary>
        /// Gets the picture chooser.
        /// </summary>
        /// <value>The picture chooser.</value>
        public IMediaPicker MediaPicker { get; private set; }

        /// <summary>
        /// Gets the network service.
        /// </summary>
        /// <value>The network service.</value>
        public INetwork Network { get; private set; }

        /// <summary>
        /// Gets or sets the accelerometer for the device if available
        /// </summary>
        /// <value>Instance of IAccelerometer if available, otherwise null.</value>
        public IAccelerometer Accelerometer { get; }

        /// <summary>
        /// Gets the gyroscope.
        /// </summary>
        /// <value>The gyroscope instance if available, otherwise null.</value>
        public IGyroscope Gyroscope { get; }

        /// <summary>
        /// Gets the file manager for the device.
        /// </summary>
        /// <value>Device file manager.</value>
        public IFileManager FileManager { get; }

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        /// <value>The name of the device.</value>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the firmware version.
        /// </summary>
        /// <value>The firmware version.</value>
        public string FirmwareVersion { get; }

        /// <summary>
        /// Gets or sets the hardware version.
        /// </summary>
        /// <value>The hardware version.</value>
        public string HardwareVersion { get; protected set; }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        /// <value>The manufacturer.</value>
        public string Manufacturer { get { return "Apple"; } }

        /// <summary>
        /// Gets the bluetooth hub service.
        /// </summary>
        /// <value>The bluetooth hub service if available, otherwise null.</value>
        public IBluetoothHub BluetoothHub { get; }

        /// <summary>
        /// Gets the total memory in bytes.
        /// </summary>
        /// <value>The total memory in bytes.</value>
        public long TotalMemory
        {
            get
            {
                return DeviceTotalMemory;
            }
        }

        /// <summary>
        /// Gets the language code.
        /// </summary>
        /// <value>The language code.</value>
        public string LanguageCode
        {
            get { return NSLocale.PreferredLanguages[ 0 ]; }
        }

        /// <summary>
        /// Gets the time zone offset.
        /// </summary>
        /// <value>The time zone offset.</value>
        public double TimeZoneOffset
        {
            get { return NSTimeZone.LocalTimeZone.GetSecondsFromGMT / 3600.0; }
        }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public string TimeZone
        {
            get { return NSTimeZone.LocalTimeZone.Name; }
        }

        /// <summary>
        /// Gets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        public Orientation Orientation
        {
            get
            {
                switch( UIApplication.SharedApplication.StatusBarOrientation )
                {
                    case UIInterfaceOrientation.LandscapeLeft:
                        return Orientation.Landscape & Orientation.LandscapeLeft;
                    case UIInterfaceOrientation.Portrait:
                        return Orientation.Portrait & Orientation.PortraitUp;
                    case UIInterfaceOrientation.PortraitUpsideDown:
                        return Orientation.Portrait & Orientation.PortraitDown;
                    case UIInterfaceOrientation.LandscapeRight:
                        return Orientation.Landscape & Orientation.LandscapeRight;
                    default:
                        return Orientation.None;
                }
            }
        }

        public bool IsInvokeRequired
        {
            get { return !NSThread.IsMain; }
        }

        public Assembly[] GetAssemblies() =>
            AppDomain.CurrentDomain.GetAssemblies();

        public string GetMD5Hash( string input )
        {
            var bytes = Checksum.ComputeHash( Encoding.UTF8.GetBytes( input ) );
            var ret = new char[ 32 ];
            for( var i = 0; i < 16; i++ )
            {
                ret[ i * 2 ] = ( char )Hex( bytes[ i ] >> 4 );
                ret[ i * 2 + 1 ] = ( char )Hex( bytes[ i ] & 0xf );
            }
            return new string( ret );
        }

        /// <summary>
        /// Starts the default app associated with the URI for the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>The launch operation.</returns>
        public Task<bool> LaunchUriAsync( Uri uri )
        {
            var launchTaskSource = new TaskCompletionSource<bool>();

            try
            {
                var url = NSUrl.FromString( uri.ToString() ) ?? new NSUrl( uri.Scheme, uri.Host, uri.LocalPath );
                return LaunchUriAsync( url );
            }
            catch( Exception exception )
            {
                launchTaskSource.SetException( exception );
                return launchTaskSource.Task;
            }
        }

        public Task<bool> LaunchUriAsync( string uri )
        {
            var url = new NSUrl( uri );
            return LaunchUriAsync( url );
        }

        public Task<bool> LaunchUriAsync( NSUrl url )
        {
            var launchTaskSource = new TaskCompletionSource<bool>();
            var app = UIApplication.SharedApplication;
            app.BeginInvokeOnMainThread( () =>
            {
                try
                {
                    var result = app.CanOpenUrl( url ) && app.OpenUrl( url );
                    launchTaskSource.SetResult( result );
                }
                catch( Exception exception )
                {
                    launchTaskSource.SetException( exception );
                }
            } );

            return launchTaskSource.Task;
        }

        #endregion

        /// <summary>
        /// Gets the hardware's <see cref="DeviceInfo"/>.
        /// </summary>
        /// <returns><see cref="DeviceInfo"/></returns>
        /// <exception cref="Exception">Throws an exception if unable to determine device type.</exception>
        public static DeviceInfo GetDeviceInfo()
        {
            var hardwareVersion = GetSystemProperty( "hw.machine" );

#if DEBUG
            var device = UIDevice.CurrentDevice;
            System.Diagnostics.Debug.WriteLine( "--------UI DEVICE--------" );
            System.Diagnostics.Debug.WriteLine( device.Description );
            System.Diagnostics.Debug.WriteLine( device.IdentifierForVendor );
            System.Diagnostics.Debug.WriteLine( device.IsMultitaskingSupported );
            System.Diagnostics.Debug.WriteLine( device.LocalizedModel );
            System.Diagnostics.Debug.WriteLine( device.Model );
            System.Diagnostics.Debug.WriteLine( device.Name );
            System.Diagnostics.Debug.WriteLine( device.Orientation );
            System.Diagnostics.Debug.WriteLine( device.SystemName );
            System.Diagnostics.Debug.WriteLine( device.SystemVersion );
            System.Diagnostics.Debug.WriteLine( device.Zone );
            System.Diagnostics.Debug.WriteLine( $"Hardware Version: {hardwareVersion}" );
            System.Diagnostics.Debug.WriteLine( "--------END UIDEVICE--------" );
#endif

            var regex = new Regex( PhoneExpression ).Match( hardwareVersion );
            if( regex.Success )
            {
                return new DeviceInfo( DeviceType.Phone, int.Parse( regex.Groups[ 1 ].Value ), int.Parse( regex.Groups[ 2 ].Value ) );
            }

            regex = new Regex( PodExpression ).Match( hardwareVersion );
            if( regex.Success )
            {
                return new DeviceInfo( DeviceType.Pod, int.Parse( regex.Groups[ 1 ].Value ), int.Parse( regex.Groups[ 2 ].Value ) );
            }

            regex = new Regex( PadExpression ).Match( hardwareVersion );
            if( regex.Success )
            {
                return new DeviceInfo( DeviceType.Pad, int.Parse( regex.Groups[ 1 ].Value ), int.Parse( regex.Groups[ 2 ].Value ) );
            }

            return new DeviceInfo( DeviceType.Simulator, 0, 0 );
        }

        private static uint GetTotalMemory()
        {
            var oldlenp = sizeof( int );
            var mib = new int[ 2 ] { CtlHw, HwPhysmem };

            uint mem;
            sysctl( mib, 2, out mem, ref oldlenp, IntPtr.Zero, 0 );

            return mem;
        }

        private static readonly MD5CryptoServiceProvider Checksum = new MD5CryptoServiceProvider();

        private static int Hex( int v )
        {
            if( v < 10 )
                return '0' + v;
            return 'a' + v - 10;
        }

        public async Task DownloadFileToPathAsync( Uri uri, string savePath )
        {
            var client = new WebClient();
            await client.DownloadFileTaskAsync( uri, savePath );
        }

        public async Task DownloadFileToPathAsync( string uri, string savePath ) =>
            await DownloadFileToPathAsync( new Uri( uri ), savePath );
    }
}
