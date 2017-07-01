using System;
using System.Diagnostics;
using System.Reflection;
using Badge.Plugin;
using NPAInspectionWriter.iOS.Extensions;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.AppData;
using NPAInspectionWriter.Models;
using NPAInspectionWriter.Services;
using NPAInspectionWriter.Views;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Xamarin.Forms;
using Acr.UserDialogs;
using NPAInspectionWriter.Views.Pages;
using NPAInspectionWriter.Caching;
using Newtonsoft.Json.Serialization;

namespace NPAInspectionWriter
{
    public partial class App : Application
    {
        #region Static Members

        public static string AppVersion { get; private set; }

        public static App current { get; private set; }

        public static AppInfo AppInfo = new AppInfo();

        #endregion

        //IEventAggregator _eventAggregator;

        public App()
        {
            InitializeComponent();

            Init();

            MainPage = new NavigationPage(new LoginPage()) { BarBackgroundColor = Color.Black, BarTextColor = Color.White };
        }

        void Init()
        {
			var baseVersion = $"{this.GetAppVersion(3)}{Settings.Current.ApiVersion}";
			AppVersion = Settings.Current.Environment == RuntimeEnvironment.Production ? baseVersion : $"{baseVersion} ({Settings.Current.Environment})";
			AppInfo.AppName = AppResources.AppName;
			AppInfo.AppVersion = AppVersion;
			AppInfo.LogoSource = ImageSource.FromResource(Constants.NPALogoResourcePath, GetType().GetTypeInfo().Assembly);

            AppRepository.Instance.Initialize();
        }


		#region Prism Setup
		/*

        private void RegisterOfflineServices()
        {
            // TODO: Implement Offline Services
        }

        
*/
		#endregion


		private bool IsUserLoggedIn()
        {
            try
            {
                var db = AppRepository.Instance;

                if( db == null ) { Debug.WriteLine( "What the....... the db is null" ); return false; }

                string sessionResult = AsyncHelpers.RunSync( async () => await db.GetCurrentSettingAsync( Constants.SessionExpiresKey ) );

                if( string.IsNullOrWhiteSpace( sessionResult ) ) return false;

                DateTime expiredSession;

                if( DateTime.TryParse( sessionResult, out expiredSession ) )
                {
                    if(DateTime.Now.CompareTo( expiredSession ) <= 0)
                    {
                        var authCred = AsyncHelpers.RunSync(async () => await AppRepository.Instance.GetCurrentSettingAsync("AuthCred"));
                        CachingAgent.Provider.Set("AuthCred", authCred);
                        return true;
                    }else{
                        AsyncHelpers.RunSync(async() => await AppRepository.Instance.ClearCurrentObjectsAsync("AuthCred"));
                        return false;
                    }
                }
            }
            catch( Exception e )
            {
                Debug.WriteLine( e );
            }

            return false;
        }

        protected override void OnStart()
        {
            base.OnStart();
            ConfigureStartup();

            Debug.WriteLine( "App starting up");

            var db = AppRepository.Instance;
            db?.ClearCurrentObjects( Constants.RequiredAppVersionKey, Constants.LastErrorMessageKey );
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            CrossConnectivity.Current.ConnectivityChanged -= ConnectivityChanged;
            var db = AppRepository.Instance;
            db.ClearCurrentObjects( Constants.RequiredAppVersionKey, Constants.LastErrorMessageKey );

            var localInspectionCount = AsyncHelpers.RunSync( async () => await db.Table<LocalInspection>()
                                                                                 .Where( i => i.IsLocalInspection )
                                                                                 .CountAsync() );
            CrossBadge.Current.SetBadge( localInspectionCount );

			Debug.WriteLine( "App going to sleep");
        }

        protected override void OnResume()
        {
            base.OnResume();
            ConfigureStartup();
			Debug.WriteLine( "App resuming");
        }

        private void ConfigureStartup()
        {
            Settings.Current.IsConnected = CrossConnectivity.Current.IsConnected;
            CrossConnectivity.Current.ConnectivityChanged += ConnectivityChanged;
        }

        protected async void ConnectivityChanged( object sender, ConnectivityChangedEventArgs e )
        {
            // TODO: Swap Services when going on/offline so that we can use offline records or the API.

            var connected = Settings.Current.IsConnected;
            Settings.Current.IsConnected = e.IsConnected;
            if( connected && !e.IsConnected )
            {
                //we went offline, should alert the user and also update UI (done via settings)
                await UserDialogs.Instance.PromptAsync( AppResources.OfflineAlertTitle, AppMessages.OfflineErrorMessage, AppResources.OkButtonText );
                //RegisterOfflineServices(); // TODO: This needs to be implemented
            }
            else
            {
                //RegisterOnlineServices();
            }
        }


        public static async void HandleExpiredSessionEvent()
        {
            var aaa = new AAAService(AppRepository.Instance, AppInfo);
            await aaa.LogoutAsync();
            App.Current.MainPage = new NavigationPage(new LoginPage()) { 
                BarBackgroundColor = Color.Black, 
                BarTextColor = Color.White 
            };
        }
    }
}
