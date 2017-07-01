using NPAInspectionWriter.Models;
using NPAInspectionWriter.iOS.Extensions;
using NPAInspectionWriter.Helpers;
using System.Threading.Tasks;
using System;
using NPAInspectionWriter.Extensions;

namespace NPAInspectionWriter.Services
{
    public class LogService : ILogService
    {
        InspectionWriterClient _client { get; }

        public LogService( InspectionWriterClient client )
        {
            _client = client;
        }

        public async Task<bool> LogErrorAsync( Error err )
        {
            return ( await _client.PostJsonObjectAsync( "log/error", err ) ).IsSuccessStatusCode;
        }

        public async void Log( string message)
        {
            var err = new Error()
            {
                Message = message
            };

            await LogErrorAsync( err );
        }
    }
}
