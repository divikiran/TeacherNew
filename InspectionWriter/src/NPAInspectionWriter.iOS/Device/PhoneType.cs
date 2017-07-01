using NPAInspectionWriter.Helpers;

namespace NPAInspectionWriter.iOS.Device
{
    /// <summary>
    /// The phone type.
    /// </summary>
    public enum PhoneType
    {
        /// <summary>
        /// Unknown phone type.
        /// </summary>
        [Description( "Unknown device" )]
        Unknown = 0,

        [DeviceInfo( "3.1.3", DeviceProcessor.ARMv6, 1, 3.5, 1, 1 )]
        [Description( "iPhone (1st generation)" )]
        iPhone,

        [DeviceInfo( "4.2", DeviceProcessor.ARMv6, 1, 3.5, 1, 2 )]
        [Description( "iPhone 3G" )]
        iPhone3G,

        [DeviceInfo( "6.1.4", DeviceProcessor.ARMv7, 1, 3.5, 2, 1 )]
        [Description( "iPhone 3GS" )]
        iPhone3GS,

        [DeviceInfo( "7.1.1", DeviceProcessor.ARMv7, 2, 3.5, 3, 1 )]
        [Description( "iPhone 4 (GSM)" )]
        iPhone4,

        [DeviceInfo( "7.1.1", DeviceProcessor.ARMv7, 2, 3.5, 3, 2 )]
        [Description( "iPhone 4 (GSM Rev A)" )]
        iPhone4RevA,

        [DeviceInfo( "7.1.1", DeviceProcessor.ARMv7, 2, 3.5, 3, 3 )]
        [Description( "iPhone 4 (CDMA)" )]
        iPhone4CDMA,

        [DeviceInfo( "9", DeviceProcessor.ARMv7, 2, 3.5, 4, 1 )]
        [Description( "iPhone 4S" )]
        iPhone4S,

        [DeviceInfo( "10+", DeviceProcessor.ARMv7, 2, 4, 5, 1 )]
        [Description( "iPhone 5 (GSM/LTE)" )]
        iPhone5,

        [DeviceInfo( "10+", DeviceProcessor.ARMv7, 2, 4, 5, 2 )]
        [Description( "iPhone 5 (CDMA/LTE)" )]
        iPhone5CDMA,

        [DeviceInfo( "10+", DeviceProcessor.ARMv7, 2, 4, 5, 3 )]
        [Description( "iPhone 5c (GSM/LTE)" )]
        iPhone5cGSM,

        [DeviceInfo( "10+", DeviceProcessor.ARMv7, 2, 4, 5, 4 )]
        [Description( "iPhone 5c (CDMA/LTE)" )]
        iPhone5cCDMA,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 4, 6, 1 )]
        [Description( "iPhone 5s (GSM/LTE)" )]
        iPhone5sGSM,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 4, 6, 2 )]
        [Description( "iPhone 5s (CDMA/LTE)" )]
        iPhone5sCDMA,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 3, 5.5, 7, 1 )]
        [Description( "iPhone 6 Plus" )]
        iPhone6Plus,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 4.7, 7, 2 )]
        [Description( "iPhone 6" )]
        iPhone6,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 4.7, 8, 1 )]
        [Description( "iPhone 6s" )]
        iPhone6s,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 3, 5.5, 8, 2 )]
        [Description( "iPhone 6s Plus" )]
        iPhone6sPlus,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 4, 8, 4 )]
        [Description( "iPhone SE" )]
        iPhoneSE,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 4.7, 9, 1 )]
        [Description( "iPhone 7 (CDMA+GSM/LTE)" )]
        iPhone7CDMA_GSM,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 2, 4.7, 9, 3 )]
        [Description( "iPhone 7 (GSM/LTE)" )]
        iPhone7GSM,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 3, 5.5, 9, 2 )]
        [Description( "iPhone 7 Plus (CDMA+GSM/LTE)" )]
        iPhone7PlusCDMA_GSM,

        [DeviceInfo( "10+", DeviceProcessor.ARM64, 3, 5.5, 9, 4 )]
        [Description( "iPhone 7 Plus (GSM/LTE)" )]
        iPhone7PlusGSM
    }
}
