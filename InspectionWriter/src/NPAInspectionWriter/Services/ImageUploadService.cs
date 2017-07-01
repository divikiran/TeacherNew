using System;
using System.Threading.Tasks;
using NPAInspectionWriter.Helpers;

namespace NPAInspectionWriter.Services
{
#pragma warning disable 1998
    public class ImageUploadService : IImageUploadService
    {
        InspectionWriterClient _client { get; }

        public ImageUploadService( InspectionWriterClient client )
        {
            _client = client;
        }

        public async Task<bool> ReplaceInspectionImageAsync( Guid inspectionImageId, byte[] image )
        {
            await _client.PostAsync( $"upload/inspectionImage/{inspectionImageId}", null );
            return false;
        }

        public async Task<bool> UploadImageAsync( byte[] image )
        {
            return false;
        }
    }
}
