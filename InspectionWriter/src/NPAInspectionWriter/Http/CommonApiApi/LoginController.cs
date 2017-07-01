using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NPAInspectionWriter.iOS.Extensions;
using NPAInspectionWriter.Logging;
using NPAInspectionWriter.iOS.Models;
using NPAInspectionWriter.Extensions;

namespace NPA.XF.Http.CommonApiApi
{
    public class LoginController
    {
        private CommonApiClient _client { get; }
        private ILog _logger { get; }

        internal LoginController( CommonApiClient client )
        {
            _client = client;
            _logger = _client._options.Logger;
        }

        public Task<LoginResponse> LoginAsync( string username, string password, CancellationToken cancellationToken = default( CancellationToken ) )
        {
            try
            {
                return _client.PostForObjectAsync<LoginResponse>( "/Api/Login", new
                {
                    Username = username,
                    Password = password
                }, cancellationToken );
            }
            catch( Exception e )
            {
                if( _client.lastResponse.IsSuccessStatusCode )
                {
                    _logger.Error( e );
                    throw;
                }

                _logger.Debug( e );
                throw new WebException( $"{( int )_client.lastResponse.StatusCode} - {_client.lastResponse.ReasonPhrase}", e );
            }
        }
    }
}
