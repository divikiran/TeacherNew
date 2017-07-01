#if USE_MOCKS
using System;
using System.Threading.Tasks;
using NPAInspectionWriter.Models;
using Newtonsoft.Json;
using NPAInspectionWriter.AppData;
using NPAInspectionWriter.Helpers;

namespace NPAInspectionWriter.Services
{
#pragma warning disable 1998
    public class AAAServiceMock : IAAAService
    {
        public async Task<bool> LoginAsync( string userName, string password )
        {
            // Sanity Checks...
            if( string.IsNullOrWhiteSpace( userName ) )
            {
                Settings.Current.AppUser.LastErrorMessage = AppMessages.BlankUserNameErrorMessage;
                return false;
            }
            else if( string.IsNullOrWhiteSpace( password ) )
            {
                Settings.Current.AppUser.LastErrorMessage = AppMessages.BlankPasswordErrorMessage;
                return false;
            }

            if( userName == "invalid" )
            {
                Settings.Current.AppUser.LastErrorMessage = "Authorization has been denied for this request.";
                return false;
            }

            string response = "{\"userAccountId\":\"47e0ecd7-b056-4fd4-bc08-78feda408b62\",\"userName\":\"mockUser\",\"userLevel\":99,\"location\":\"San Diego\",\"locationId\":\"c4b1d148-57e2-4fd2-9dae-0121d40854a0\",\"emailAddress\":\"mockUser@npauctions.com\",\"currentAppVersion\":\"3.5.1B\",\"currentAppVersionRequiredDate\":\"2016-04-25T00:00:00\",\"canCreateCr\":true}";
            var amsUser = JsonConvert.DeserializeObject<AmsUser>( response );

            Settings.Current.AppUser.UserAccountId = amsUser.UserAccountId;
            Settings.Current.AppUser.UserName = amsUser.UserName;
            Settings.Current.AppUser.UserLevel = amsUser.UserLevel;
            Settings.Current.AppUser.Location = amsUser.Location;
            Settings.Current.AppUser.LocationId = amsUser.LocationId;
            Settings.Current.AppUser.EmailAddress = amsUser.EmailAddress;
            Settings.Current.AppUser.CanCreateInspections = amsUser.CanCreateInspections;

            Settings.Current.AppUser.IsLoggedIn = true;
            Settings.Current.AppUser.LoginExpires = DateTime.Now + TimeSpan.FromHours( Constants.LoginExpiresHours );
            Settings.Current.AppUser.LastErrorMessage = Settings.Current.AppUser.CanCreateInspections ? AppMessages.MatrixGlitchErrorMessage : AppMessages.UnauthorizedErrorMessage;

            // If the user cannot create CR's they will be prevented from logging in.
            return Settings.Current.AppUser.CanCreateInspections;
        }

        public async Task LogoutAsync()
        {
            //Settings.Current.AppUser.Logout();
        }

        Task<bool> IAAAService.LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
#endif