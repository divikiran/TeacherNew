using System.Threading;
using System.Threading.Tasks;
using NPAInspectionWriter.Logging;

namespace NPA.XF.Http.CommonApiApi
{
    public class VinController
    {
        private CommonApiClient _client { get; }
        private ILog _logger { get; }

        internal VinController( CommonApiClient client )
        {
            _client = client;
            _logger = _client._options.Logger;
        }

        public async Task<bool> IsVinValid( string vin, CancellationToken cancellationToken = default( CancellationToken ) )
        {
            var result = await _client.GetAsync( $"Api/Vin/{vin}/IsValidVin", cancellationToken );

            if( !result.IsSuccessStatusCode ) return false;

            return bool.Parse( await result.Content.ReadAsStringAsync() );
        }
    }
}
