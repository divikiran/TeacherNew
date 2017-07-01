using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AVFoundation;
using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using CoreVideo;
using Foundation;
using NPAInspectionWriter.iOS.Extensions;
using NPAInspectionWriter.iOS.Extensions;
using NPAInspectionWriter.Logging;
using NPAInspectionWriter.Services;
using ObjCRuntime;
using UIKit;

namespace NPAInspectionWriter.iOS.Services
{
    public enum SetupResult
    {
        Success,
        CameraNotAuthorized,
        SessionConfigurationFailed
    };

    public partial class MultiImagePicker : UIViewController, IAVCapturePhotoCaptureDelegate
    {
        #region Properties

        IDisposable focusModeToken;
        IDisposable lensPositionToken;
        IDisposable exposureModeToken;
        IDisposable exposureDurationToken;
        IDisposable isoToken;
        IDisposable runningToken;
        IDisposable exposureTargetBiasToken;
        IDisposable exposureTargetOffsetToken;
        IDisposable whiteBalanceModeToken;
        IDisposable deviceWhiteBalanceGainsToken;
        NSObject subjectAreaDidChangeToken;
        NSObject runtimeErrorToken;
        NSObject wasInterruptedToken;
        NSObject interruptionEndedToken;

        DispatchQueue sessionQueue;
        //AVCaptureMovieFileOutput movieFileOutput;
        AVCapturePhotoOutput photoOutput;
        public AVCaptureSession Session
        {
            [Export( "session" )]
            get;
            [Export( "setSession:" )]
            set;
        }

        AVCaptureDeviceInput videoDeviceInput;

        public AVCaptureDeviceInput VideoDeviceInput
        {
            [Export( "videoDeviceInput" )]
            get
            {
                return videoDeviceInput;
            }
            [Export( "setVideoDeviceInput:" )]
            set
            {
                WillChangeValue( "videoDeviceInput" );
                videoDeviceInput = value;
                DidChangeValue( "videoDeviceInput" );
            }
        }

        AVCaptureDevice videoDevice;
        AVCaptureDevice VideoDevice
        {
            [Export( "videoDevice" )]
            get
            {
                return videoDevice;
            }
            [Export( "setVideoDevice:" )]
            set
            {
                WillChangeValue( "videoDevice" );
                videoDevice = value;
                DidChangeValue( "videoDevice" );
            }
        }

        AVCaptureVideoPreviewLayer PreviewLayer { get; set; }

        // Utilities
        SetupResult setupResult;
        bool sessionRunning;
        nint backgroundRecordingID;

        // Higher numbers will give the slider more sensitivity at shorter durations
        const float ExposureDurationPower = 5;

        // Limit exposure duration to a useful range
        const float ExposureMinDuration = 1f / 1000;

        // Picker
        bool isConfigured = false;
        bool isDone = false;
        bool didLoad = false;
        List<MediaFile> MediaFiles { get; set; } = new List<MediaFile>();
        public string DefaultStoragePath { get; } = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.Personal ), "Images", DateTime.Now.ToString( "MM-dd-yy" ) );
        public string StoragePath { get; set; }
        private int imageCount = 0;
        public int ImageCount
        {
            get { return imageCount; }
            set
            {
                imageCount = value;
                UpdateCountColor();
                if( imageCountLabel != null )
                    imageCountLabel.Text = $"{value}";
            }
        }
        private int warningCount = 15;
        public int WarningCount
        {
            get { return warningCount; }
            set
            {
                warningCount = value;
                if( value > HighCount )
                    HighCount = value + 1;

                UpdateCountColor();
            }
        }

        private int highCount = 20;
        public int HighCount
        {
            get { return highCount; }
            set
            {
                highCount = value;
                if( value < WarningCount )
                    WarningCount = value - 1;

                UpdateCountColor();
            }
        }
        public int MaxPixelSize { get; set; }

        #endregion

        public MultiImagePicker() : base( "MultiImagePicker", null )
        {
        }

        //public override void DidReceiveMemoryWarning()
        //{
        //    base.DidReceiveMemoryWarning();

        //    // Release any cached data, images, etc that aren't in use.
        //}

        private void UpdateCountColor()
        {
            if( imageCountLabel == null ) return;

            if( ImageCount > HighCount )
                imageCountLabel.TextColor = UIColor.Red;
            else if( ImageCount > WarningCount )
                imageCountLabel.TextColor = UIColor.Yellow;
            else
                imageCountLabel.TextColor = UIColor.Green;
        }

        #region Lifecycle

        //protected override void Dispose( bool disposing )
        //{
        //    isConfigured = false;

        //    if( VideoDeviceInput != null )
        //    {
        //        VideoDeviceInput.Dispose();
        //        VideoDeviceInput = null;
        //    }

        //    if( VideoDevice != null )
        //    {
        //        VideoDevice.Dispose();
        //        VideoDevice = null;
        //    }

        //    if( photoOutput != null )
        //    {
        //        photoOutput.Dispose();
        //        photoOutput = null;
        //    }

        //    if( PreviewLayer != null )
        //    {
        //        PreviewLayer.Dispose();
        //        PreviewLayer = null;
        //    }

        //    if( Session != null )
        //    {
        //        Session.Dispose();
        //        Session = null;
        //    }

        //    base.Dispose( disposing );
        //}

        public override void ViewDidLoad()
        {
            if( didLoad ) return;
            didLoad = true;
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
            takePhotoBtn.Enabled = false;

            // Create the AVCaptureSession
            Session = new AVCaptureSession();

            // Set up preview
            var display = Device.Display.Current;
            PreviewLayer = new AVCaptureVideoPreviewLayer( Session )
            {
                // Width & Height swapped because we are locking the camera to Landscape Only
                Frame = new CGRect( 0, 0, display.Height / display.Scale, display.Width / display.Scale ),
                VideoGravity = AVLayerVideoGravity.ResizeAspectFill,
                Orientation = GetVideoOrientation( InterfaceOrientation ),
            };
            View.Layer.InsertSublayer( PreviewLayer, 0 );

            sessionQueue = new DispatchQueue( "session queue" );
            setupResult = SetupResult.Success;

            // Check video authorization status. Video access is required and audio access is optional.
            // If audio access is denied, audio is not recorded during movie recording.
            CheckDeviceAuthorizationStatus();

            // Setup the capture session.
            // In general it is not safe to mutate an AVCaptureSession or any of its inputs, outputs, or connections from multiple threads at the same time.
            // Why not do all of this on the main queue?
            // Because AVCaptureSession.StartRunning is a blocking call which can take a long time. We dispatch session setup to the sessionQueue
            // so that the main queue isn't blocked, which keeps the UI responsive.
            sessionQueue.DispatchAsync( ConfigureSession );

            imageCountLabel.Text = $"{ImageCount}";
            UpdateCountColor();
        }

        private AVCaptureVideoOrientation GetVideoOrientation( UIInterfaceOrientation interfaceOrientation )
        {
            switch( interfaceOrientation )
            {
                case UIInterfaceOrientation.LandscapeLeft:
                    return AVCaptureVideoOrientation.LandscapeLeft;
                case UIInterfaceOrientation.LandscapeRight:
                    return AVCaptureVideoOrientation.LandscapeRight;
                case UIInterfaceOrientation.PortraitUpsideDown:
                    return AVCaptureVideoOrientation.PortraitUpsideDown;
                case UIInterfaceOrientation.Portrait:
                default:
                    return AVCaptureVideoOrientation.Portrait;
            }
        }

        void CheckDeviceAuthorizationStatus()
        {
            var status = AVCaptureDevice.GetAuthorizationStatus( AVMediaType.Video );
            switch( status )
            {
                // The user has previously granted access to the camera
                case AVAuthorizationStatus.Authorized:
                    CameraUnavailableLabel.Hidden = true;
                    break;

                // The user has not yet been presented with the option to grant video access.
                // We suspend the session queue to delay session running until the access request has completed.
                // Note that audio access will be implicitly requested when we create an AVCaptureDeviceInput for audio during session setup.
                case AVAuthorizationStatus.NotDetermined:
                    sessionQueue.Suspend();
                    AVCaptureDevice.RequestAccessForMediaType( AVMediaType.Video, granted => {
                        if( !granted )
                            setupResult = SetupResult.CameraNotAuthorized;
                        else
                            CameraUnavailableLabel.Hidden = true;

                        sessionQueue.Resume();
                    } );
                    break;

                default:
                    // The user has previously denied access
                    setupResult = SetupResult.CameraNotAuthorized;
                    break;
            }
        }

        public override void ViewWillAppear( bool animated )
        {
            base.ViewWillAppear( animated );

            var appName = NSBundle.MainBundle.ObjectForInfoDictionary( "CFBundleDisplayName" ).ToString();
            sessionQueue.DispatchAsync( () => {
                switch( setupResult )
                {
                    // Only setup observers and start the session running if setup succeeded
                    case SetupResult.Success:
                        AddObservers();
                        Session.StartRunning();
                        sessionRunning = Session.Running;
                        break;

                    case SetupResult.CameraNotAuthorized:
                        DispatchQueue.MainQueue.DispatchAsync( () => {
                            string message = $"{appName} doesn't have permission to use the camera, please change privacy settings";
                            UIAlertController alertController = UIAlertController.Create( appName, message, UIAlertControllerStyle.Alert );
                            UIAlertAction cancelAction = UIAlertAction.Create( "OK", UIAlertActionStyle.Cancel, null );
                            alertController.AddAction( cancelAction );
                            // Provide quick access to Settings
                            UIAlertAction settingsAction = UIAlertAction.Create( "Settings", UIAlertActionStyle.Default, action => {
                                UIApplication.SharedApplication.OpenUrl( NSUrl.FromString( UIApplication.OpenSettingsUrlString ), new UIApplicationOpenUrlOptions(), null );
                            } );
                            alertController.AddAction( settingsAction );
                            PresentViewController( alertController, true, null );
                        } );
                        break;
                    case SetupResult.SessionConfigurationFailed:
                        DispatchQueue.MainQueue.DispatchAsync( () => {
                            string message = "Unable to capture media";
                            UIAlertController alertController = UIAlertController.Create( appName, message, UIAlertControllerStyle.Alert );
                            UIAlertAction cancelAction = UIAlertAction.Create( "OK", UIAlertActionStyle.Cancel, null );
                            alertController.AddAction( cancelAction );
                            PresentViewController( alertController, true, null );
                        } );
                        break;
                }
            } );
        }

        public override void ViewDidDisappear( bool animated )
        {
            CloseSession();
            base.ViewDidDisappear( animated );
        }

        private void CloseSession()
        {
            DispatchQueue.MainQueue.DispatchAsync( () => {
                if( setupResult == SetupResult.Success )
                {
                    Session.StopRunning();
                    RemoveObservers();
                }
            } );
        }

        public override void ViewWillTransitionToSize( CGSize toSize, IUIViewControllerTransitionCoordinator coordinator )
        {
            base.ViewWillTransitionToSize( toSize, coordinator );

            UIDeviceOrientation deviceOrientation = UIDevice.CurrentDevice.Orientation;
            if( deviceOrientation.IsPortrait() || deviceOrientation.IsLandscape() )
            {
                var connection = PreviewLayer.Connection;
                if( connection != null )
                    connection.VideoOrientation = ( AVCaptureVideoOrientation )deviceOrientation;
            }

            var display = Device.Display.Current;
            PreviewLayer.Frame = new CGRect( 0, 0, display.Width / display.Scale, display.Height / display.Scale );
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.LandscapeRight;
        }

        //public override bool ShouldAutorotate()
        //{
        //    // Disable autorotation of the interface when recording is in progress
        //    return movieFileOutput == null || !movieFileOutput.Recording;
        //}

        public override bool PrefersStatusBarHidden()
        {
            return true;
        }

        public async Task<List<MediaFile>> GetMediaFiles()
        {
            await Task.Run( () =>
            {
                while( !isDone )
                {
                }
            } );
            return MediaFiles;
        }

        partial void DoneButton_TouchUpInside( UIButton sender )
        {
            CloseSession();
            isDone = true;
            //this.DismissViewController();
        }

        partial void TakePhotoBtn_TouchUpInside( UIButton sender )
        {
            try
            {

            CapturePhoto( sender );
            }
            catch( Exception e )
            {
                StaticLogger.ExceptionLogger( e );
            }
        }

        #endregion

        #region Session Management

        void ConfigureSession()
        {
            if( setupResult != SetupResult.Success || isConfigured )
                return;

            NSError error = null;
            Session.BeginConfiguration();
            Session.SessionPreset = AVCaptureSession.PresetPhoto;

            // Add video input
            AVCaptureDevice vDevice = GetDeviceFrom( AVMediaType.Video, AVCaptureDevicePosition.Back );
            AVCaptureDeviceInput vDeviceInput = AVCaptureDeviceInput.FromDevice( vDevice, out error );
            if( error != null )
            {
                Console.WriteLine( "Could not create video device input: {0}", error );
                setupResult = SetupResult.SessionConfigurationFailed;
                Session.CommitConfiguration();
                return;
            }


            if( Session.CanAddInput( vDeviceInput ) )
            {
                Session.AddInput( vDeviceInput );
                VideoDeviceInput = vDeviceInput;
                VideoDevice = vDeviceInput.Device;
            }
            else if( Session.Inputs.Count() < 1 )
            {
                Console.WriteLine( "Could not add video device input to the session" );
                setupResult = SetupResult.SessionConfigurationFailed;
                Session.CommitConfiguration();
                return;
            }

            //// Add audio input
            //AVCaptureDevice aDevice = AVCaptureDevice.DefaultDeviceWithMediaType( AVMediaType.Audio );
            //AVCaptureDeviceInput aDeviceInput = AVCaptureDeviceInput.FromDevice( aDevice, out error );
            //if( error != null )
            //    Console.WriteLine( "Could not create audio device input: {0}", error );
            //if( Session.CanAddInput( aDeviceInput ) )
            //    Session.AddInput( aDeviceInput );
            //else
            //    Console.WriteLine( "Could not add audio device input to the session" );

            // Add photo output
            var po = new AVCapturePhotoOutput();
            if( Session.CanAddOutput( po ) )
            {
                Session.AddOutput( po );
                photoOutput = po;
                photoOutput.IsHighResolutionCaptureEnabled = true;
            }
            else if( Session.Outputs.Count() < 1 )
            {
                Console.WriteLine( "Could not add photo output to the session" );
                setupResult = SetupResult.SessionConfigurationFailed;
                Session.CommitConfiguration();
                return;
            }

            // We will not create an AVCaptureMovieFileOutput when configuring the session because the AVCaptureMovieFileOutput does not support movie recording with AVCaptureSessionPresetPhoto
            backgroundRecordingID = -1;

            Session.CommitConfiguration();
            //DispatchQueue.MainQueue.DispatchAsync( ConfigureManualHUD );

            isConfigured = true;
        }

        // Should be called on the main queue
        AVCapturePhotoSettings GetCurrentPhotoSettings()
        {
            bool lensStabilizationEnabled = true;
            bool rawEnabled = false;
            AVCapturePhotoSettings photoSettings = null;

            if( lensStabilizationEnabled && photoOutput.IsLensStabilizationDuringBracketedCaptureSupported )
            {
                AVCaptureBracketedStillImageSettings[] bracketedSettings = null;
                if( VideoDevice.ExposureMode == AVCaptureExposureMode.Custom )
                {
                    bracketedSettings = new AVCaptureBracketedStillImageSettings[] {
                        AVCaptureManualExposureBracketedStillImageSettings.Create(AVCaptureDevice.ExposureDurationCurrent, AVCaptureDevice.ISOCurrent)
                    };
                }
                else
                {
                    bracketedSettings = new AVCaptureBracketedStillImageSettings[]{
                        AVCaptureAutoExposureBracketedStillImageSettings.Create(AVCaptureDevice.ExposureTargetBiasCurrent)
                    };
                }
                if( rawEnabled && photoOutput.AvailableRawPhotoPixelFormatTypes.Length > 0 )
                {
                    photoSettings = AVCapturePhotoBracketSettings.FromRawPixelFormatType( photoOutput.AvailableRawPhotoPixelFormatTypes[ 0 ].UInt32Value, null, bracketedSettings );
                }
                else
                {
                    // TODO: https://bugzilla.xamarin.com/show_bug.cgi?id=44111
                    photoSettings = AVCapturePhotoBracketSettings.FromRawPixelFormatType( 0, new NSDictionary<NSString, NSObject>( AVVideo.CodecKey, new NSNumber( ( int )AVVideoCodec.JPEG ) ), bracketedSettings );
                }

                ( ( AVCapturePhotoBracketSettings )photoSettings ).IsLensStabilizationEnabled = true;
            }
            else
            {
                if( rawEnabled && photoOutput.AvailableRawPhotoPixelFormatTypes.Length > 0 )
                {
                    photoSettings = AVCapturePhotoSettings.FromRawPixelFormatType( photoOutput.AvailableRawPhotoPixelFormatTypes[ 0 ].UInt32Value );
                }
                else
                {
                    photoSettings = AVCapturePhotoSettings.Create();
                }

                // We choose not to use flash when doing manual exposure
                if( VideoDevice.ExposureMode == AVCaptureExposureMode.Custom )
                {
                    photoSettings.FlashMode = AVCaptureFlashMode.Off;
                }
                else
                {
                    photoSettings.FlashMode = photoOutput.SupportedFlashModes.Contains( new NSNumber( ( long )AVCaptureFlashMode.Auto ) ) ? AVCaptureFlashMode.Auto : AVCaptureFlashMode.Off;
                }
            }

            // The first format in the array is the preferred format
            if( photoSettings.AvailablePreviewPhotoPixelFormatTypes.Length > 0 )
                photoSettings.PreviewPhotoFormat = new NSDictionary<NSString, NSObject>( CVPixelBuffer.PixelFormatTypeKey, photoSettings.AvailablePreviewPhotoPixelFormatTypes[ 0 ] );

            if( VideoDevice.ExposureMode == AVCaptureExposureMode.Custom )
                photoSettings.IsAutoStillImageStabilizationEnabled = false;

            photoSettings.IsHighResolutionPhotoEnabled = true;
            return photoSettings;
        }

        [Action( "resumeInterruptedSession:" )]
        void ResumeInterruptedSession( NSObject sender )
        {
            // The session might fail to start running, e.g. if a phone or FaceTime call is still using audio or video.
            // A failure to start the session will be communicated via a session runtime error notification.
            // To avoid repeatedly failing to start the session running, we only try to restart the session in the
            // session runtime error handler if we aren't trying to resume the session running.
            sessionQueue.DispatchAsync( () => {
                Session.StartRunning();
                sessionRunning = Session.Running;
                if( !Session.Running )
                {
                    DispatchQueue.MainQueue.DispatchAsync( () => {
                        var message = "Unable to resume";
                        UIAlertController alertController = UIAlertController.Create( "AVCamManual", message, UIAlertControllerStyle.Alert );
                        UIAlertAction cancelAction = UIAlertAction.Create( "OK", UIAlertActionStyle.Cancel, null );
                        alertController.AddAction( cancelAction );
                        PresentViewController( alertController, true, null );
                    } );
                }
                //else
                //{
                //    DispatchQueue.MainQueue.DispatchAsync( () => ResumeButton.Hidden = true );
                //}
            } );
        }

        [Action( "changeCaptureMode:" )]
        void ChangeCaptureMode( UISegmentedControl captureModeControl )
        {
            //if( captureModeControl.SelectedSegment == ( int )CaptureMode.Photo )
            //{
            //    RecordButton.Enabled = false;

            // Remove the AVCaptureMovieFileOutput from the session because movie recording is not supported with AVCaptureSessionPresetPhoto. Additionally, Live Photo
            // capture is not supported when an AVCaptureMovieFileOutput is connected to the session.
            sessionQueue.DispatchAsync( () => {
                Session.BeginConfiguration();
                //Session.RemoveOutput( movieFileOutput );
                Session.SessionPreset = AVCaptureSession.PresetPhoto;
                Session.CommitConfiguration();

                //movieFileOutput = null;
            } );
            //}
            //else if( captureModeControl.SelectedSegment == ( int )CaptureMode.Movie )
            //{
            //    sessionQueue.DispatchAsync( () => {
            //        var mfo = new AVCaptureMovieFileOutput();
            //        if( Session.CanAddOutput( mfo ) )
            //        {
            //            Session.BeginConfiguration();
            //            Session.AddOutput( mfo );
            //            Session.SessionPreset = AVCaptureSession.PresetHigh;
            //            AVCaptureConnection connection = mfo.ConnectionFromMediaType( AVMediaType.Video );
            //            if( connection.SupportsVideoStabilization )
            //                connection.PreferredVideoStabilizationMode = AVCaptureVideoStabilizationMode.Auto;
            //            Session.CommitConfiguration();
            //            movieFileOutput = mfo;

            //            DispatchQueue.MainQueue.DispatchAsync( () => {
            //                RecordButton.Enabled = true;
            //            } );
            //        }
            //    } );
            //}
        }

        #endregion

        #region Device Configuration

        static AVCaptureDevice GetDeviceFrom( string mediaType, AVCaptureDevicePosition position )
        {
            AVCaptureDevice[] devices = AVCaptureDevice.DevicesWithMediaType( mediaType );
            AVCaptureDevice captureDevice = devices.FirstOrDefault( d => d.Position == position );
            return captureDevice;
        }

        void SetFocusAndMode( AVCaptureFocusMode focusMode, AVCaptureExposureMode exposureMode, CGPoint point, bool monitorSubjectAreaChange )
        {
            sessionQueue.DispatchAsync( () => {
                AVCaptureDevice device = VideoDevice;
                NSError error = null;
                if( device.LockForConfiguration( out error ) )
                {
                    // Setting (Focus|Exposure)PointOfInterest alone does not initiate a (focus/exposure) operation
                    // Set (Focus|Exposure)Mode to apply the new point of interest
                    if( focusMode != AVCaptureFocusMode.Locked && device.FocusPointOfInterestSupported && device.IsFocusModeSupported( focusMode ) )
                    {
                        device.FocusMode = focusMode;
                        device.FocusPointOfInterest = point;
                    }
                    if( exposureMode != AVCaptureExposureMode.Custom && device.ExposurePointOfInterestSupported && device.IsExposureModeSupported( exposureMode ) )
                    {
                        device.ExposureMode = exposureMode;
                        device.ExposurePointOfInterest = point;
                    }
                    device.SubjectAreaChangeMonitoringEnabled = monitorSubjectAreaChange;
                    device.UnlockForConfiguration();
                }
                else
                {
                    Console.WriteLine( $"Could not lock device for configuration: {error}" );
                }
            } );
        }

        void SetWhiteBalanceGains( AVCaptureWhiteBalanceGains gains )
        {
            NSError error = null;

            if( VideoDevice.LockForConfiguration( out error ) )
            {
                AVCaptureWhiteBalanceGains newGains = NormalizeGains( gains ); // Conversion can yield out-of-bound values, cap to limits
                VideoDevice.SetWhiteBalanceModeLockedWithDeviceWhiteBalanceGains( newGains, null );
                VideoDevice.UnlockForConfiguration();
            }
            else
            {
                Console.WriteLine( $"Could not lock device for configuration: {error}" );
            }
        }

        AVCaptureWhiteBalanceGains NormalizeGains( AVCaptureWhiteBalanceGains gains )
        {
            gains.RedGain = Math.Max( 1, gains.RedGain );
            gains.GreenGain = Math.Max( 1, gains.GreenGain );
            gains.BlueGain = Math.Max( 1, gains.BlueGain );

            float maxGain = VideoDevice.MaxWhiteBalanceGain;
            gains.RedGain = Math.Min( maxGain, gains.RedGain );
            gains.GreenGain = Math.Min( maxGain, gains.GreenGain );
            gains.BlueGain = Math.Min( maxGain, gains.BlueGain );

            return gains;
        }

        #endregion

        #region Capturing Photos

        [Action( "capturePhoto:" )]
        void CapturePhoto( NSObject sender )
        {
            // Retrieve the video preview layer's video orientation on the main queue before entering the session queue
            // We do this to ensure UI elements are accessed on the main thread and session configuration is done on the session queue
            var previewLayer = PreviewLayer;
            AVCaptureVideoOrientation videoPreviewLayerVideoOrientation = previewLayer.Connection.VideoOrientation;

            AVCapturePhotoSettings settings = GetCurrentPhotoSettings();
            sessionQueue.DispatchAsync( () => {
                // Update the orientation on the photo output video connection before capturing
                AVCaptureConnection photoOutputConnection = photoOutput.ConnectionFromMediaType( AVMediaType.Video );
                photoOutputConnection.VideoOrientation = videoPreviewLayerVideoOrientation;
                photoOutput.CapturePhoto( settings, this );
            } );
        }

        private AVCaptureVideoOrientation GetStillOrientation()
        {
            switch( InterfaceOrientation )
            {
                case UIInterfaceOrientation.LandscapeLeft:
                case UIInterfaceOrientation.PortraitUpsideDown:
                    return AVCaptureVideoOrientation.LandscapeLeft;
                default:
                    return AVCaptureVideoOrientation.LandscapeRight;
            }
        }

        [Export( "captureOutput:willCapturePhotoForResolvedSettings:" )]
        void WillCapturePhoto( AVCapturePhotoOutput captureOutput, AVCaptureResolvedPhotoSettings resolvedSettings )
        {
            DispatchQueue.MainQueue.DispatchAsync( () => {
                View.Layer.Opacity = 0;
                UIView.Animate( 0.25, () => {
                    View.Layer.Opacity = 1;
                } );
            } );
        }

        [Export( "captureOutput:didFinishProcessingPhotoSampleBuffer:previewPhotoSampleBuffer:resolvedSettings:bracketSettings:error:" )]
        void DidFinishProcessingPhoto( AVCapturePhotoOutput captureOutput,
                                       CMSampleBuffer photoSampleBuffer, CMSampleBuffer previewPhotoSampleBuffer,
                                       AVCaptureResolvedPhotoSettings resolvedSettings, AVCaptureBracketedStillImageSettings bracketSettings,
                                       NSError error )
        {
            if( photoSampleBuffer == null )
            {
                Console.WriteLine( $"Error occurred while capturing photo: {error}" );
                return;
            }

            try
            {
                NSData imageData = AVCapturePhotoOutput.GetJpegPhotoDataRepresentation( photoSampleBuffer, previewPhotoSampleBuffer );
                var image = UIImage.LoadFromData( imageData );
                //bool imageIsLandscape = image.Size.Width > image.Size.Height;
                //var maxImageSize = imageIsLandscape ? image.Size.Width : image.Size.Height;
                Session.StopRunning();

                var acceptPreview = new AcceptPreviewImage( image, ( previewImage ) =>
                {
                    ValidateStoragePath();
                    var path = Path.Combine( StoragePath, $"{Guid.NewGuid()}.jpeg" );

                    previewImage.Scale( new CGSize( 1600, 1200 ) )
                                .AsJPEG()
                                .Save( path, true, out error );
                    //imageData.Save( path, true );
                    MediaFiles.Add( new MediaFile( path.RemoveApplicationRoot(), () => File.OpenRead( path ) ) );
                    ImageCount++;
                } );
                GetPresentedViewController().PresentModalViewController( acceptPreview, true );
                //this.PresentModalViewController( acceptPreview, true );
            }
            catch( Exception e )
            {
                StaticLogger.ExceptionLogger( e );
            }
            finally
            {
                Session.StartRunning();
            }

            //PHPhotoLibrary.RequestAuthorization( status => {
            //    if( status == PHAuthorizationStatus.Authorized )
            //    {
            //        PHPhotoLibrary.SharedPhotoLibrary.PerformChanges( () => {
            //            PHAssetCreationRequest.CreationRequestForAsset().AddResource( PHAssetResourceType.Photo, imageData, null );
            //        }, ( success, err ) => {
            //            if( !success )
            //            {
            //                Console.WriteLine( $"Error occurred while saving photo to photo library: {err}" );
            //            }
            //            else
            //            {
            //                Console.WriteLine( "Photo was saved to photo library" );
            //            }
            //        } );
            //    }
            //    else
            //    {
            //        Console.WriteLine( "Not authorized to save photo" );
            //    }
            //} );
        }

        //[Export( "captureOutput:didFinishProcessingRawPhotoSampleBuffer:previewPhotoSampleBuffer:resolvedSettings:bracketSettings:error:" )]
        //void DidFinishProcessingRawPhoto( AVCapturePhotoOutput captureOutput,
        //                                  CMSampleBuffer rawSampleBuffer, CMSampleBuffer previewPhotoSampleBuffer,
        //                                  AVCaptureResolvedPhotoSettings resolvedSettings, AVCaptureBracketedStillImageSettings bracketSettings,
        //                                  NSError error )
        //{
        //    if( rawSampleBuffer == null )
        //    {
        //        Console.WriteLine( $"Error occurred while capturing photo: {error}" );
        //        return;
        //    }

        //    var filePath = Path.Combine( Path.GetTempPath(), $"{resolvedSettings.UniqueID}.dng" );
        //    NSData imageData = AVCapturePhotoOutput.GetDngPhotoDataRepresentation( rawSampleBuffer, previewPhotoSampleBuffer );
        //    imageData.Save( filePath, true );

        //    //PHPhotoLibrary.RequestAuthorization( status => {
        //    //    if( status == PHAuthorizationStatus.Authorized )
        //    //    {
        //    //        PHPhotoLibrary.SharedPhotoLibrary.PerformChanges( () => {
        //    //            // In iOS 9 and later, it's possible to move the file into the photo library without duplicating the file data.
        //    //            // This avoids using double the disk space during save, which can make a difference on devices with limited free disk space.
        //    //            var options = new PHAssetResourceCreationOptions();
        //    //            options.ShouldMoveFile = true;
        //    //            PHAssetCreationRequest.CreationRequestForAsset().AddResource( PHAssetResourceType.Photo, filePath, options ); // Add move (not copy) option
        //    //        }, ( success, err ) => {
        //    //            if( !success )
        //    //                Console.WriteLine( $"Error occurred while saving raw photo to photo library: {err}" );
        //    //            else
        //    //                Console.WriteLine( "Raw photo was saved to photo library" );

        //    //            NSError rErr;
        //    //            if( NSFileManager.DefaultManager.FileExists( filePath ) )
        //    //                NSFileManager.DefaultManager.Remove( filePath, out rErr );
        //    //        } );
        //    //    }
        //    //    else
        //    //    {
        //    //        Console.WriteLine( "Not authorized to save photo" );
        //    //    }
        //    //} );
        //}

        #endregion

        #region KVO

        void AddObservers()
        {
            // To learn more about KVO visit:
            // http://tirania.org/monomac/archive/2012/Apr-19.html?utm_source=feedburner&utm_medium=feed&utm_campaign=Feed%3A+MiguelsOsxAndIosBlog+(Miguel%27s+OSX+and+iOS+blog)
            runningToken = AddObserver( "session.running", NSKeyValueObservingOptions.New, SessionRunningChanged );
            focusModeToken = AddObserver( "videoDevice.focusMode", NSKeyValueObservingOptions.Old | NSKeyValueObservingOptions.New, FocusModeChanged );
            lensPositionToken = AddObserver( "videoDevice.lensPosition", NSKeyValueObservingOptions.New, LensPositionChanged );
            exposureModeToken = AddObserver( "videoDevice.exposureMode", NSKeyValueObservingOptions.Old | NSKeyValueObservingOptions.New, ExposureModeChanged );
            exposureDurationToken = AddObserver( "videoDevice.exposureDuration", NSKeyValueObservingOptions.New, ExposureDurationChanged );
            isoToken = AddObserver( "videoDevice.ISO", NSKeyValueObservingOptions.New, ISOChanged );
            exposureTargetBiasToken = AddObserver( "videoDevice.exposureTargetBias", NSKeyValueObservingOptions.New, ExposureTargetBiasChanged );
            exposureTargetOffsetToken = AddObserver( "videoDevice.exposureTargetOffset", NSKeyValueObservingOptions.New, ExposureTargetOffsetChanged );
            whiteBalanceModeToken = AddObserver( "videoDevice.whiteBalanceMode", NSKeyValueObservingOptions.Old | NSKeyValueObservingOptions.New, WhiteBalanceModeChange );
            deviceWhiteBalanceGainsToken = AddObserver( "videoDevice.deviceWhiteBalanceGains", NSKeyValueObservingOptions.New, DeviceWhiteBalanceGainsChange );


            subjectAreaDidChangeToken = NSNotificationCenter.DefaultCenter.AddObserver( AVCaptureDevice.SubjectAreaDidChangeNotification, SubjectAreaDidChange, VideoDevice );
            runtimeErrorToken = NSNotificationCenter.DefaultCenter.AddObserver( AVCaptureSession.RuntimeErrorNotification, SessionRuntimeError, Session );
            // A session can only run when the app is full screen. It will be interrupted in a multi-app layout, introduced in iOS 9,
            // see also the documentation of AVCaptureSessionInterruptionReason. Add observers to handle these session interruptions
            // and show a preview is paused message. See the documentation of AVCaptureSessionWasInterruptedNotification for other
            // interruption reasons.
            wasInterruptedToken = NSNotificationCenter.DefaultCenter.AddObserver( AVCaptureSession.WasInterruptedNotification, SessionWasInterrupted, Session );
            interruptionEndedToken = NSNotificationCenter.DefaultCenter.AddObserver( AVCaptureSession.InterruptionEndedNotification, SessionInterruptionEnded, Session );
        }

        void RemoveObservers()
        {
            subjectAreaDidChangeToken.Dispose();
            runtimeErrorToken.Dispose();
            wasInterruptedToken.Dispose();
            interruptionEndedToken.Dispose();

            runningToken.Dispose();
            focusModeToken.Dispose();
            lensPositionToken.Dispose();
            exposureModeToken.Dispose();
            exposureDurationToken.Dispose();
            isoToken.Dispose();
            exposureTargetBiasToken.Dispose();
            exposureTargetOffsetToken.Dispose();
            whiteBalanceModeToken.Dispose();
            deviceWhiteBalanceGainsToken.Dispose();
        }

        void SessionRunningChanged( NSObservedChange obj )
        {
            var isRunning = false;
            if( obj.NewValue != null && obj.NewValue != NSNull.Null )
                isRunning = obj.NewValue.AsBool();

            DispatchQueue.MainQueue.DispatchAsync( () => {
                //CameraButton.Enabled = isRunning && ( AVCaptureDevice.DevicesWithMediaType( AVMediaType.Video ).Length > 1 );
                //RecordButton.Enabled = isRunning && ( CaptureModeControl.SelectedSegment == ( int )CaptureMode.Movie );
                //PhotoButton.Enabled = isRunning;
                //HUDButton.Enabled = isRunning;
                //CaptureModeControl.Enabled = isRunning;
                takePhotoBtn.Enabled = isRunning;
            } );
        }

        void FocusModeChanged( NSObservedChange obj )
        {
            var newValue = obj.NewValue;
            var oldValue = obj.OldValue;

            if( newValue != null && newValue != NSNull.Null )
            {
                var newMode = ( AVCaptureFocusMode )newValue.AsInt();
                DispatchQueue.MainQueue.DispatchAsync( () => {
                    //FocusModeControl.SelectedSegment = Array.IndexOf( focusModes, newMode );
                    //LensPositionSlider.Enabled = ( newMode == AVCaptureFocusMode.Locked );

                    if( oldValue != null && oldValue != NSNull.Null )
                    {
                        var oldMode = ( AVCaptureFocusMode )oldValue.AsInt();
                        Console.WriteLine( $"focus mode: {StringFromFocusMode( oldMode )} -> {StringFromFocusMode( newMode )}" );
                    }
                    else
                    {
                        Console.WriteLine( $"focus mode: {StringFromFocusMode( newMode )}" );
                    }
                } );
            }
        }

        void LensPositionChanged( NSObservedChange obj )
        {
            var newValue = obj.NewValue;
            if( newValue != null && newValue != NSNull.Null )
            {
                AVCaptureFocusMode focusMode = VideoDevice.FocusMode;
                float newLensPosition = newValue.AsFloat();
                //DispatchQueue.MainQueue.DispatchAsync( () => {
                //    if( focusMode != AVCaptureFocusMode.Locked )
                //        LensPositionSlider.Value = newLensPosition;
                //    LensPositionValueLabel.Text = newLensPosition.ToString( "F1", CultureInfo.InvariantCulture );
                //} );
            }
        }

        void ExposureModeChanged( NSObservedChange obj )
        {
            var newValue = obj.NewValue;
            var oldValue = obj.OldValue;

            if( newValue != null && newValue != NSNull.Null )
            {
                var newMode = ( AVCaptureExposureMode )newValue.AsInt();
                if( oldValue != null && oldValue != NSNull.Null )
                {
                    var oldMode = ( AVCaptureExposureMode )oldValue.AsInt();

                    // It抯 important to understand the relationship between ExposureDuration and the minimum frame rate as represented by ActiveVideoMaxFrameDuration.
                    // In manual mode, if ExposureDuration is set to a value that's greater than ActiveVideoMaxFrameDuration, then ActiveVideoMaxFrameDuration will
                    // increase to match it, thus lowering the minimum frame rate. If ExposureMode is then changed to automatic mode, the minimum frame rate will
                    // remain lower than its default. If this is not the desired behavior, the min and max frameRates can be reset to their default values for the
                    // current ActiveFormat by setting ActiveVideoMaxFrameDuration and ActiveVideoMinFrameDuration to CMTime.Invalid.
                    if( oldMode != newMode && oldMode == AVCaptureExposureMode.Custom )
                    {
                        NSError error = null;
                        if( VideoDevice.LockForConfiguration( out error ) )
                        {
                            VideoDevice.ActiveVideoMaxFrameDuration = CMTime.Invalid;
                            VideoDevice.ActiveVideoMinFrameDuration = CMTime.Invalid;
                            VideoDevice.UnlockForConfiguration();
                        }
                        else
                        {
                            Console.WriteLine( $"Could not lock device for configuration: {error}" );
                        }
                    }
                }
                DispatchQueue.MainQueue.DispatchAsync( () => {
                    //ExposureModeControl.SelectedSegment = Array.IndexOf( exposureModes, newMode );
                    //ExposureDurationSlider.Enabled = ( newMode == AVCaptureExposureMode.Custom );
                    //ISOSlider.Enabled = ( newMode == AVCaptureExposureMode.Custom );

                    if( oldValue != null && oldValue != NSNull.Null )
                    {
                        var oldMode = ( AVCaptureExposureMode )oldValue.AsInt();
                        Console.WriteLine( $"exposure mode: {StringFromExposureMode( oldMode )} -> {StringFromExposureMode( newMode )}" );
                    }
                    else
                    {
                        Console.WriteLine( $"exposure mode: {StringFromExposureMode( newMode )}" );
                    }
                } );
            }
        }

        void ExposureDurationChanged( NSObservedChange obj )
        {
            var newValue = obj.NewValue;
            var oldValue = obj.OldValue;

            if( newValue != null && newValue != NSNull.Null )
            {
                double newDurationSeconds = newValue.AsCMTime().Seconds;
                AVCaptureExposureMode exposureMode = VideoDevice.ExposureMode;

                double minDurationSeconds = Math.Max( VideoDevice.ActiveFormat.MinExposureDuration.Seconds, ExposureMinDuration );
                double maxDurationSeconds = VideoDevice.ActiveFormat.MaxExposureDuration.Seconds;
                // Map from duration to non-linear UI range 0-1
                double p = ( newDurationSeconds - minDurationSeconds ) / ( maxDurationSeconds - minDurationSeconds ); // Scale to 0-1
                //DispatchQueue.MainQueue.DispatchAsync( () => {
                //    if( exposureMode != AVCaptureExposureMode.Custom )
                //        ExposureDurationSlider.Value = ( float )Math.Pow( p, 1 / ExposureDurationPower ); // Apply inverse power
                //    ExposureDurationValueLabel.Text = FormatDuration( newDurationSeconds );
                //} );
            }
        }

        void ISOChanged( NSObservedChange obj )
        {
            var newValue = obj.NewValue;
            if( newValue != null && newValue != NSNull.Null )
            {
                float newISO = newValue.AsFloat();
                AVCaptureExposureMode exposureMode = VideoDevice.ExposureMode;

                //DispatchQueue.MainQueue.DispatchAsync( () => {
                //    if( exposureMode != AVCaptureExposureMode.Custom )
                //        ISOSlider.Value = newISO;
                //    ISOValueLabel.Text = ( ( int )newISO ).ToString( CultureInfo.InvariantCulture );
                //} );
            }
        }

        void ExposureTargetBiasChanged( NSObservedChange obj )
        {
            var newValue = obj.NewValue;
            if( newValue != null && newValue != NSNull.Null )
            {
                float newExposureTargetBias = newValue.AsFloat();
                DispatchQueue.MainQueue.DispatchAsync( () => {
                    //ExposureTargetBiasValueLabel.Text = newExposureTargetBias.ToString( "F1", CultureInfo.InvariantCulture );
                } );
            }
        }

        void ExposureTargetOffsetChanged( NSObservedChange obj )
        {
            var newValue = obj.NewValue;
            if( newValue != null && newValue != NSNull.Null )
            {
                float newExposureTargetOffset = newValue.AsFloat();
                //DispatchQueue.MainQueue.DispatchAsync( () => {
                //    ExposureTargetOffsetSlider.Value = newExposureTargetOffset;
                //    ExposureTargetOffsetValueLabel.Text = newExposureTargetOffset.ToString( "F1", CultureInfo.InvariantCulture );
                //} );
            }
        }

        void WhiteBalanceModeChange( NSObservedChange obj )
        {
            var newValue = obj.NewValue;
            var oldValue = obj.OldValue;

            if( newValue != null && newValue != NSNull.Null )
            {
                var newMode = ( AVCaptureWhiteBalanceMode )newValue.AsInt();
                DispatchQueue.MainQueue.DispatchAsync( () => {
                    //WhiteBalanceModeControl.SelectedSegment = Array.IndexOf( whiteBalanceModes, newMode );
                    //TemperatureSlider.Enabled = ( newMode == AVCaptureWhiteBalanceMode.Locked );
                    //TintSlider.Enabled = ( newMode == AVCaptureWhiteBalanceMode.Locked );

                    if( oldValue != null && oldValue != NSNull.Null )
                    {
                        var oldMode = ( AVCaptureWhiteBalanceMode )oldValue.AsInt();
                        Console.WriteLine( $"white balance mode: {StringFromWhiteBalanceMode( oldMode )} -> {StringFromWhiteBalanceMode( newMode )}" );
                    }
                } );
            }
        }

        unsafe void DeviceWhiteBalanceGainsChange( NSObservedChange obj )
        {
            var gains = ( NSValue )obj.NewValue;
            if( gains != null )
            {
                AVCaptureWhiteBalanceGains newGains;
                gains.StoreValueAtAddress( ( IntPtr )( void* )&newGains );

                AVCaptureWhiteBalanceTemperatureAndTintValues newTemperatureAndTint = VideoDevice.GetTemperatureAndTintValues( newGains );
                AVCaptureWhiteBalanceMode whiteBalanceMode = VideoDevice.WhiteBalanceMode;
                //DispatchQueue.MainQueue.DispatchAsync( () => {
                //    if( whiteBalanceMode != AVCaptureWhiteBalanceMode.Locked )
                //    {
                //        TemperatureSlider.Value = newTemperatureAndTint.Temperature;
                //        TintSlider.Value = newTemperatureAndTint.Tint;
                //    }

                //    var ci = CultureInfo.InvariantCulture;
                //    TemperatureValueLabel.Text = ( ( int )newTemperatureAndTint.Temperature ).ToString( ci );
                //    TintValueLabel.Text = ( ( int )newTemperatureAndTint.Tint ).ToString( ci );
                //} );
            }
        }

        void SubjectAreaDidChange( NSNotification obj )
        {
            var devicePoint = new CGPoint( 0.5f, 0.5f );
            SetFocusAndMode( VideoDevice.FocusMode, VideoDevice.ExposureMode, devicePoint, false );
        }

        void SessionRuntimeError( NSNotification notification )
        {
            var error = ( NSError )notification.UserInfo[ AVCaptureSession.ErrorKey ];
            Console.WriteLine( $"Capture session runtime error: {error}" );

            if( error.Code == ( long )AVError.MediaServicesWereReset )
            {
                sessionQueue.DispatchAsync( () => {
                    // If we aren't trying to resume the session, try to restart it, since it must have been stopped due to an error (see -[resumeInterruptedSession:])
                    if( sessionRunning )
                    {
                        Session.StartRunning();
                        sessionRunning = Session.Running;
                    }
                    //else
                    //{
                    //    DispatchQueue.MainQueue.DispatchAsync( () => {
                    //        ResumeButton.Hidden = false;
                    //    } );
                    //}
                } );
            }
            //else
            //{
            //    ResumeButton.Hidden = false;
            //}
        }

        void SessionWasInterrupted( NSNotification notification )
        {
            // In some scenarios we want to enable the user to restart the capture session.
            // For example, if music playback is initiated via Control Center while using AVCamManual,
            // then the user can let AVCamManual resume the session running, which will stop music playback.
            // Note that stopping music playback in Control Center will not automatically resume the session.
            // Also note that it is not always possible to resume, see ResumeInterruptedSession method.
            // In iOS 9 and later, the notification's UserInfo dictionary contains information about why the session was interrupted
            var reason = ( AVCaptureSessionInterruptionReason )notification.UserInfo[ AVCaptureSession.InterruptionReasonKey ].AsInt();
            Console.WriteLine( $"Capture session was interrupted with reason {reason}" );

            if( reason == AVCaptureSessionInterruptionReason.AudioDeviceInUseByAnotherClient ||
                reason == AVCaptureSessionInterruptionReason.VideoDeviceInUseByAnotherClient )
            {
                // Simply fade-in a button to enable the user to try to resume the session running
                //ResumeButton.Hidden = false;
                //ResumeButton.Alpha = 0;
                //UIView.Animate( 0.25, () => ResumeButton.Alpha = 1 );
            }
            else if( reason == AVCaptureSessionInterruptionReason.VideoDeviceNotAvailableWithMultipleForegroundApps )
            {
                // Simply fade-in a label to inform the user that the camera is unavailable
                CameraUnavailableLabel.Hidden = false;
                CameraUnavailableLabel.Alpha = 0;
                UIView.Animate( 0.25, () => CameraUnavailableLabel.Alpha = 1 );
            }
        }

        void SessionInterruptionEnded( NSNotification obj )
        {
            Console.WriteLine( "Capture session interruption ended" );

            //if( !ResumeButton.Hidden )
            //    UIView.AnimateNotify( 0.25, () => ResumeButton.Alpha = 0, ( finished ) => ResumeButton.Hidden = true );
            if( !CameraUnavailableLabel.Hidden )
                UIView.AnimateNotify( 0.25, () => CameraUnavailableLabel.Alpha = 0, ( finished ) => CameraUnavailableLabel.Hidden = true );
        }

        string FormatDuration( double duration )
        {
            var ci = CultureInfo.InvariantCulture;

            if( duration <= 0 )
                throw new ArgumentOutOfRangeException( nameof( duration ) );

            if( duration < 1 )
            {
                // e.x. 1/1000 1/350 etc
                var digits = ( int )Math.Max( 0, 2 + Math.Floor( Math.Log10( duration ) ) );
                string pattern = "1/{0:####." + new string( '0', digits ) + "}";
                return string.Format( pattern, 1.0 / duration, ci );
            }

            return duration.ToString( "F2", ci );
        }

        #endregion

        #region Utilities

        static string StringFromFocusMode( AVCaptureFocusMode focusMode )
        {
            switch( focusMode )
            {
                case AVCaptureFocusMode.Locked:
                    return "Locked";
                case AVCaptureFocusMode.AutoFocus:
                    return "Auto";
                case AVCaptureFocusMode.ContinuousAutoFocus:
                    return "ContinuousAuto";
                default:
                    return "INVALID FOCUS MODE";
            }
        }

        string StringFromExposureMode( AVCaptureExposureMode exposureMode )
        {
            switch( exposureMode )
            {
                case AVCaptureExposureMode.Locked:
                    return "Locked";
                case AVCaptureExposureMode.AutoExpose:
                    return "Auto";
                case AVCaptureExposureMode.ContinuousAutoExposure:
                    return "ContinuousAuto";
                case AVCaptureExposureMode.Custom:
                    return "Custom";
                default:
                    return "INVALID EXPOSURE MODE";
            }
        }

        string StringFromWhiteBalanceMode( AVCaptureWhiteBalanceMode whiteBalanceMode )
        {
            switch( whiteBalanceMode )
            {
                case AVCaptureWhiteBalanceMode.Locked:
                    return "Locked";
                case AVCaptureWhiteBalanceMode.AutoWhiteBalance:
                    return "Auto";
                case AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance:
                    return "ContinuousAuto";
                default:
                    return "INVALID WHITE BALANCE MODE";
            }
        }

        private void ValidateStoragePath()
        {
            try
            {
                if( string.IsNullOrWhiteSpace( StoragePath ) )
                    StoragePath = DefaultStoragePath;

                if( !Directory.Exists( StoragePath ) )
                    Directory.CreateDirectory( StoragePath );
            }
            catch( Exception e )
            {
                StaticLogger.Log( e, Level.Error );
            }
        }

        private UIViewController GetPresentedViewController()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            if( window == null )
            {
                throw new InvalidOperationException( "There's no current active window" );
            }

            var viewController = window.RootViewController;

#if __IOS_10__
            if (viewController == null || (viewController.PresentedViewController != null && viewController.PresentedViewController.GetType() == typeof(UIAlertController)))
            {
                window =
                    UIApplication.SharedApplication.Windows.OrderByDescending(w => w.WindowLevel)
                        .FirstOrDefault(w => w.RootViewController != null);

                if (window == null)
                {
                    throw new InvalidOperationException("Could not find current view controller");
                }

                viewController = window.RootViewController;
            }
#endif
            while( viewController.PresentedViewController != null )
            {
                viewController = viewController.PresentedViewController;
            }

            return viewController;
        }

        #endregion
    }
}
