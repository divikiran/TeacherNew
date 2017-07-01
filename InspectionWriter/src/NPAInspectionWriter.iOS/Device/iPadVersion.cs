using NPAInspectionWriter.Helpers;

namespace NPAInspectionWriter.iOS.Device
{
    /// <summary>
    /// Enum IPadVersion
    /// </summary>
    public enum IPadVersion
    {
        /// <summary>
        /// The unknown
        /// </summary>
        [Description( "Unknown" )]
        Unknown = 0,

        [DeviceInfo( "5.1.1", DeviceProcessor.ARMv7, 1, 9.7, 1, 1 )]
        [Description( "iPad" )]
        iPad,

        [DeviceInfo( "9", DeviceProcessor.ARMv7, 1, 9.7, 2, 1 )]
        [Description( "iPad 2 (Wi-Fi)" )]
        iPad2Wifi,

        [DeviceInfo( "9", DeviceProcessor.ARMv7, 1, 9.7, 2, 2 )]
        [Description( "iPad 2 (GSM)" )]
        iPad2GSM,

        [DeviceInfo( "9", DeviceProcessor.ARMv7, 1, 9.7, 2, 3 )]
        [Description( "iPad 2 (CDMA)" )]
        iPad2CDMA,

        [DeviceInfo( "9", DeviceProcessor.ARMv7, 1, 9.7, 2, 4 )]
        [Description( "iPad 2 (Wi-Fi, A5R)" )]
        iPad2WifiA5R,

        [DeviceInfo( "9", DeviceProcessor.ARMv7, 2, 9.7, 3, 1 )]
        [Description( "iPad (3rd generation, Wi-Fi)" )]
        iPad3Wifi,

        [DeviceInfo( "9", DeviceProcessor.ARMv7, 2, 9.7, 3, 2 )]
        [Description( "iPad (3rd generation, GSM/LTE)" )]
        iPad3GSM,

        [DeviceInfo( "9", DeviceProcessor.ARMv7, 2, 9.7, 3, 3 )]
        [Description( "iPad (3rd generation, CDMA/LTE)" )]
        iPad3CDMA,

        [DeviceInfo( "10+", DeviceProcessor.ARMv7s, 2, 9.7, 3, 4 )]
        [Description( "iPad (4th generation, Wi-Fi)" )]
        iPad4Wifi,

        [DeviceInfo( "10+", DeviceProcessor.ARMv7s, 2, 9.7, 3, 5 )]
        [Description( "iPad (4th generation, GSM/LTE)" )]
        iPad4GSM,

        [DeviceInfo( "10+", DeviceProcessor.ARMv7s, 2, 9.7, 3, 6 )]
        [Description( "iPad (4th generation, CDMA/LTE)" )]
        iPad4CDMA,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 9.7, 4, 1 )]
        [Description( "iPad Air (Wi-Fi)" )]
        iPadAirWifi,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 9.7, 4, 2 )]
        [Description( "iPad Air (LTE)" )]
        iPadAirLTE,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 9.7, 5, 3 )]
        [Description( "iPad Air 2 (Wi-Fi)" )]
        iPadAir2Wifi,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 9.7, 5, 4 )]
        [Description( "iPad Air 2 (LTE)" )]
        iPadAir2LTE,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 12.9, 6, 7 )]
        [Description( "12.9-inch iPad Pro (Wi-Fi)" )]
        iPadPro12_9Wifi,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 12.9, 6, 8 )]
        [Description( "12.9-inch iPad Pro (LTE)" )]
        iPadPro12_9LTE,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 9.7, 6, 3 )]
        [Description( "9.7-inch iPad Pro (Wi-Fi)" )]
        iPadPro9_7Wifi,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 9.7, 6, 4 )]
        [Description( "9.7-inch iPad Pro (LTE)" )]
        iPadPro9_7LTE,

        #region iPad Mini

        [DeviceInfo( "9", DeviceProcessor.ARMv7, 1, 7.9, 2, 5 )]
        [Description( "iPad mini (Wi-Fi)" )]
        iPadMiniWifi,

        [DeviceInfo( "9", DeviceProcessor.ARMv7, 1, 7.9, 2, 6 )]
        [Description( "iPad mini (GSM/LTE)" )]
        iPadMiniGSM,

        [DeviceInfo( "9", DeviceProcessor.ARMv7, 1, 7.9, 2, 7 )]
        [Description( "iPad mini (CDMA/LTE)" )]
        iPadMiniCDMA,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 7.9, 4, 4 )]
        [Description( "iPad mini with Retina display (Wi-Fi)" )]
        iPadMini2Wifi,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 7.9, 4, 5 )]
        [Description( "iPad mini with Retina display (LTE)" )]
        iPadMini2LTE,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 7.9, 4, 6 )]
        [Description( "iPad mini with Retina display (China)" )]
        iPadMini2China,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 7.9, 4, 7 )]
        [Description( "iPad mini 3 (Wi-Fi)" )]
        iPadMini3Wifi,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 7.9, 4, 8 )]
        [Description( "iPad mini 3 (LTE)" )]
        iPadMini3LTE,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 7.9, 4, 9 )]
        [Description( "iPad mini 3 (China)" )]
        iPadMini3China,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 7.9, 5, 1 )]
        [Description( "iPad mini 4 (Wi-Fi)" )]
        iPadMini4Wifi,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 7.9, 5, 2 )]
        [Description( "iPad mini 4 (LTE)" )]
        iPadMini4LTE,

        #endregion
    }
}
