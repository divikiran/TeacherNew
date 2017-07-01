// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace NPAInspectionWriter.iOS
{
    [Register ("ImageViewCell")]
    partial class ImageViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DeleteImageBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageItem { get; set; }

        [Action ("DeleteImage:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void DeleteImage (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (DeleteImageBtn != null) {
                DeleteImageBtn.Dispose ();
                DeleteImageBtn = null;
            }

            if (ImageItem != null) {
                ImageItem.Dispose ();
                ImageItem = null;
            }
        }
    }
}