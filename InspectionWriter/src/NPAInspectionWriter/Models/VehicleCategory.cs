using System;
using Newtonsoft.Json;

namespace NPAInspectionWriter.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class VehicleCategory
    {
        private NPA.CodeGen.VehicleCategory _vehicleCategory;

        private VehicleCategory() { }

        public VehicleCategory(Guid? categoryId)
        {
            if (categoryId.HasValue)
                _vehicleCategory = NPA.CodeGen.VehicleCategory.FromGuid(categoryId.Value);
            else
                _vehicleCategory = NPA.CodeGen.VehicleCategory.UNKNOWN;
        }

        public Guid VehicleCategoryId
        {
            get
            {
                return _vehicleCategory.VehicleCategoryId;
            }
            set { }
        }

        private string name;
        [JsonProperty(PropertyName = "displayName")]
        public string Name
        {
            get
            {
                if( _vehicleCategory == null && !string.IsNullOrWhiteSpace( name ) ) return name;

                return _vehicleCategory != null
                           ? _vehicleCategory.DisplayName
                           : NPA.CodeGen.VehicleCategory.UNKNOWN.DisplayName;
            }
            set { name = value; }
        }

        public int VinTypeId
        {
            get
            {
                return _vehicleCategory != null
                    ? _vehicleCategory.VinType.VINTypeId
                    : NPA.CodeGen.VehicleCategory.UNKNOWN.VinType.VINTypeId;
            }
            set { }
        }
    }
}
