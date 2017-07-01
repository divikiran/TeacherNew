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
    [Register ("AcceptPreviewImage")]
    partial class AcceptPreviewImage
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem acceptBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIToolbar bottomToolbar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imagePreview { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem rejectBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton rotateBtn { get; set; }

        [Action ("AcceptBtn_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AcceptBtn_Activated (UIKit.UIBarButtonItem sender);

        [Action ("RejectBtn_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void RejectBtn_Activated (UIKit.UIBarButtonItem sender);

        [Action ("RotateBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void RotateBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (acceptBtn != null) {
                acceptBtn.Dispose ();
                acceptBtn = null;
            }

            if (bottomToolbar != null) {
                bottomToolbar.Dispose ();
                bottomToolbar = null;
            }

            if (imagePreview != null) {
                imagePreview.Dispose ();
                imagePreview = null;
            }

            if (rejectBtn != null) {
                rejectBtn.Dispose ();
                rejectBtn = null;
            }

            if (rotateBtn != null) {
                rotateBtn.Dispose ();
                rotateBtn = null;
            }
        }
    }
}