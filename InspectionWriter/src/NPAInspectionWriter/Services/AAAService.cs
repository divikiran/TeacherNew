using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPAInspectionWriter.iOS.Extensions;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.AppData;
using NPAInspectionWriter.Extensions;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.Models;
using Plugin.Connectivity;
using System.Diagnostics;

namespace NPAInspectionWriter.Services
{
#pragma warning disable 1998
    public class AAAService : IAAAService
    {
        AppRepository _db { get; }
        AppInfo _appInfo { get; }
        //InspectionWriterClient _client { get; }

        public AAAService(AppRepository database, AppInfo appInfo )
        {
            _db = database;
            _appInfo = appInfo;
        }

        public async Task<bool> LoginAsync( string userName, string password )
        {
            // Sanity Checks...
            if( string.IsNullOrWhiteSpace( userName ) )
            {
                await _db.AddCurrentObjectAsync( Constants.LastErrorMessageKey, AppMessages.BlankUserNameErrorMessage );
                return false;
            }
            else if( string.IsNullOrWhiteSpace( password ) )
            {
                await _db.AddCurrentObjectAsync( Constants.LastErrorMessageKey, AppMessages.BlankPasswordErrorMessage );
                return false;
            }
            else if( !CrossConnectivity.Current.IsConnected )
            {
                await _db.AddCurrentObjectAsync( Constants.LastErrorMessageKey, AppMessages.OfflineErrorMessage);
                return false;
            }
            //else if( !await CrossConnectivity.Current.IsReachable( Settings.Current.InspectionWriterWebApiBase.GetUriHostname() ) )
            //{
            //    _logger.Log( "Api is not reachable.", Category.Debug, Priority.High );
            //    Settings.Current.AppUser.LastErrorMessage = AppMessages.MatrixGlitchErrorMessage;
            //    return false;
            //}

           

            AmsUser amsUser = default( AmsUser );

            using( var client = new InspectionWriterClient( userName, password ) )
            {
                try
                {
                    Debug.WriteLine(client.BaseAddress);

                    var webApiUri = $"{client.BaseAddress}" + "login?AppVersion=" + App.AppVersion;
					Debug.WriteLine("URLPath : " + webApiUri);

                    var response = await client.GetAsync(webApiUri);

                    switch( response.StatusCode )
                    {
                        case HttpStatusCode.Unauthorized:
                            DestroySessionSettings(); 
                            await _db.AddCurrentObjectAsync( Constants.LastErrorMessageKey, AppMessages.UnauthorizedLoginMessage );
                            return false;
                    }

                    if( !response.IsSuccessStatusCode )
                    {
                        Debug.WriteLine( $"We got a bad response from the API. HTTP Status Code {response.StatusCode}");
                        await _db.AddCurrentObjectAsync( Constants.LastErrorMessageKey, AppMessages.MatrixGlitchErrorMessage );
                        return false;
                    }

                    string result = await response.Content.ReadAsStringAsync();
                    var jsonResult = JObject.Parse( result );
                    if( !string.IsNullOrWhiteSpace( jsonResult?[ "message" ]?.ToString() ) )
                    {
                        await _db.AddCurrentObjectAsync( Constants.LastErrorMessageKey, jsonResult[ "message" ].ToString() );
                        return false;
                    }

                    amsUser = JsonConvert.DeserializeObject<AmsUser>( result );

                    Settings.Current.AppUser = amsUser;

                    if( !App.AppVersion.VersionIsEqualOrGreater( amsUser.CurrentAppVersion ) )
                    {
                        await _db.AddCurrentObjectAsync( Constants.LastErrorMessageKey, AppMessages.RequiredAppUpdateErrorMessage );
                        await _db.AddCurrentObjectAsync( Constants.RequiredAppVersionKey, amsUser.CurrentAppVersion );
                        return false;
                    }

                    await _db.InsertOrReplaceAsync( (CRWriterUser)amsUser );
                    // When in Rome... why bother securing passwords...
                    await _db.InsertOrReplaceAsync( amsUser.ToOfflineAuthModel( password ) );

                    string locationCode = amsUser.Location;
                    try
                    {
                        locationCode = NPAConstants.LookupLocationCodeById( amsUser.LocationId );
                    }
                    catch( Exception e )
                    {
                        Debug.WriteLine( e );
                    }
                    finally
                    {
                        _appInfo.Location = locationCode;
                        _appInfo.UserName = amsUser.UserName;
                        await _db.AddCurrentObjectAsync( Constants.CurrentUserAndLocationKey, $@"{locationCode}\{amsUser.UserName}" );
                    }
                }
                catch( AggregateException ae )
                {
                    Debug.WriteLine( $"Exception Type: {ae.GetType().Name}");
                    Debug.WriteLine( ae );
                    await _db.AddCurrentObjectAsync( Constants.LastErrorMessageKey, ae.Message );
                    return false;
                }
                catch( WebException we )
                {
                    Debug.WriteLine( $"Exception Type: {we.GetType().Name}" );
                    Debug.WriteLine( we );
                    await _db.AddCurrentObjectAsync( Constants.LastErrorMessageKey, we.Message );
                    return false;
                }
                catch( Exception e1 )
                {
                    Debug.WriteLine( $"Exception Type: {e1.GetType().Name}" );
                    Debug.WriteLine( e1 );
                    await _db.AddCurrentObjectAsync( Constants.LastErrorMessageKey, e1.Message );
                    return false;
                }
            }

            // If the user cannot create CR's they will be prevented from logging in.
            if( amsUser.CanCreateInspections )
            {
                //Settings.Current.AppUser.LoginExpires = DateTime.Now + TimeSpan.FromHours( Constants.LoginExpiresHours );

                Debug.WriteLine( "User successfully logged in." );
                await _db.AddCurrentObjectAsync( Constants.LastErrorMessageKey, AppMessages.MatrixGlitchErrorMessage );
            }
            else
            {
                //Settings.Current.AppUser.LoginExpires = DateTime.Now - TimeSpan.FromDays( 2 );
                await _db.AddCurrentObjectAsync( Constants.LastErrorMessageKey, AppMessages.ExpiredLoginMessage );
            }

            await _db.AddCurrentObjectAsync( amsUser );
            await _db.AddCurrentObjectAsync( Constants.SessionExpiresKey, DateTime.Now.AddHours( Constants.LoginExpiresHours ) );

            Settings.Current.AuthToken = amsUser.AuthToken;
            Settings.Current.UserLevel = (UserLevel)amsUser.UserLevel;
            Settings.Current.UserName = amsUser.UserName;
            Settings.Current.LocationId = amsUser.LocationId;

            return amsUser != null;
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                DestroySessionSettings();
            }
            catch( Exception e )
            {
                Debug.WriteLine( e );
            }

            return false;
        }

        private void DestroySessionSettings()
        {
            AppRepository.Instance.ClearCurrentObjects(Constants.SessionExpiresKey);
            AppRepository.Instance.ClearCurrentObjects("AuthCred");
			Settings.Current.AuthToken = string.Empty;
            Settings.Current.UserLevel = default( UserLevel );
            Settings.Current.LocationId = default( Guid );
        }
    }
}
