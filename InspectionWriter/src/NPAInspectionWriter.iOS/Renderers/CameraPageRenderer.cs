using System;
using System.Diagnostics;
using Acr.UserDialogs;
using AVFoundation;
using CoreGraphics;
using Foundation;
using NPAInspectionWriter.Controls;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using NPAInspectionWriter.ViewModels;
using NPAInspectionWriter.Models;
using System.IO;

[assembly: ExportRenderer(typeof(CameraView), typeof(CameraPageRenderer))]
namespace NPAInspectionWriter.iOS.Renderers
{
    public class CameraPageRenderer : ViewRenderer
	{
        public static InspectionTabbedPageViewModel vm;
		AVCaptureSession captureSession;
		AVCaptureDeviceInput captureDeviceInput;
        public AVCaptureVideoPreviewLayer videoPreviewLayer;
		AVCaptureStillImageOutput stillImageOutput;
		UIView liveCameraStream;
		UIButton takePhotoButton;
        UIView confirmView;
        UIPanGestureRecognizer panGesture;
        UIView photoButtonStatus;
        UILabel count;
		nfloat dx = 0;
		nfloat dy = 0;

		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);
			if (Control == null)
			{
				if (e.OldElement != null || Element == null)
				{
					return;
				}
				try
				{
                    vm = (Element).BindingContext as InspectionTabbedPageViewModel;
					var notificationCenter = NSNotificationCenter.DefaultCenter;
					notificationCenter.AddObserver(UIApplication.DidChangeStatusBarOrientationNotification, DeviceOrientationDidChange);

					UIDevice.CurrentDevice.BeginGeneratingDeviceOrientationNotifications();

					var view = new UIView();
                    view.Frame = new CGRect(0, 0, (int)UIScreen.MainScreen.Bounds.Height, (int)UIScreen.MainScreen.Bounds.Width);
					SetNativeControl(view);
					SetupUserInterface();
					SetupEventHandlers();
					SetupLiveCameraStream();
					AuthorizeCameraUse();
				}
				catch (Exception ex)
				{
					Debug.WriteLine(@"ERROR: ", ex.Message);
				}
			}
		}

		/// <summary>
		/// Devices the orientation did change.
		/// </summary>
		/// <param name="notification">Notification.</param>
		public void DeviceOrientationDidChange(NSNotification notification)
		{
            if (UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeLeft)
                videoPreviewLayer.Orientation = AVCaptureVideoOrientation.LandscapeRight;
            else
                videoPreviewLayer.Orientation = AVCaptureVideoOrientation.LandscapeLeft;
		}

		void SetupUserInterface()
		{
            var bottomButtonY = Control.Bounds.Height - 175;
			var buttonWidth = 150;
			var buttonHeight = 100;

			liveCameraStream = new UIView()
			{
                Frame = new CGRect(0f, 0f, Control.Bounds.Width, Control.Bounds.Height),
			};

			takePhotoButton = UIButton.FromType(UIButtonType.Custom);
            takePhotoButton.Frame = new CGRect(25, 15, buttonWidth, buttonHeight);
			takePhotoButton.SetImage(UIImage.FromFile("Icons/cambutton.png"), UIControlState.Normal);

            photoButtonStatus = new UIView();
            photoButtonStatus.Layer.BorderWidth = 1.5f;
            photoButtonStatus.Layer.BorderColor = UIColor.Green.CGColor;
            photoButtonStatus.Layer.MasksToBounds = true;
            photoButtonStatus.ClipsToBounds = true;
            photoButtonStatus.Frame = new CGRect(25, bottomButtonY - 75, buttonWidth + 50, buttonHeight + 30);
            photoButtonStatus.Add(takePhotoButton);
            photoButtonStatus.BringSubviewToFront(takePhotoButton);

			panGesture = new UIPanGestureRecognizer(() =>
			{
				if ((panGesture.State == UIGestureRecognizerState.Began || panGesture.State == UIGestureRecognizerState.Changed) && (panGesture.NumberOfTouches == 1))
				{

					var p0 = panGesture.LocationInView(Control);

					if (dx == 0)
                        dx = p0.X - photoButtonStatus.Center.X;

					if (dy == 0)
						dy = p0.Y - photoButtonStatus.Center.Y;

					var p1 = new CGPoint(p0.X - dx, p0.Y - dy);

					photoButtonStatus.Center = p1;
				}
				else if (panGesture.State == UIGestureRecognizerState.Ended)
				{
					dx = 0;
					dy = 0;
				}
			});

            photoButtonStatus.AddGestureRecognizer(panGesture);

            confirmView = new UIView() { BackgroundColor = UIColor.Gray, Hidden = true, Frame = Control.Bounds };

            var headerView = new UIView(){BackgroundColor=UIColor.DarkGray};
            var closeButton = new UIButton(){TintColor = UIColor.Blue};
            var picsCount = AsyncHelpers.RunSync(async () => await vm.GetCurrentCount()).ToString();
            count = new UILabel() { Text = picsCount, TintColor=UIColor.White, TextColor=UIColor.White};
            count.Frame = new CGRect(Control.Bounds.Width - 20, 20, 10, 20);
            closeButton.Frame = new CGRect(20, 20, 100, 20);
            headerView.Frame = new CGRect(0, 0, Control.Bounds.Width, 45);
            closeButton.SetTitle("Done", UIControlState.Normal);
            closeButton.TouchUpInside += (sender, e) => {
                vm.Navigation.PopModalAsync();
            };
            headerView.Add(closeButton);
            headerView.Add(count);

			Control.Add(liveCameraStream);
            Control.Add(photoButtonStatus);
            Control.Add(confirmView);
            Control.Add(headerView);
		}

		void SetupEventHandlers()
		{
			takePhotoButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				CapturePhoto();
			};
		}

		async void CapturePhoto()
		{
			var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
            videoConnection.VideoOrientation = UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeRight ? AVCaptureVideoOrientation.LandscapeLeft : AVCaptureVideoOrientation.LandscapeRight;
			
            var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);
			var jpegImage = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);

            photoButtonStatus.Hidden = true;
            liveCameraStream.Hidden = true;
            confirmView.Hidden = false;

			var photo = new UIImage(jpegImage);
            var img = new UIImageView(photo) { Frame = new CGRect(50, 84, 900, 600) };

            var acceptButton = new UIButton();
			acceptButton.SetTitle("Accept", UIControlState.Normal);
			acceptButton.Frame = new CGRect(100, Control.Bounds.Height - 70, 100, 50);
			acceptButton.TouchUpInside += async (sender, e) =>
			{
				UserDialogs.Instance.ShowLoading("Saving Image...");
                var imgGuid = Guid.NewGuid();
				var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var directoryname = Path.Combine(documents, vm.CurrentInspection.InspectionId.ToString());
                Directory.CreateDirectory(directoryname);
                string fileName = Path.Combine(directoryname, imgGuid.ToString() + ".png");
				using (NSData imageData = photo.AsPNG())
				using (var client = new InspectionWriterClient())
				{
					//Create Byte Array
                    Byte[] myByteArray = new Byte[imageData.Length];
					System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));
					
                    // Save image in local storage
                    NSError err = null;
                    if (imageData.Save(fileName, false, out err))
					{
						Console.WriteLine("saved as " + fileName);
					}
					else
					{
						Console.WriteLine("NOT saved as " + fileName + " because" + err.LocalizedDescription);
					}

                    // Create actual picture object
					var pic = new Picture
					{
                        ImageData = myByteArray,
						LocalImage = true,
                        Id = Guid.NewGuid(),
                        IsLandscape = true,
                        LocalPath = fileName,
                        InspectionId = vm.CurrentInspection.InspectionId,
                        VehicleId = vm.CurrentInspection.VehicleId
                    };

                    var newCount = await vm.SaveImage(pic);

                    //Update Count/overlay
                    if (newCount > 19)
                        photoButtonStatus.Layer.BorderColor = UIColor.Red.CGColor;
                    else if (newCount > 14)
                        photoButtonStatus.Layer.BorderColor = UIColor.Yellow.CGColor;

                    count.Text = newCount.ToString();

				}
                photoButtonStatus.Hidden = false;
				liveCameraStream.Hidden = false;
				confirmView.Hidden = true;
                confirmView.WillRemoveSubview(img);
				UserDialogs.Instance.HideLoading();
			};

			var retakeButton = new UIButton();
			retakeButton.SetTitle("Retake", UIControlState.Normal);
            retakeButton.Frame = new CGRect(Control.Bounds.Width - 200, Control.Bounds.Height - 70, 100, 50);
			retakeButton.TouchUpInside += (sender, e) =>
			{
                photoButtonStatus.Hidden = false;
				liveCameraStream.Hidden = false;
                confirmView.Hidden = true;
			};

            confirmView.Add(img);
            confirmView.Add(acceptButton);
            confirmView.Add(retakeButton);
		}
        /*
		void SetUpConfirmView(UIImage photo)
		{
			confirmView = new UIView();
			confirmView.Tag = 20;
			var acceptButton = new UIButton();
			acceptButton.SetTitle("Accept", UIControlState.Normal);
			acceptButton.Frame = new CGRect(100, Control.Bounds.Bottom - 70, 100, 50);
			acceptButton.TouchUpInside += async (sender, e) =>
			{
				UserDialogs.Instance.ShowLoading("Saving Image...");
				using (NSData imageData = photo.AsPNG())
				using (var client = new InspectionWriterClient())
				{

					Byte[] myByteArray = new Byte[imageData.Length];
					System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));
					
				}
				captureSession.StartRunning();
				SetNativeControl(tempControl);
				Control.Add(takePhotoButton);
				UserDialogs.Instance.HideLoading();
			};

			var retakeButton = new UIButton();
			retakeButton.SetTitle("Retake", UIControlState.Normal);
			retakeButton.Frame = new CGRect(Control.Bounds.Width - 200, Control.Bounds.Bottom - 70, 100, 50);
			retakeButton.TouchUpInside += (sender, e) =>
			{
				captureSession.StartRunning();
				SetNativeControl(tempControl);
				Control.Add(takePhotoButton);
			};

			confirmView.Add(acceptButton);
			confirmView.Add(retakeButton);
		}
*/

		AVCaptureDevice GetCameraForOrientation(AVCaptureDevicePosition orientation)
		{
			var devices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);

			foreach (var device in devices)
			{
				if (device.Position == orientation)
				{
					return device;
				}
			}
			return null;
		}

		void SetupLiveCameraStream()
		{
			captureSession = new AVCaptureSession();

			var viewLayer = liveCameraStream.Layer;
			videoPreviewLayer = new AVCaptureVideoPreviewLayer(captureSession)
			{
				Frame = liveCameraStream.Bounds
			};

            videoPreviewLayer.Orientation = AVCaptureVideoOrientation.LandscapeLeft;

            videoPreviewLayer.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;
			liveCameraStream.Layer.AddSublayer(videoPreviewLayer);

            var captureDevice = AVCaptureDevice.GetDefaultDevice(AVMediaTypes.Video);
			ConfigureCameraForDevice(captureDevice);
			captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);

			var dictionary = new NSMutableDictionary();
            dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
			stillImageOutput = new AVCaptureStillImageOutput()
			{
				OutputSettings = new NSDictionary()
			};


            captureSession.AddOutput(stillImageOutput);
			captureSession.AddInput(captureDeviceInput);
			captureSession.StartRunning();
		}

		void ConfigureCameraForDevice(AVCaptureDevice device)
		{
			var error = new NSError();
			if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
			{
				device.LockForConfiguration(out error);
				device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
				device.UnlockForConfiguration();
			}
			else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
			{
				device.LockForConfiguration(out error);
				device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
				device.UnlockForConfiguration();
			}
			else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
			{
				device.LockForConfiguration(out error);
				device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
				device.UnlockForConfiguration();
			}
		}

		async void AuthorizeCameraUse()
		{
			var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
			if (authorizationStatus != AVAuthorizationStatus.Authorized)
			{
				await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
			}
		}
	}
}
