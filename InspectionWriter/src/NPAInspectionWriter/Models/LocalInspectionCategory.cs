using System;
using NPAInspectionWriter.AppData;
using SQLite.Net.Attributes;

namespace NPAInspectionWriter.Models
{
    public class LocalInspectionCategory
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }

        public Guid CategoryId { get; set; }

        public string CategoryDisplayName { get; set; }

        public int Weight { get; set; }

        public int MaxScore { get; set; }

        public bool Required { get; set; }

        public string OptionsJson { get; set; }

        public Guid OptionId { get; set; }

        public string OptionDisplayName { get; set; }

        public int Value { get; set; }

        public string Notes { get; set; }

        //[ForeignKey( typeof( LocalInspectionMaster ) )]
        public int LocalInspectionMasterId { get; set; }

        //[ManyToOne]
        [Ignore]
        public LocalInspectionMaster InspectionMaster { get; set; }

        [Ignore]
        public string DisplayName
        {
            get
            {
                return string.IsNullOrWhiteSpace( OptionDisplayName ) ?
                    $"{CategoryDisplayName} - {AppResources.UnratedInspectionOption}" :
                    $"{CategoryDisplayName} - {OptionDisplayName}";
            }
        }

        [Ignore]
        public bool IsDone
        {
            get { return !string.IsNullOrWhiteSpace( OptionDisplayName ); }
        }

        public string PreviousValue { get; set; }
    }
}
