using System.ComponentModel;

namespace NPAInspectionWriter.iOS.Device
{
    /// <summary>
    /// Enum PodVersion
    /// </summary>
    public enum PodVersion
    {
        [Description( "Unknown Device" )]
        Unknown = 0,

        [DeviceInfo( "3.1.3", DeviceProcessor.ARMv6, 1, 3.5, 1, 1 )]
        [Description( "iPod touch (1st generation)" )]
        iPodTouch1,

        [DeviceInfo( "4.2", DeviceProcessor.ARMv6, 1, 3.5, 2, 1 )]
        [Description( "iPod touch (2nd generation)" )]
        iPodTouch2,

        [DeviceInfo( "5.1.1", DeviceProcessor.ARMv7, 1, 3.5, 3, 1 )]
        [Description( "iPod touch (3rd generation)" )]
        iPodTouch3,

        [DeviceInfo( "6.1.4", DeviceProcessor.ARMv7, 1, 3.5, 4, 1 )]
        [Description( "iPod touch (4th generation)" )]
        iPodTouch4,

        [DeviceInfo( "9", DeviceProcessor.ARMv7, 2, 4, 5, 1 )]
        [Description( "iPod touch (5th generation)" )]
        iPodTouch5,

        [DeviceInfo( "10", DeviceProcessor.ARM64, 2, 4, 7, 1 )]
        [Description( "iPod touch (6th generation)" )]
        iPodTouch6,
    }
}
