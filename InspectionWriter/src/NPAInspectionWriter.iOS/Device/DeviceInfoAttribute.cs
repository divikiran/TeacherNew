using System;

namespace NPAInspectionWriter.iOS.Device
{
    public class DeviceInfoAttribute : Attribute
    {
        public DeviceInfoAttribute( string firmware, DeviceProcessor arm, int imageXFactor, double inches, int majorRevision, int minorRevision )
        {
            Firmware = firmware;
            ARM = arm;
            ImageXFactor = imageXFactor;
            Inches = inches;
            MajorRevision = majorRevision;
            MinorRevision = minorRevision;
        }

        public string Firmware { get; }
        public DeviceProcessor ARM { get; }
        public int ImageXFactor { get; }
        public double Inches { get; }
        public int MajorRevision { get; }
        public int MinorRevision { get; }
    }
}
