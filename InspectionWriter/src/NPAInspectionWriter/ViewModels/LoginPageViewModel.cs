using NPAInspectionWriter.Helpers;
using Xamarin.Forms;
using System.Diagnostics;
using NPAInspectionWriter.Resx;
using NPAInspectionWriter.Services;
using NPAInspectionWriter.Models;
using System.Threading.Tasks;
using System;
using Acr.UserDialogs;
using System.Linq;
using NPAInspectionWriter.AppData;
using System.Runtime.InteropServices.WindowsRuntime;
using NPAInspectionWriter.Views.Pages;

namespace NPAInspectionWriter.ViewModels
{
    public class LoginPageViewModel : CRWriterBase
    {

        public LoginPageViewModel(AppInfo appInfo)
        {
            AppName = appInfo?.AppName ?? ModuleResources.AppNameNotPresent;
            Logo = appInfo?.LogoSource ?? ImageSource.FromResource("NPA.Xamarin.Common.Assets.NPALogo.png", typeof(AppInfo));

            if (string.IsNullOrWhiteSpace(appInfo?.AppName) || string.IsNullOrWhiteSpace(appInfo?.AppVersion))
            {
                Debug.WriteLine("App Info instance could not be located or was not initialized properly");
            }
        }


#if DEBUG
        private string _userName = "inspection";
#else
        private string _userName;
#endif
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

#if DEBUG
        private string _password = "#1crwriterGuy";
#else
        private string _password;
#endif
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private string _appName;
        public string AppName
        {
            get { return _appName; }
            set { SetProperty(ref _appName, value); }
        }

        private ImageSource _logo;
        public ImageSource Logo
        {
            get { return _logo; }
            set { SetProperty(ref _logo, value); }
        }

        private Command _loginCommand;
        public Command LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand = new Command(async () =>
                {
                    UserDialogs.Instance.ShowLoading("Attempting Login");
                    var aaaService = new AAAService(AppRepository.Instance, App.AppInfo);
                    var cont = await aaaService.LoginAsync(UserName, Password);
                    if (cont)
                    {
                        await HandleLoggedInUserSession();
                    }
                    else
                    {
                        var db = AppRepository.Instance;
                        var errMsg = await db.GetCurrentSettingAsync(Constants.LastErrorMessageKey);

                        if (errMsg == AppMessages.RequiredAppUpdateErrorMessage)
                        {
                            var newVersion = await db.GetCurrentSettingAsync(Constants.RequiredAppVersionKey);
                            //await NavigationService.NavigateAsync( $"npa://inspectionwriter.app/UpdateAvailable?newVersion={newVersion}" );
                        }
                        else
                        {
                            await UserDialogs.Instance.AlertAsync(errMsg, "Something went wrong", "OK");
                        }
                    }
                    UserDialogs.Instance.HideLoading();
                }));
            }
        }

        public override string Title => "Defaut";

        public override ImageSource Icon => "Default";

        private async Task HandleLoggedInUserSession()
        {
            try
            {
                var db = AppRepository.Instance;
                var user = (db.Query<OfflineUserAuthentication>("SELECT * FROM OfflineUserAuthentication oua " +
                                                                  "INNER JOIN CurrentObject co ON oua.UserAccountId = co.ObjectValue " +
                                                                  "WHERE co.Key = ?", Constants.CurrentUserKey))?.FirstOrDefault();

                if (user == null)
                {
                    await UserDialogs.Instance.AlertAsync("Please check your information and try again", "Invaid User Information", "OK");
                    return;
                }
                var searchPage = new VehicleSearchPage();
                await Navigation.PushAsync(searchPage);

                //await Navigation.PushAsync(new TestPage());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

    }
}
