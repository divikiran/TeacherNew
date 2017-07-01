using System;
using System.Collections.Generic;
using System.Linq;
using NPAInspectionWriter.Models;
using SQLite.Net.Attributes;

namespace NPAInspectionWriter.Models
{
    public class LocalInspectionMaster
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }

        public Guid MasterId { get; set; }

        public string DisplayName { get; set; }

        public int MaxInspectionScore { get; set; }

        public Guid InspectionId { get; set; }

        //[Ignore]
        //public LocalInspection Inspection { get; set; }

        [Ignore]
        public List<LocalInspectionCategory> Categories { get; set; } = new List<LocalInspectionCategory>();

        public static implicit operator LocalInspectionMaster( InspectionMaster master )
        {
            var localMaster = new LocalInspectionMaster()
            {
                MasterId = master.MasterId,
                DisplayName = master.DisplayName,
                //InspectionId =
                MaxInspectionScore = master.MaxInspectionScore
            };



            foreach ( var category in master.Categories )
            {
                var option = category.Options.FirstOrDefault( x => x.OptionId == category.CurrentValue?.OptionId );

                localMaster.Categories.Add( new LocalInspectionCategory()
                {
                    CategoryId = category.CategoryId,
                    Notes = category.Comments,
                    CategoryDisplayName = category.DisplayName,
                    MaxScore = category.MaxScore,
                    OptionDisplayName = option?.DisplayName,
                    OptionId = option?.OptionId ?? default( Guid ),
                    OptionsJson = Newtonsoft.Json.JsonConvert.SerializeObject( category.Options ),
                    PreviousValue = category.Options.FirstOrDefault( x => x.OptionId == category.PreviousValue?.OptionId )?.DisplayName,
                    Required = category.Required,
                    Value = option?.Value ?? 0,
                    Weight = category.Weight
                } );
            }

            return localMaster;
        }
    }
}
