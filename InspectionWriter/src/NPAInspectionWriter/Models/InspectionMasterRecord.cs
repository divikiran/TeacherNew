using System;
using SQLite.Net.Attributes;

namespace NPAInspectionWriter.Models
{
    public class InspectionMasterRecord
    {
        [PrimaryKey]
        public Guid MasterId { get; set; }

        public string DisplayName { get; set; }

        public int MaxInspectionScore { get; set; }
    }
}
