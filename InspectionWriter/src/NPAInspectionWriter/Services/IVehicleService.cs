using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NPAInspectionWriter.Models;

namespace NPAInspectionWriter.Services
{
    public interface IVehicleService
    {
        Task<List<Vehicle>> SearchAsync(VehicleSearchRequest searchRequest);

        Task<IEnumerable<InspectionMaster>> GetMastersForVehicleAsync( Vehicle vehicle );

        Task<InspectionTypesAndMasters> GetAvailableInspectionTypesAndMastersAsync( Vehicle vehicle );

        Task<IDictionary<string, IEnumerable<Inspection>>> GetInspectionsAsync( Vehicle vehicle );

        Task<Guid> GetPrimaryPictureIdAsync( Vehicle vehicle );

        Task<string> GetPrimaryPictureAsync( Vehicle vehicle );

        Task<IEnumerable<string>> GetVehicleAlertsAsync( Vehicle vehicle );

        Task<IEnumerable<string>> GetVinAlertsAsync( Vehicle vehicle );
    }
}
