using AVFoundation;
using Foundation;

namespace NPAInspectionWriter.iOS.Extensions
{
    public static class AVCaptureDeviceExtensions
    {
        public static bool LockForConfiguration( this AVCaptureDevice device )
        {
            NSError error = null;
            return device.LockForConfiguration( out error );
        }

        public static bool TrySetFocusMode( this AVCaptureDevice device, AVCaptureFocusMode mode )
        {
            bool isSupported = device.IsFocusModeSupported( mode );
            if( isSupported )
                device.FocusMode = mode;

            return isSupported;
        }

        public static bool TrySetExposureMode( this AVCaptureDevice device, AVCaptureExposureMode mode )
        {
            bool isSupported = device.IsExposureModeSupported( mode );
            if( isSupported )
                device.ExposureMode = mode;

            return isSupported;
        }
    }
}
