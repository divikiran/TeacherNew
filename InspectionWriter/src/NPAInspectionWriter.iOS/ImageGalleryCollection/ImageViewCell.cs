using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using NPAInspectionWriter.Helpers;
using UIKit;

namespace NPAInspectionWriter.iOS
{
    public partial class ImageViewCell : UICollectionViewCell
    {
        public static readonly NSString Key = new NSString("ImageViewCell");
        public static readonly UINib Nib;

        string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        string _imageUrl;
        public string ImageUrl
        {
            get
            {
                return _imageUrl;
            }

            set
            {
                _imageUrl = value;
				var url = new NSUrl(this._imageUrl);

				//var data = Task.Run(async () =>
				//{
     //               var imageContent = await LoadImage(_imageUrl);
     //               ImageItem.Image = imageContent; //UIImage.LoadFromData(NSData.FromUrl(url));
					//ImageItem.SizeToFit();
                //});

				//ImageItem.Image = UIImage.LoadFromData(NSData.FromUrl(url));
				//ImageItem.SizeToFit();

                //ImageItem = new UIImageView(UIImage.FromBundle("Icons/barcode.png"));
            }
        }
        UIImage imageData;

        public UIImage ImageData
        {
            get
            {
                return imageData;
            }

            set
            {
                imageData = value;
                ImageItem.Image = imageData;
            }
        }

        static ImageViewCell()
        {
            Nib = UINib.FromName("ImageViewCell", NSBundle.MainBundle);
        }

        protected ImageViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
			
        }

        partial void DeleteImage(UIButton sender)
        {
            UIAlertController _error = UIAlertController.Create("Todo", "Are you sure, you want to delete this image", UIAlertControllerStyle.Alert);
            _error.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, DeleteImageAction(sender)));
            var navigation = UIApplication.SharedApplication.KeyWindow.RootViewController;
            navigation.PresentViewController(_error, false, null);
        }

        private Action<UIAlertAction> DeleteImageAction(UIButton sender)
        {
            throw new NotImplementedException();

        }
    }
}