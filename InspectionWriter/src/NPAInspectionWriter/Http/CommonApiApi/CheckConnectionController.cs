using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NPAInspectionWriter.Extensions;
using System.Net;

namespace NPA.XF.Http.CommonApiApi
{
    public class CheckConnectionController
    {
        const string echo = "Hello from NPA Xamarin Common Library";

        private CommonApiClient _client { get; }

        internal CheckConnectionController( CommonApiClient client )
        {
            _client = client;
        }

        public async Task<bool> CheckConnection( CancellationToken cancellationToken = default( CancellationToken ) ) =>
            await CheckGetConnection( cancellationToken ) && await CheckPostConnection( cancellationToken );

        public async Task<bool> CheckGetConnection( CancellationToken cancellationToken = default( CancellationToken ) )
        {
            try
            {
                var response = await _client.GetAsync<ConnectionCheck>( $"Api/CheckConnection/{WebUtility.UrlEncode( echo )}" );
                return response.Connected && response.Echo == echo;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CheckPostConnection( CancellationToken cancellationToken = default( CancellationToken ) )
        {
            try
            {
                var responseMessage = await _client.PostAsync( "Api/CheckConnection", new { Echo = echo }, cancellationToken );
                var response = JsonConvert.DeserializeObject<ConnectionCheck>( await responseMessage.Content.ReadAsStringAsync() );

                return response.Connected && response.Echo == echo;
            }
            catch
            {
                return false;
            }
        }

        private class ConnectionCheck
        {
            public bool Connected { get; set; }
            public string Echo { get; set; }
        }
    }
}
