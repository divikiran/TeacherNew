//using System;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using CoreGraphics;
//using Foundation;
//using NPAInspectionWriter.iOS.Extensions;
//using NPAInspectionWriter.Models;
//using UIKit;

//namespace NPAInspectionWriter.iOS
//{
//    public partial class ImageGalleryCell : UICollectionViewCell
//    {
//        public static readonly NSString Key = new NSString("ImageGalleryCell");
//        public static readonly UINib Nib;
//        protected UIImageView img;
//        protected UILabel lbl;
//        protected UIActivityIndicatorView aiView;
//        protected bool isBusy = true;
//        public static readonly int CellWidth = 220;
//        public static readonly int CellHeight = 165;
//        public static readonly int CellPadding = 0;
//        public int position { get; set; }
//        public static ICommand Command { get; set; }
//        protected Picture picture { get; set; }

//        static ImageGalleryCell()
//        {
//            Nib = UINib.FromName("ImageGalleryCell", NSBundle.MainBundle);
//        }

//        protected ImageGalleryCell(IntPtr handle) : base(handle)
//        {
//            // Note: this .ctor should not contain any initialization logic.
//        }

//        [Export("initWithFrame:")]
//        public ImageGalleryCell(CGRect frame) : base(frame)
//        {
//            //BackgroundView = new UIView { BackgroundColor = UIColor.Black };
//            //SelectedBackgroundView = new UIView { BackgroundColor = UIColor.Black };
//            ContentView.Layer.BorderColor = UIColor.White.CGColor;
//            ContentView.Layer.BorderWidth = 0f;
//            ContentView.BackgroundColor = UIColor.White;
//            ContentView.Transform = CGAffineTransform.MakeScale(1f, 1f);

//            frame = new CGRect(0, 0, CellWidth, CellHeight - 20);
//            img = new UIImageView(frame);
//            frame = new CGRect(0, CellHeight - 20, CellWidth, 20);
//            lbl = new UILabel(frame);
//            lbl.TextAlignment = UITextAlignment.Center;

//            ContentView.AddSubview(img);
//            ContentView.AddSubview(lbl);

//            ShowSpinner();

//            // ContentView.SizeToFit(); // this doesn't seem to work
//            ContentView.Frame = new CGRect(ContentView.Frame.X, ContentView.Frame.Y, CellWidth, CellHeight);
//            AddGestureRecognizer( new UITapGestureRecognizer( HandleTappedEvent ) );
//        }

//        //public override void PrepareForReuse()
//        //{
//        //    base.PrepareForReuse();

//        //    ////ContentView.ClearsContextBeforeDrawing = true;
//        //    ////ContentView.RemoveFromSuperview();

//        //    //ContentView.BackgroundColor = UIColor.White;
//        //    //CGRect frame = new CGRect(0, 0, CellWidth, CellHeight - 20);
//        //    //img = new UIImageView(frame);

//        //    ShowSpinner();
//        //}

//        private void HandleTappedEvent()
//        {
//            if( Command != null && Command.CanExecute( picture ) )
//            {
//                Command?.Execute( picture );
//            }
//        }

//        public async void UpdateCell(NPAInspectionWriter.Models.Picture image, string ImageBaseUrl)
//        {
//            try
//            {
//                position = image.Position;
//                picture = image;
//                if (img.Image == null || lbl.Text != image.Position.ToString())
//                {
//                    //aiView.StartAnimating();
//                    // download the image only once
//                    if( !string.IsNullOrWhiteSpace( image.BaseUrl ) )
//                    {
//                        img.Image = await ImageFromUrl(image, ImageBaseUrl);
//                        HideSpinner();
//                    }
//                    else
//                    {
//                        img.Image = ImageFromData(image);
//                        HideSpinner();
//                    }
//                }
//                //var size = new NSString(value).StringSize(lbl.Font, 9999, UILineBreakMode.CharacterWrap);
//                lbl.Text = image.Position.ToString();
//            }
//            catch (Exception e)
//            {
//                // what to do???
//                NPAInspectionWriter.Logging.StaticLogger.ExceptionLogger( e );
//            }
//            HideSpinner();
//        }

//        internal void UpdateCellText(string position)
//        {
//            lbl.Text = position;
//            HideSpinner();
//        }

//        private void HideSpinner()
//        {
//            aiView.StopAnimating();
//            aiView.RemoveFromSuperview();
//        }

//        private void ShowSpinner()
//        {
//            aiView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
//            aiView.Frame = new CGRect(
//                (CellWidth / 2) - (aiView.Frame.Width / 2),
//                (CellHeight / 2) - aiView.Frame.Height - 20,
//                aiView.Frame.Width,
//                aiView.Frame.Height);
//            aiView.AutoresizingMask = UIViewAutoresizing.All;

//            ContentView.AddSubview(aiView);
//            aiView.StartAnimating();
//        }

//        private async Task<UIImage> ImageFromUrl( NPAInspectionWriter.Models.Picture image, string ImageBaseUrl)
//        {
//            bool saveLocally = false;
//            var imagePath = Path.Combine( PlatformSettings.Current.ImageCacheFolderPath, $"{image.InspectionId}", $"{image.Id}.jpeg" );

//            // Check to see if local version exists
//            if( !File.Exists( imagePath ) )
//            {
//                // Download image if it does not exist locally
//                var hc = new HttpClient();
//                // get the thumbnail only... this time
//                string uri = string.Format( ImageBaseUrl, image.Id ) + string.Format("&width={0}&height={1}", CellWidth, CellHeight);

//                Task<byte[]> contentsTask = hc.GetByteArrayAsync( uri );
//                image.ImageData = await contentsTask;
//                saveLocally = true;
//            }
//            else
//            {
//                // Load Image From Locally cached version
//                image.ImageData = NSData.FromFile(imagePath).ToArray();
//            }

//            UIImage tmpImage = null;

//            var data = image.ImageData.ToArray();

//            if (data != null)
//            {
//                tmpImage = UIImage.LoadFromData( NSData.FromArray( data ) );
//            }

//            HideSpinner();

//            if( saveLocally && tmpImage != null )
//            {
//                tmpImage.Save( imagePath );
//            }

//            return tmpImage;
//        }
//        private UIImage ImageFromData( Picture image )
//        {
//            HideSpinner();
//            return UIImage.FromFile( image.LocalPath.EnsureProperApplicationRoot() );
//        }

//        private void SaveImage( UIImage image, string path )
//        {
//            var dir = Path.GetDirectoryName(path);
//            if (!Directory.Exists(dir))
//            {
//                Directory.CreateDirectory(dir);
//            }
//            var imageData = image.AsPNG();
//            NSError err = new NSError();

//            if( !imageData.Save( path, false, out err ) )
//            {
//                NPAInspectionWriter.Logging.StaticLogger.WarningLogger( $"Image not saved to: {path}.\n{err.LocalizedDescription}\n\n{err.LocalizedFailureReason}" );
//            }
//        }
//    }
//}
