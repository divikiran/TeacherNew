using System;
using System.IO;
using System.Net;
using Foundation;
using HockeyApp.iOS;
using NPAInspectionWriter.iOS.Extensions;
using UIKit;
using UserNotifications;
using Xamarin.Forms.Platform.iOS;

namespace NPAInspectionWriter.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        NSObject _observer;

        //
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes() { TextColor = UIColor.White });
            UINavigationBar.Appearance.BackgroundColor = UIColor.Black;
            InitializeHockeyApp();
            InitializeLibraries();

            // Prevent device from going to sleep while app is open
            UIApplication.SharedApplication.IdleTimerDisabled = true;
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, true);

            var settings = UIUserNotificationSettings.GetSettingsForTypes(
                                UIUserNotificationType.Alert
                                | UIUserNotificationType.Badge
                                | UIUserNotificationType.Sound,
                                new NSSet());
            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);

#if DEBUG
            //TODO: Remove in production
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
#endif

            LoadApplication( new App() );

            _observer = NSNotificationCenter.DefaultCenter.AddObserver( (NSString)"NSUserDefaultsDidChangeNotification", SettingsChanged );
            //System.Threading.Tasks.Task.Factory.StartNew( CleanupCache );
            return base.FinishedLaunching( app, options );
        }

        private void InitializeLibraries()
        {
            global::Rg.Plugins.Popup.IOS.Popup.Init();
            global::Xamarin.Forms.Forms.Init();
        }

        private void InitializeHockeyApp()
        {
            // 1) Make sure Debugger isn't currently attached.
            if( !System.Diagnostics.Debugger.IsAttached
                // 2) Make sure the key isn't just the placeholder
                && Helpers.Constants.HockeyAppiOS != $"__{nameof( Helpers.Constants.HockeyAppiOS )}__"
                // 3) Make sure the key is actually set to something and isn't an empty string or white space
                && !string.IsNullOrWhiteSpace( Helpers.Constants.HockeyAppiOS ) )
            {
                var manager = BITHockeyManager.SharedHockeyManager;
                manager.Configure( Helpers.Constants.HockeyAppiOS );
                manager.LogLevel = BITLogLevel.Debug;
                manager.StartManager();
            }
            else
            {
                BITHockeyManager.SharedHockeyManager.DisableCrashManager = true;
                BITHockeyManager.SharedHockeyManager.DisableFeedbackManager = true;
            }
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations( UIApplication application, UIWindow forWindow )
        {
            return UIInterfaceOrientationMask.All;
        }

        private void SettingsChanged( NSNotification obj )
        {
            //App.current.Logger.Log( "Settings Changed.", Prism.Logging.Category.Info, Prism.Logging.Priority.High );
            //PlatformSettings.Current.LoadDefaultValues();
            //var initializer = new PlatformInitializer();
            //ifnitializer.ConfigureLogging( App.current.Container );
        }

        private void CleanupCache()
        {
            var taskId = UIApplication.SharedApplication.BeginBackgroundTask( () => {
                NPAInspectionWriter.Logging.StaticLogger.AlertLogger( "Cleanup Cache canceling due to expiring time limit." );
            } );

            var oldestDate = DateTime.Now.AddDays( - PlatformSettings.Current.ImageCacheDays );
            int deletedFiles = 0;
            foreach( var filePath in Directory.EnumerateFiles( PlatformSettings.Current.ImageCacheFolderPath ) )
            {
                if( DateTime.Compare( oldestDate, File.GetCreationTime( filePath ) ) > 0 )
                {
                    File.Delete( filePath );
                    deletedFiles++;
                }
            }

            if( deletedFiles > 0 )
            {
                NPAInspectionWriter.Logging.StaticLogger.InfoLogger( $"{deletedFiles} images were deleted from the image cache on {UIDevice.CurrentDevice.Name}" );
            }

            UIApplication.SharedApplication.EndBackgroundTask( taskId );
        }

        protected override void Dispose( bool disposing )
        {
            if( _observer != null )
            {
                _observer.Dispose();
                _observer = null;
            }

            base.Dispose( disposing );
        }
    }
}
