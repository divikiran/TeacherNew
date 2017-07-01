using System.Diagnostics;
using System.Threading.Tasks;
using NPAInspectionWriter.Models;
using NPAInspectionWriter.iOS.Extensions;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.Extensions;

namespace NPAInspectionWriter.Services
{
    public class EmailService : IEmailService
    {
        InspectionWriterClient _client { get; }

        public EmailService( InspectionWriterClient client)
        {
            _client = client;
        }

        public async Task<bool> SendEmailAsync( Message message )
        {
            var result = await _client.PostJsonObjectAsync( "message/email", message );

            if( !result.IsSuccessStatusCode )
               Debug.WriteLine( result );

            return result.IsSuccessStatusCode;
        }
    }
}
