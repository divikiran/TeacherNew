using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NPAInspectionWriter.Models;

namespace NPAInspectionWriter.Services
{
    public interface IInspectionsService
    {
        Task<IEnumerable<InspectionMaster>> GetMastersAsync( Inspection inspection );

        Task<InspectionTypesAndMasters> GetMastersAndTypesAsync( Inspection inspection );

        Task<InspectionTypesAndMasters> GetMasterAndTypeAsync(int inspectionTypeId, Guid masterId);

        Task<InspectionMaster> GetMasterAsync( Guid masterId );

        Task<InspectionMaster> GetMasterWithPreviousValueAsync( Guid vehicleId, Guid masterId, int numberOfDays );

        Task<InspectionMaster> GetMasterForInspectionWithSelectedValueAsync( Guid inspectionId );

        Task<System.Net.Http.HttpResponseMessage> SaveNewInspectionAsync( Inspection inspection );

        Task<Inspection> GetInspectionAsync( Guid inspectionId );

        Task<IEnumerable<Inspection>> GetInspectionsAsync( InspectionSearchRequest searchRequest );

        Task<IEnumerable<InspectionItem>> GetInspectionItemsAsync( Inspection inspection );

        Task<InspectionType> GetInspectionTypeAsync( Inspection inspection );

        Task<IEnumerable<InspectionImage>> GetInspectionImagesFromInspectionAsync( LocalInspection inspection );

        Task<IEnumerable<InspectionImage>> GetInspectionImagesAsync(Guid inspectionId);

        // TODO: Determine proper object to return here.
        Task<IEnumerable<string>> ImageUploadSuccessAsync( Inspection inspection, IEnumerable<InspectionImage> images );

        Task<string> GetAmsSettingAsync( string setting );

        Task<string> GetAmsSettingAsync( string setting, Guid locationId );
    }
}
