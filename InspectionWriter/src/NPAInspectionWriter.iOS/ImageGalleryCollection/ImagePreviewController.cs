using System;
using Foundation;
using UIKit;

namespace NPAInspectionWriter.iOS.ImageGalleryCollection
{
    public partial class ImagePreviewController : UIViewController
    {
        public UIViewController ViewController
        {
            get;
            set;
        }
        public UIImage ImageUrl { get; private set; }

        public ImagePreviewController(UIViewController viewController, UIImage imageUrl) : base("ImagePreviewController", null)
        {
            ViewController = viewController;
            ImageUrl = imageUrl;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            //var url = new NSUrl(ImageUrl);
            PreviewImage.Image = ImageUrl;// UIImage.LoadFromData(NSData.FromUrl(url));
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void CloseModel(UIButton sender)
        {
            //var navController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            ViewController.DismissViewController(false, null);
        }
    }
}

