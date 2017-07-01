using NPAInspectionWriter.AppData;

namespace NPAInspectionWriter.Helpers
{
    public class VehicleNotFoundException : ObjectNotFoundException
    {
        public VehicleNotFoundException() : base( AppResources.VehicleNotFound ) { }

        public VehicleNotFoundException( string query, SearchType searchType ) :
            base( string.Format( AppMessages.VehicleNotFoundSearchMessage, searchType, query ) ) { }
    }
}
