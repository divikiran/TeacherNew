using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NPAInspectionWriter.Models;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.iOS.Models;
using NPAInspectionWriter.AppData;
using SQLite.Net.Attributes;
using Xamarin.Forms;

namespace NPAInspectionWriter.Models
{
    public class LocalInspection :  IDatabaseRecord
    {

        public static implicit operator Guid(LocalInspection inspection) => inspection?.InspectionId ?? default(Guid);

        [PrimaryKey]
        public Guid InspectionId { get; set; }

        //[ForeignKey( typeof( VehicleRecord ) )]
        public Guid VehicleId { get; set; }

        public string ContextAppVersion { get; set; }

        public string ContextiOSVersion { get; set; }

        public string ContextiPadModel { get; set; }

        public string ContextUsername { get; set; }

        public string ContextLocation { get; set; }

        public string InspectionValue { get; set; }

        public Guid? LocationId { get; set; }

        private string _location;
        [Ignore]
        public string Location
        {
            get
            {
                if( LocationId.HasValue && string.IsNullOrWhiteSpace( _location ) )
                {
                    _location = NPAConstants.LookupLocationCodeById( ( Guid )LocationId );
                }

                return _location;
            }
        }

        //[ForeignKey( typeof( LocalInspectionMaster ) )]
        //public int MasterId { get; set; }

        public Guid MasterId { get; set; }

        public string MasterDisplayName { get; set; }

        public string InspectionType { get; set; }

        //[ForeignKey( typeof( InspectionTypeRecord ) )]
        public int InspectionTypeId { get; set; }

        public string InspectionUser { get; set; }

        public Guid InspectionUserId { get; set; }

        public int? Score { get; set; }

        public string Comments { get; set; }

        public DateTime InspectionDate { get; set; }

        public bool OpenRepair { get; set; }

        public string RepairComments { get; set; }

        public string NewColor { get; set; }

        public string NewMileage { get; set; }

        public string NewVIN { get; set; }

        public string NewVehicleModelId { get; set; }

        public int? ElapsedTime { get; set; }

        private bool _allowEditing;
        public bool AllowEditing
        {
            get
            {
                if( IsLocalInspection ) return true;

                return _allowEditing;
            }

            set { _allowEditing = value; }
        }

        public string InspectionMilesHours { get; set; }

        public string InspectionColor { get; set; }

        //[ManyToOne]
        [Ignore]
        public VehicleRecord Vehicle { get; set; }

        //[OneToOne]
        [Ignore]
        public LocalInspectionMaster Master { get; set; }

        //[ManyToOne]
        [Ignore]
        public InspectionTypeRecord Type { get; set; }

        [Ignore]
        public List<Picture> Pictures { get; set; } = new List<Picture>();

        public DateTime LastSync { get; set; } = DateTime.Now.ToLocalTime();

        private string stockNumber;
        public string StockNumber
        {
            get { return stockNumber; }
            set
            {
                if( Regex.IsMatch( value, @"\d+\-\d+" ) )
                {
                    stockNumber = value.Split( '-' )[ 1 ];
                }
                else
                {
                    stockNumber = value;
                }
            }
        }

        [Ignore]
        public string YearMake
        {
            get
            {
                return Vehicle != null ? $"{Vehicle.Year} {Vehicle.Brand}" : string.Empty;
            }
        }

        [Ignore]
        public string Model { get { return Vehicle?.Model ?? ""; } }

        public bool IsLocalInspection { get; set; }

        public bool FailedUpload { get; set; }

        public bool WasEdited { get; set; }

        [Ignore]
        public string GroupHeader
        {
            get
            {
                if( IsLocalInspection )
                {
                    return AppResources.LocalInspectionGrouping;
                }
                else
                {
                    return string.Format( AppResources.InspectionStockNumberGrouping, StockNumber );
                }
            }
        }

        [Ignore]
        public bool IsPendingUpload
        {
            get
            {
                // If any required categories are NOT done, we are not pending upload. TODO:Jarid uncomment
                return true; //!Master?.Categories?.Where( x => x.Required ).Any( c => c.IsDone == false ) ?? false;
            }
        }

        public static implicit operator LocalInspection( Inspection inspection )
        {
            return new LocalInspection()
            {
                AllowEditing = inspection.AllowEditing,
                Comments = inspection.Comments,
                ContextAppVersion = inspection.ContextAppVersion,
                ContextiOSVersion = inspection.ContextiOSVersion,
                ContextiPadModel = inspection.ContextiPadModel,
                ContextLocation = inspection.ContextLocation,
                ContextUsername = inspection.ContextUsername,
                ElapsedTime = inspection.ElapsedTime,
                FailedUpload = false,
                InspectionColor = inspection.InspectionColor,
                InspectionDate = inspection.InspectionDate,
                InspectionId = inspection.InspectionId,
                InspectionMilesHours = inspection.InspectionMilesHours,
                InspectionType = inspection.InspectionType,
                InspectionTypeId = inspection.InspectionTypeId,
                InspectionUser = inspection.InspectionUser,
                InspectionUserId = inspection.InspectionUserId,
                InspectionValue = inspection.InspectionValue,
                IsLocalInspection = false,
                LastSync = DateTime.Now,
                LocationId = inspection.LocationId,
                //Master
                //MasterId
                MasterId = inspection.MasterId,
                MasterDisplayName = inspection.MasterDisplayName,
                NewColor = inspection.NewColor,
                NewMileage = inspection.NewMileage,
                NewVehicleModelId = inspection.NewVehicleModelId,
                NewVIN = inspection.NewVIN,
                OpenRepair = inspection.OpenRepair,
                RepairComments = inspection.RepairComments,
                Score = inspection.Score,
                //Type
                VehicleId = inspection.VehicleId
            };
        }

        public static implicit operator Inspection( LocalInspection localInspection )
        {
            if( localInspection == null ) return null;

            var insp = new Inspection()
            {
                ContextAppVersion = localInspection.ContextAppVersion,
                ContextiOSVersion = localInspection.ContextiOSVersion,
                ContextiPadModel = localInspection.ContextiPadModel,
                ContextUsername = localInspection.ContextUsername,
                ContextLocation = localInspection.ContextLocation,
                InspectionId = localInspection.InspectionId,
                InspectionValue = localInspection.InspectionValue,
                VehicleId = localInspection.VehicleId,
                LocationId = localInspection.LocationId,
                MasterId = localInspection.MasterId,
                MasterDisplayName = localInspection.MasterDisplayName,
                InspectionType = localInspection.InspectionType,
                InspectionTypeId = localInspection.InspectionTypeId,
                InspectionUser = localInspection.InspectionUser,
                InspectionUserId = localInspection.InspectionUserId,
                Score = localInspection.Score,
                Comments = localInspection.Comments,
                InspectionDate = localInspection.InspectionDate,
                //InspectionImages = localInspection.Pictures,
                OpenRepair = localInspection.OpenRepair,
                RepairComments = localInspection.RepairComments,
                NewColor = localInspection.NewColor,
                NewMileage = localInspection.NewMileage,
                NewVIN = localInspection.NewVIN,
                NewVehicleModelId = localInspection.NewVehicleModelId,
                ElapsedTime = localInspection.ElapsedTime,
                AllowEditing = localInspection.AllowEditing,
                InspectionMilesHours = localInspection.InspectionMilesHours,
                InspectionColor = localInspection.InspectionColor
            };

            insp.InspectionItems = new List<InspectionItem>();
            foreach(var c in localInspection.Master.Categories)
            {
                insp.InspectionItems.Add(new InspectionItem()
                {
                    CategoryId = c.CategoryId,
                    ItemComments = c.Notes,
                    ItemScore = c.Value,
                    OptionId = c.OptionId
                });
            }

            return insp;
        }

        public int SetScore()
        {
            if(Master == null || Master.Categories.Count == 0) { return 0; }
            int val = 0;
            double tmpVal = 0;
            double totScore = 0;
            double totWeight = 0;

            try
            {
                var populatedCats = Master.Categories.Where(x => x.OptionId != Guid.Empty).ToArray();
                foreach (var c in populatedCats)
                {
                    tmpVal = ((double)c.Value / (double)c.MaxScore) * (double)c.Weight;
                    totScore += tmpVal;
                    totWeight += c.Weight;
                }
                totScore /= totWeight;
                val = (int)Math.Round( totScore * this.Master.MaxInspectionScore);
            }
            catch (Exception ex)
            {
                NPAInspectionWriter.Logging.StaticLogger.ExceptionLogger(ex);
            }

            this.Score = val;
            return val;
        }

		[Ignore]
		public ImageSource UploadButtonSource
		{
			get
			{
                if (IsLocalInspection)
                    return ImageSource.FromFile("Icons/upload-blue-34x46.png");
                else
                    return "";
			}
		}

		
    }
}
