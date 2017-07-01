using Foundation;
using System;
using UIKit;

namespace NPAInspectionWriter.iOS
{
    public partial class ImageCollectionView : UICollectionView
    {
		#region Computed Properties
		public ImageGalleryCollectionSource Source
		{
			get
			{
				return (ImageGalleryCollectionSource)DataSource;
			}
		}
		#endregion
		public ImageCollectionView (IntPtr handle) : base (handle)
        {
			
        }

		public override void AwakeFromNib()
		{
			base.AwakeFromNib();

			// Initialize
			DataSource = new ImageGalleryCollectionSource(this);
            var navigation = UIApplication.SharedApplication.KeyWindow.RootViewController;
            Delegate = new ImageGalleryCollectionDelegate(this,navigation);
		}
    }
}