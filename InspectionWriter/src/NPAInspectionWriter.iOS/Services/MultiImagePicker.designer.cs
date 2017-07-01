// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace NPAInspectionWriter.iOS.Services
{
    [Register ("MultiImagePicker")]
    partial class MultiImagePicker
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CameraUnavailableLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton doneButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel imageCountLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton takePhotoBtn { get; set; }

        [Action ("DoneButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void DoneButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("TakePhotoBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TakePhotoBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CameraUnavailableLabel != null) {
                CameraUnavailableLabel.Dispose ();
                CameraUnavailableLabel = null;
            }

            if (doneButton != null) {
                doneButton.Dispose ();
                doneButton = null;
            }

            if (imageCountLabel != null) {
                imageCountLabel.Dispose ();
                imageCountLabel = null;
            }

            if (takePhotoBtn != null) {
                takePhotoBtn.Dispose ();
                takePhotoBtn = null;
            }
        }
    }
}