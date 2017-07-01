using System;
using Foundation;
using NPAInspectionWriter.iOS.ImageGalleryCollection;
using UIKit;

namespace NPAInspectionWriter.iOS
{
    public class ImageGalleryCollectionDelegate : UICollectionViewDelegateFlowLayout
    {
		public ImageCollectionView CollectionView { get; set; }
        private UIViewController navController { get; set; }

        public ImageGalleryCollectionDelegate(ImageCollectionView collectionView, UIViewController nav)
        {
			// Initialize
			CollectionView = collectionView;
            navController = nav;
        }

		public override bool ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
		{
			// Always allow for highlighting
			return true;
		}

		public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
		{
			// Get cell and change to green background
			var cell = collectionView.CellForItem(indexPath);
			cell.ContentView.BackgroundColor = UIColor.FromRGB(183, 208, 57);
		}

		public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
		{
			// Get cell and return to blue background
			var cell = collectionView.CellForItem(indexPath);
			cell.ContentView.BackgroundColor = UIColor.FromRGB(164, 205, 255);
		}

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            //base.ItemSelected(collectionView, indexPath);
            var cell = collectionView.CellForItem(indexPath);
            var myCell = cell as ImageViewCell;
            navController?.PresentViewController(new ImagePreviewController(navController, myCell.ImageData), false,null);

        }
    }
}
