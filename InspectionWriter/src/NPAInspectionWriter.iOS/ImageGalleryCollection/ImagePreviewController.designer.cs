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

namespace NPAInspectionWriter.iOS.ImageGalleryCollection
{
    [Register ("ImagePreviewController")]
    partial class ImagePreviewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView PreviewImage { get; set; }

        [Action ("CloseModel:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CloseModel (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (PreviewImage != null) {
                PreviewImage.Dispose ();
                PreviewImage = null;
            }
        }
    }
}