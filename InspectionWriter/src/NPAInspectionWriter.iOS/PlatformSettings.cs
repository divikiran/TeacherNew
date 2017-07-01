using System;
using System.Diagnostics;
using Foundation;
using NPAInspectionWriter.iOS.Extensions;
using NPAInspectionWriter.Extensions;
using NPAInspectionWriter.Helpers;

namespace NPAInspectionWriter.iOS
{
    public class PlatformSettings
    {
        const string settingBundlePath = "Settings.bundle/Root.plist";
        const int portDefault = 9050;

        #region Preference Specifier Keys

        // Diagnostics
        const string hostNameOrIp = "loggingServer";
        const string port = "loggingPort";
        const string loggingLevel = "loggingLevel";

        // Developer Settings
        const string apiOverride = "apiOverride";
        const string forceUseStage = "forceUseStage";
        const string apiVersion = "apiVersion";

        // General Settings
        const string hideCancel = "hideCancel";
        const string portraitLock = "portraitLock";
        const string ignoreCellularConnectivity = "ignoreCellularConnectivity";
        const string inspectionLookback = "inspectionLookback";
        const string imageCache = "imageCache";

        // About
        const string appVersion = "appVersion";

        #endregion

        public static PlatformSettings Current { get; private set; }

        private static NSUserDefaults defaults { get; set; }

        static PlatformSettings()
        {
            if( Current == null )
            {
                ReloadNSDefaults();
                defaults.AddPrefernceSpecifierObserver( () =>
                {
                    ReloadNSDefaults();
                    Current.LoadDefaultValues();
                } );
                Current = new PlatformSettings();
            }
        }

        private static void ReloadNSDefaults()
        {
                var bundlePath = NSBundle.MainBundle.PathForResource( settingBundlePath, null );
                var settingsDict = new NSDictionary( bundlePath );

                NSUserDefaults.StandardUserDefaults.RegisterDefaults( settingsDict );
                defaults = NSUserDefaults.StandardUserDefaults;
        }

        private PlatformSettings()
        {
            LoadDefaultValues();
        }

        public string HostNameOrIp { get; private set; } = "10.7.1.75";

        public int Port { get; private set; }

        public bool ForceUseStage { get; private set; }

        public bool PortraitLock { get; private set; }

        public string ApiOverride { get; private set; }

        public int LoggingLevel { get; private set; }

        public int ImageCacheDays { get; private set; }

        public string ImageCacheFolderPath
        {
            get
            {
                var docDirectory = Environment.GetFolderPath( Environment.SpecialFolder.Personal );
                return System.IO.Path.Combine( docDirectory, "ImageCache" );
            }
        }

        public void LoadDefaultValues()
        {
            try
            {
                // DIAGNOSTICS
                HostNameOrIp = defaults.StringFromPreferenceKey( hostNameOrIp )?.Trim();
                Port = defaults.IntFromPreferenceKey( port, portDefault );
                if( Port <= 0 || Port > 65535 ) Port = portDefault;
                LoggingLevel = defaults.IntFromPreferenceKey( loggingLevel, -1 );

                // API SETTINGS
                #region API Settings

                Uri tempUri;
                var overrideUrl = defaults.StringFromPreferenceKey( apiOverride )?.Trim()?.ToLower();
                if( Uri.TryCreate( overrideUrl, UriKind.Absolute, out tempUri ) )
                {
                    Settings.Current.InspectionWriterWebApiBase = ApiOverride = overrideUrl;
                    Settings.Current.DisableWebApiLocationOverride = true;
                }

                ForceUseStage = defaults.BoolFromPreferenceKey( forceUseStage );
                if( ForceUseStage )
                {
                    Settings.Current.InspectionWriterWebApiBase = Constants.StageWebApiBase;
                    Settings.Current.DisableWebApiLocationOverride = true;
                }

                var apiVersionValue = defaults.StringFromPreferenceKey( apiVersion );
                if( !string.IsNullOrWhiteSpace( apiVersionValue ) && apiVersionValue.ToLower() != "default" )
                {
                    var version = apiVersionValue.ToUpper().CharAt( 0 );
                    if( char.IsLetter( version ) )
                    {
                        Settings.Current.ApiVersion = version;
                        Settings.Current.DisableWebApiVersionOverride = true;
                    }
                }

                #endregion

                // GENERAL SETTINGS
                Settings.Current.HideCancelButton = defaults.BoolFromPreferenceKey( hideCancel );

                PortraitLock = defaults.BoolFromPreferenceKey( portraitLock );

                Settings.Current.InspectionLookback = defaults.IntFromPreferenceKey( inspectionLookback );

                ImageCacheDays = defaults.IntFromPreferenceKey( imageCache );

                Settings.Current.IgnoreCellularConnectivity = defaults.BoolFromPreferenceKey( ignoreCellularConnectivity );

                // ABOUT
                defaults.SetValueForKey( App.AppVersion.ToNSString(), appVersion.ToNSString() );

                // Synchronize
                defaults.Synchronize();
            }
            catch( Exception e )
            {
                Debug.WriteLine( "Error loading Settings.bundle" );
                Debug.WriteLine( e );
            }
        }
    }
}
