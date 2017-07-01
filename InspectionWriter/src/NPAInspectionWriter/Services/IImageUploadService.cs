using System;
using System.Threading.Tasks;

namespace NPAInspectionWriter.Services
{
    public interface IImageUploadService
    {
        Task<bool> ReplaceInspectionImageAsync( Guid inspectionImageId, byte[] image );

        Task<bool> UploadImageAsync( byte[] image );
    }
}
