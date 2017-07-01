//using System;
//using System.Collections.ObjectModel;
//using System.Collections.Generic;
//using System.Text;
//using Foundation;
//using UIKit;
//using Prism.Events;

//using CoreGraphics;
//using Xamarin.Forms;
//using NPAInspectionWriter;
//using System.Windows.Input;
//using NPAInspectionWriter.Models;

//[assembly: ExportRenderer(typeof(UICollectionViewSource), typeof(NPAInspectionWriter.iOS.ImageCellSource))]
//namespace NPAInspectionWriter.iOS
//{
//    public class ImageCellSource : UICollectionViewDataSource
//    {

//        private ObservableCollection<Picture> Images;
//        private string ImageBaseUrl;
//        private bool Editable;
//        public ICommand SaveCommand { get; set; }
//        //public List<int> Position { get; set; } = new List<int>();

//        public ImageCellSource(ObservableCollection<NPAInspectionWriter.Models.Picture> images, string imageBaseUrl)
//        {
//            Images = images;
//            ImageBaseUrl = imageBaseUrl;
//            Editable = true;
//        }

//        public override UICollectionViewCell GetCell(UICollectionView imagesView, NSIndexPath indexPath)
//        {
//            int position = (int)indexPath.Item;

//            if(position < 1)
//                imagesView.RegisterClassForCell(typeof(ImageGalleryCell), "ImageGalleryCell");

//            UICollectionViewCell cell = (UICollectionViewCell)imagesView.DequeueReusableCell("ImageGalleryCell", indexPath);

//            if(cell == null)
//            {
//                ((UICollectionViewCell)cell).Init();
//            }

//            if(((ImageGalleryCell)cell).position != (position + 1))
//            {
//                //((ImageGalleryCell)cell).UpdateCellText((position + 1).ToString());
//                //((ImageGalleryCell)cell).ShowSpinner();
//                ((ImageGalleryCell)cell).UpdateCell(Images[position], ImageBaseUrl);
//                //((ImageGalleryCell)cell).HideSpinner();
//                //cell.SizeToFit();
//            }

//            return cell;
//        }

//        public override nint GetItemsCount(UICollectionView imagesView, nint section)
//        {
//            return Images.Count;
//        }

//        public TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
//        {
//            throw new NotImplementedException();
//        }

//        //public override bool CanMoveItem(UICollectionView imagesView, NSIndexPath indexPath)
//        //{
//        //    return Editable;
//        //}

//        public override void MoveItem( UICollectionView imagesView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath )
//        {
//            // Resort the images
//            var item = Images[ ( int )sourceIndexPath.Item ];
//            Images.RemoveAt( ( int )sourceIndexPath.Item );
//            Images.Insert( ( int )destinationIndexPath.Item, item );

//            // Reorder our list of items
//            int position = Math.Min( ( int )sourceIndexPath.Item, ( int )destinationIndexPath.Item );
//            for( int i = position; i < Images.Count; i++ )
//            {
//                Images[ i ].Position = i + 1;
//            }

//            // Update the Cells text
//            foreach( var ip in imagesView.IndexPathsForVisibleItems )
//            {
//                int i = ( int )ip.Item;
//                ( ( ImageGalleryCell )imagesView.CellForItem( ip ) ).UpdateCellText( ( i + 1 ).ToString() );
//            }

//            SaveChanges();
//        }

//        private void SaveChanges()
//        {
//            if( Editable && SaveCommand != null && SaveCommand.CanExecute( Images ) )
//            {
//                SaveCommand.Execute( Images );
//            }
//        }

//    }
//}
