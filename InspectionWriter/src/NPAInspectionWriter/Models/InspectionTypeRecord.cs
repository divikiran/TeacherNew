using System;
using System.Collections.Generic;
using NPAInspectionWriter.iOS.Models;
using SQLite.Net.Attributes;

namespace NPAInspectionWriter.Models
{
    public class InspectionTypeRecord : IDatabaseRecord
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public DateTime LastSync { get; set; } = DateTime.Now;

        //[OneToMany( CascadeOperations = CascadeOperation.CascadeRead )]
        //public List<LocalInspection> Inspections { get; set; }

        //[ManyToMany( typeof( VehicleRecord ), CascadeOperations = CascadeOperation.CascadeRead )]
        //public List<VehicleRecord> Vehicles { get; set; }
    }
}
