using System;
using SQLite.Net.Attributes;

namespace NPAInspectionWriter.Models
{
    public class VehicleRecordAvailableInspectionTypeRecord
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public Guid VehicleId { get; set; }

        public int InspectionTypeId { get; set; }
    }
}
