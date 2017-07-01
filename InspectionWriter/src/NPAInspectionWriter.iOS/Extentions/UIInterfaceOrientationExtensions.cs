using NPAInspectionWriter.Device;
using NPAInspectionWriter.iOS.Device;
using UIKit;

namespace NPAInspectionWriter.iOS.Extensions
{
    public static class UIInterfaceOrientationExtensions
    {
        public static Orientation GetOrientation( this UIInterfaceOrientation interfaceOrientation )
        {
            switch( interfaceOrientation )
            {
                case UIInterfaceOrientation.LandscapeLeft:
                    return Orientation.Landscape & Orientation.LandscapeLeft;
                case UIInterfaceOrientation.LandscapeRight:
                    return Orientation.Landscape & Orientation.LandscapeRight;
                case UIInterfaceOrientation.Portrait:
                    return Orientation.Portrait & Orientation.PortraitUp;
                case UIInterfaceOrientation.PortraitUpsideDown:
                    return Orientation.Portrait & Orientation.PortraitDown;
                default:
                    return Orientation.None;
            }

        }
    }
}
