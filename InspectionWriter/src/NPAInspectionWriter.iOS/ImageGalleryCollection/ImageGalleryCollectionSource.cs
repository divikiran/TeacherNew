using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using Acr.UserDialogs;
using NPAInspectionWriter.Models;
using System.Linq;

namespace NPAInspectionWriter.iOS
{
    public class ImageGalleryCollectionSource : UICollectionViewDataSource
    {

        public ImageCollectionView CollectionView { get; set; }

        public IEnumerable<InspectionImage> InspectionImages { get; private set; }

        public static readonly NSString Identifier = new NSString("Imagecell1");

        public ImageGalleryCollectionSource(ImageCollectionView collectionView)
        {
            //UserDialogs.Instance.ShowLoading("Loading Images...");

            // Initialize
            CollectionView = collectionView;

            CollectionView.RegisterClassForCell(typeof(ImageViewCell), Identifier);
            //CollectionView.RegisterNibForCell(ImageCollectionCell.Nib, ImageCollectionCell.Key);

            CollectionView.RegisterNibForCell(UINib.FromName("ImageViewCell", NSBundle.MainBundle), Identifier);

            var longPressGesture = new UILongPressGestureRecognizer(gesture =>
            {

                // Take action based on state
                switch (gesture.State)
                {
                    case UIGestureRecognizerState.Began:
                        var selectedIndexPath = CollectionView.IndexPathForItemAtPoint(gesture.LocationInView(CollectionView));
                        if (selectedIndexPath != null)
                            CollectionView.BeginInteractiveMovementForItem(selectedIndexPath);
                        break;
                    case UIGestureRecognizerState.Changed:
                        CollectionView.UpdateInteractiveMovement(gesture.LocationInView(CollectionView));
                        break;
                    case UIGestureRecognizerState.Ended:
                        CollectionView.EndInteractiveMovement();
                        break;
                    default:
                        CollectionView.CancelInteractiveMovement();
                        break;
                }
            });

            // Add the custom recognizer to the collection view
            CollectionView.AddGestureRecognizer(longPressGesture);

            InspectionImages = AppRepository.Instance.InspectionImages;

        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            // We only have one section
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            // Return the number of items
            var inspectionImages = AppRepository.Instance.InspectionImages;
            if (inspectionImages != null)
            {
                var count = new List<InspectionImage>(inspectionImages).Count;
                return count;
            }
            return 0;


        }
        public string BaseImageUrl
        {
            get;
            set;
        }
        public static List<UIImage> InspectionUIImages { get; internal set; }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // Get a reusable cell and set it's title from the item
            var cell = collectionView.DequeueReusableCell(Identifier, indexPath) as ImageViewCell;
            var inspectionImages = AppRepository.Instance.InspectionImages;

            if (inspectionImages != null && inspectionImages.Count() >= indexPath.Row)
            {
                var imageData = InspectionUIImages[indexPath.Row];
                cell.ImageData = imageData;
            }
            return cell;
        }



        public override bool CanMoveItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            // We can always move items
            return true;
        }

        public override void MoveItem(UICollectionView collectionView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
        {
            var uiImages = ImageGalleryCollectionSource.InspectionUIImages[(int)sourceIndexPath.Item];
            ImageGalleryCollectionSource.InspectionUIImages.RemoveAt((int)sourceIndexPath.Item);
            ImageGalleryCollectionSource.InspectionUIImages.Insert((int)destinationIndexPath.Item, uiImages);
        }
    }
}
