using System;
using System.Threading.Tasks;
using CoreGraphics;
using UIKit;

namespace NPAInspectionWriter.iOS.Services
{
    public delegate void SaveImage ( UIImage image );

    public partial class AcceptPreviewImage : UIViewController
    {
        //bool isDone = false;
        //bool result = false;
        UIImage _image;
        SaveImage _saveImage { get; }

        public AcceptPreviewImage( UIImage image, SaveImage saveImage ) : base( "AcceptPreviewImage", null )
        {
            _saveImage = saveImage;
            _image = image;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
            imagePreview.Image = _image;
            // NOTE: Height & Width swapped because we are locking this preview to Landscape
            var height = (nfloat)Device.Display.Current.Width / (nfloat)Device.Display.Current.Scale;
            var width = ( nfloat )Device.Display.Current.Height / ( nfloat )Device.Display.Current.Scale;
            bottomToolbar.Frame = new CGRect( 0, height - bottomToolbar.Frame.Height, width, bottomToolbar.Frame.Height );
            imagePreview.Frame = new CGRect( imagePreview.Frame.X, imagePreview.Frame.Y, width, height );
        }

        partial void AcceptBtn_Activated( UIBarButtonItem sender )
        {
            _saveImage?.Invoke( imagePreview.Image );
            DismissViewController( true, null );
        }

        partial void RejectBtn_Activated( UIBarButtonItem sender )
        {
            DismissViewController( true, null );
        }

        partial void RotateBtn_TouchUpInside( UIButton sender )
        {
            //imagePreview.Transform = CGAffineTransform.MakeRotation( ( nfloat )Math.PI * 2f );
        }

        //public async Task<bool> IsImageAcceptedAsync()
        //{
        //    await Task.Run( () =>
        //    {
        //        while( !isDone ) { }
        //        acceptBtn.Enabled = rejectBtn.Enabled = rotateBtn.Enabled = false;
        //    } );
        //    return result;
        //}

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.LandscapeRight;
        }
    }
}
