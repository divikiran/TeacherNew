using System;
using System.Collections.Generic;
using System.Linq;
using NPAInspectionWriter.Models;
using Newtonsoft.Json;

namespace NPAInspectionWriter.Extensions
{
    public static class ApiToLocalModelExtensions
    {
        public static OfflineUserAuthentication ToOfflineAuthModel( this AmsUser user, string password )
        {
            return new OfflineUserAuthentication()
            {
                AccountLevel = user.UserLevel,
                CanCreateCr = user.CanCreateInspections,
                Email = user.EmailAddress,
                Location = user.Location,
                LocationId = user.LocationId,
                Password = password,
                UserAccountId = user.UserAccountId,
                UserName = user.UserName
            };
        }

        public static IEnumerable<LocalInspection> ToLocalModelCollection( this IDictionary<string, IEnumerable<Inspection>> inspections )
        {
            var list = new List<LocalInspection>();
            foreach( KeyValuePair<string,IEnumerable<Inspection>> record in inspections )
            {
                foreach( Inspection inspection in record.Value )
                {
                    LocalInspection local = inspection;
                    local.StockNumber = record.Key;
                    list.Add( local );
                }
            }

            return list;
        }

        public static List<VehicleAlertRecord> ToLocalVehicleAlertCollection( this IEnumerable<string> alerts )
        {
            var list = new List<VehicleAlertRecord>();

            foreach( string alert in alerts )
            {
                list.Add( new VehicleAlertRecord()
                {
                    Alert = alert
                } );
            }

            return list;
        }

        public static List<VinAlertRecord> ToLocalVinAlertCollection( this IEnumerable<string> alerts )
        {
            var list = new List<VinAlertRecord>();

            foreach( string alert in alerts )
            {
                list.Add( new VinAlertRecord()
                {
                    Alert = alert
                } );
            }

            return list;
        }

        public static LocalInspectionMaster ToLocalModel( this InspectionMaster master, LocalInspection inspection )
        {

            var localMaster = new LocalInspectionMaster()
            {
                DisplayName = master.DisplayName,
                //Inspection = inspection,
                InspectionId = inspection.InspectionId,
                MasterId = master.MasterId,
                MaxInspectionScore = master.MaxInspectionScore
            };

            if( master?.Categories?.Count > 0 )
            {
                localMaster.Categories = master.Categories.ToLocalModelCollection(localMaster, true);
            }

            return localMaster;
        }

        public static LocalInspectionCategory ToLocalModel( this InspectionCategory category, LocalInspectionMaster master, string previousValue = null )
        {
            return new LocalInspectionCategory()
            {
                CategoryDisplayName = category.DisplayName,
                CategoryId = category.CategoryId,
                InspectionMaster = master,
                MaxScore = category.MaxScore,
                Required = category.Required,
                Weight = category.Weight,
                OptionsJson = JsonConvert.SerializeObject(category.Options),
                OptionDisplayName = category.CurrentValue != null ? category.CurrentValue.DisplayName : string.Empty,
                //OptionId = default(Guid),
                OptionId = category.CurrentValue != null ? category.CurrentValue.OptionId : default(Guid),
                Value = 0, // category.MaxScore,
                PreviousValue = previousValue,
                Notes = category.Comments
            };
        }

        public static List<LocalInspectionCategory> ToLocalModelCollection( this IEnumerable<InspectionCategory> categories, LocalInspectionMaster master, bool isCurrentInspection = false )
        {
            var list = new List<LocalInspectionCategory>();
            foreach( InspectionCategory item in categories )
            {
                InspectionOption previousOption = null;
                if( item.PreviousValue != null && isCurrentInspection )
                    previousOption = item.Options.FirstOrDefault( x => x.OptionId == item.PreviousValue.OptionId );

                list.Add( item.ToLocalModel( master, previousOption?.DisplayName ) );
            }
            return list;
        }
    }
}
