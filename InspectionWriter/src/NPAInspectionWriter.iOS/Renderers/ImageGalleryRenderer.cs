//using System;
//using UIKit;
//using Xamarin.Forms.Platform.iOS;
//using Xamarin.Forms;
//using NPAInspectionWriter;
//using NPAInspectionWriter.ViewModels;
//using NPAInspectionWriter.Controls;
//using CoreGraphics;
//using System.ComponentModel;
//using Prism.Events;
//using System.Windows.Input;
//using NPAInspectionWriter.iOS.Device;
//using Foundation;

//[assembly: ExportRenderer(typeof(ucImageGallery), typeof(NPAInspectionWriter.iOS.ImageGalleryRenderer))]
//namespace NPAInspectionWriter.iOS
//{
//    public class ImageGalleryRenderer : ViewRenderer<ucImageGallery, UICollectionView>
//    {
//        public ImageGalleryRenderer()
//        {
//            //Control.RegisterClassForCell(typeof(ImageGalleryCell), "ImageGalleryCell");
//            MessagingCenter.Subscribe<ImageGalleryPageViewModel, int>(this, "GalleryUpdated", (sender, arg) => { if (arg > 0) { ImagesUpdated(sender); }; });
//        }

//        protected override void OnElementPropertyChanged( object sender, PropertyChangedEventArgs e )
//        {
//            base.OnElementPropertyChanged( sender, e );
//            ImageGalleryCell.Command = Element.ImageTappedCommand;
//        }

//        //public override void ViewDidLoad()
//        //{
//        //    base.ViewDidLoad();
//        //    CollectionView.RegisterClassForCell(typeof(ImageGalleryCell), cellClass);
//        //    CollectionView.BackgroundColor = UIColor.White;
//        //}

//        //protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
//        private void ImagesUpdated(ImageGalleryPageViewModel sender)
//        {

//            var images = sender.GalleryImages;
//            string imageBaseUrl = sender.ImageBaseUrl;

//            if (Control == null)
//            {
//                ResetControl();
//            }

//            if( Control != null )
//            {
//                Control.DataSource = new ImageCellSource( images, imageBaseUrl )
//                {
//                    SaveCommand = Element.ImagesReorderedCommand
//                };

//                Control.BackgroundColor = UIColor.White;
//                Control.ReloadData();

//                if (sender.IsEditable)
//                {
//                    // Create a custom gesture recognizer
//                    var longPressGesture = new UILongPressGestureRecognizer(gesture =>
//                    {
//                        // Take action based on state
//                        switch (gesture.State)
//                        {
//                            case UIGestureRecognizerState.Began:
//                                var selectedIndexPath = Control.IndexPathForItemAtPoint(gesture.LocationInView(this.Control.ViewForBaselineLayout));
//                                if (selectedIndexPath != null)
//                                {
//                                    Control.BeginInteractiveMovementForItem(selectedIndexPath);
//                                }
//                                break;
//                            case UIGestureRecognizerState.Changed:
//                                Control.UpdateInteractiveMovement(gesture.LocationInView(Control.ViewForBaselineLayout));
//                                break;
//                            case UIGestureRecognizerState.Ended:
//                                Control.EndInteractiveMovement();
//                                break;
//                            default:
//                                Control.CancelInteractiveMovement();
//                                break;
//                        }
//                    });

//                    longPressGesture.MinimumPressDuration = 0.25;

//                    // Add the custom recognizer to the collection view
//                    Control.AddGestureRecognizer(longPressGesture);
//                }
//            }
//        }

//        protected override void OnElementChanged(ElementChangedEventArgs<ucImageGallery> e)
//        {
//            base.OnElementChanged(e);
//            if (e.NewElement != null)
//            {
//                ResetControl();
//            }
//        }

//        protected void ResetControl()
//        {
//            var layout = new UICollectionViewFlowLayout();
//            layout.ItemSize = new CGSize(ImageGalleryCell.CellWidth, ImageGalleryCell.CellHeight);
//            layout.ScrollDirection = UICollectionViewScrollDirection.Vertical;

//            int numberOfRows = AppleDevice.CurrentDevice.IsInPortrait() ? 6 : 4;

//            if (Control == null)
//            {
//                try
//                {
//                    // this blows up when returning to a gallery for the second time. might need to change e.NewElement != null to e.OldElement == null or something. will fix later.
//                    SetNativeControl(new UICollectionView(new CGRect(0, 0, ImageGalleryCell.CellWidth * 3, (ImageGalleryCell.CellHeight * numberOfRows) + 20), layout));
//                }
//                catch (Exception ex)
//                {
//                    NPAInspectionWriter.Logging.StaticLogger.WarningLogger("Caught SetNativeControl error: " + ex.Message);
//                }
//            }
//        }
//    }
//}
