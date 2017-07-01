using System;
using NPAInspectionWriter.iOS.Models;
using SQLite.Net.Attributes;

namespace NPAInspectionWriter.Models
{
    public class VehicleAlertRecord : IDatabaseRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        //[ForeignKey( typeof( VehicleRecord ) )]
        public Guid VehicleRecordId { get; set; }

        [NotNull]
        public string Alert { get; set; }

        //[ManyToOne]
        //public VehicleRecord Vehicle { get; set; }

        [NotNull]
        public DateTime LastSync { get; set; }
    }
}
