using System;
using NPAInspectionWriter.iOS.Models;
using SQLite.Net.Attributes;

namespace NPAInspectionWriter.Models
{
    public class VinAlertRecord : IDatabaseRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        //[ForeignKey( typeof( VehicleRecord ) )]
        public Guid VehicleId { get; set; }

        [NotNull]
        public string Alert { get; set; }

        public DateTime LastSync { get; set; } = DateTime.Now;

        //[ManyToOne]
        //public VehicleRecord Vehicle { get; set; }
    }
}
