using System;
using SQLite.Net.Attributes;

namespace NPAInspectionWriter.Models
{
    public class VehicleRecordAvailableInspectionMasterRecord
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public Guid VehicleId { get; set; }

        public Guid MasterId { get; set; }

        public override bool Equals( object obj )
        {
            var record = obj as VehicleRecordAvailableInspectionMasterRecord;

            return record != null &&
                VehicleId == record.VehicleId &&
                MasterId == record.MasterId;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
