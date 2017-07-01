#if USE_MOCKS
using System;
using System.Threading.Tasks;

namespace NPAInspectionWriter.Services
{
#pragma warning disable 1998
    public class ImageUploadServiceMock : IImageUploadService
    {
        public async Task<bool> ReplaceInspectionImageAsync( Guid inspectionImageId, byte[] image )
        {
            return true;
        }

        public async Task<bool> UploadImageAsync( byte[] image )
        {
            return true;
        }
    }
}
#endif