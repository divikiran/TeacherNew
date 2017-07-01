using System;
using System.Collections.Generic;
using System.Linq;
using NPAInspectionWriter.Models;
using NPAInspectionWriter.iOS.Models;
using SQLite.Net.Attributes;

namespace NPAInspectionWriter.Models
{
    public class VehicleRecord : IDatabaseRecord
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [Unique]
        public string Vin { get; set; }

        [Unique]
        public string StockNumber { get; set; }

        public int? Year { get; set; }

        public string Brand { get; set; }

        public string Color { get; set; }

        public string Model { get; set; }

        public string VehicleModelId { get; set; }

        public string VehicleCategoryId { get; set; }

        public bool VinCheckExclude { get; set; }

        public string MilesHours { get; set; }

        public bool NoBattery { get; set; }

        public Guid? PictureId { get; set; }

        public string PrimaryPictureUrl { get; set; }

        public string VehicleComments { get; set; }

        public string PublicAuctionNotes { get; set; }

        public string AuctioneerNotes { get; set; }

        public bool RepairExists { get; set; }

        public string RepairComments { get; set; }

        public string VehicleState { get; set; }

        public bool IsFactoryUnit { get; set; }

        public string Seller { get; set; }

        public string Borrower { get; set; }

        public string SalesRep { get; set; }

        public string SalesRepEmail { get; set; }

        public Guid? LocationId { get; set; }

        private bool _allowNewInspections;
        public bool AllowNewInspections
        {
            get
            {
                return AvailableInspections?.Count > 0 ?
                    !AvailableInspections.Any( x => x.IsLocalInspection ) :
                    _allowNewInspections;

            }
            set { _allowNewInspections = value; }
        }

        public string Size { get; set; }

        //[OneToMany( CascadeOperations = CascadeOperation.All )]
        [Ignore]
        public List<LocalInspection> AvailableInspections { get; set; } = new List<LocalInspection>();

        //[OneToMany( CascadeOperations = CascadeOperation.All )]
        [Ignore]
        public List<VehicleAlertRecord> VehicleAlerts { get; set; } = new List<VehicleAlertRecord>();

        //[OneToMany( CascadeOperations = CascadeOperation.All )]
        [Ignore]
        public List<VinAlertRecord> VinAlerts { get; set; } = new List<VinAlertRecord>();

        public DateTime LastSync { get; set; } = DateTime.Now.ToLocalTime();

        //[ManyToOne( CascadeOperations = CascadeOperation.All )]
        [Ignore]
        public List<InspectionTypeRecord> AvailableInspectionTypes { get; set; } = new List<InspectionTypeRecord>();

        //[ManyToOne( CascadeOperations = CascadeOperation.All )]
        [Ignore]
        public List<InspectionMasterRecord> AvailableMasters { get; set; }

        [Ignore]
        public int CommentCount
        {
            get
            {
                int temp = 0;
                if( !string.IsNullOrWhiteSpace( RepairComments ) ) temp++;
                if( !string.IsNullOrWhiteSpace( VehicleComments ) ) temp++;
                if( !string.IsNullOrWhiteSpace( AuctioneerNotes ) ) temp++;
                if( !string.IsNullOrWhiteSpace( PublicAuctionNotes ) ) temp++;

                return temp;
            }
        }

        public override bool Equals( object obj )
        {
            if( obj.GetType() == typeof( VehicleRecord ) )
            {
                var vehB = obj as VehicleRecord;
                return Id == vehB.Id || Vin == vehB.Vin ||
                    StockNumber == vehB.StockNumber;
            }
            else if( obj.GetType() == typeof( Vehicle ) )
            {
                var vehB = obj as Vehicle;
                return Id == vehB.Id || Vin == vehB.Vin ||
                    StockNumber == vehB.StockNumber;
            }
            return false;
        }

        public override int GetHashCode()
        {
            // Quiets Build Warning CS0659
            return base.GetHashCode();
        }

        public static implicit operator VehicleRecord( Vehicle vehicle )
        {
            return new VehicleRecord()
            {
                Id = vehicle.Id,
                Vin = vehicle.Vin,
                StockNumber = vehicle.StockNumber,
                Year = vehicle.Year,
                Brand = vehicle.Brand,
                Color = vehicle.Color,
                Model = vehicle.Model,
                VehicleModelId = vehicle.VehicleModelId,
                VehicleCategoryId = vehicle.VehicleCategoryId,
                VinCheckExclude = vehicle.VinCheckExclude,
                MilesHours = vehicle.MilesHours,
                NoBattery = vehicle.NoBattery,
                PictureId = vehicle.PictureId,
                PrimaryPictureUrl = vehicle.PrimaryPictureUrl,
                VehicleComments = vehicle.VehicleComments,
                PublicAuctionNotes = vehicle.PublicAuctionNotes,
                AuctioneerNotes = vehicle.AuctioneerNotes,
                RepairExists = vehicle.RepairExists,
                RepairComments = vehicle.RepairComments,
                VehicleState = vehicle.VehicleState,
                IsFactoryUnit = vehicle.IsFactoryUnit,
                Seller = vehicle.Seller,
                Borrower = vehicle.Borrower,
                SalesRep = vehicle.SalesRep,
                SalesRepEmail = vehicle.SalesRepEmail,
                LocationId = vehicle.LocationId,
                AllowNewInspections = vehicle.AllowNewInspections,
                Size = vehicle.Size,
                //AvailableInspections = vehicle.AvailableInspections,
                //AvailableInspectionTypes = vehicle.AvailableInspectionTypes,
                //AvailableMasters = vehicle.AvailableMasters,
            };
        }

        public static implicit operator Vehicle( VehicleRecord vehicle )
        {
            return new Vehicle()
            {
                Id = vehicle.Id,
                Vin = vehicle.Vin,
                StockNumber = vehicle.StockNumber,
                Year = vehicle.Year,
                Brand = vehicle.Brand,
                Color = vehicle.Color,
                Model = vehicle.Model,
                VehicleModelId = vehicle.VehicleModelId,
                VehicleCategoryId = vehicle.VehicleCategoryId,
                VinCheckExclude = vehicle.VinCheckExclude,
                MilesHours = vehicle.MilesHours,
                NoBattery = vehicle.NoBattery,
                PictureId = vehicle.PictureId,
                VehicleComments = vehicle.VehicleComments,
                PublicAuctionNotes = vehicle.PublicAuctionNotes,
                AuctioneerNotes = vehicle.AuctioneerNotes,
                RepairExists = vehicle.RepairExists,
                RepairComments = vehicle.RepairComments,
                VehicleState = vehicle.VehicleState,
                IsFactoryUnit = vehicle.IsFactoryUnit,
                Seller = vehicle.Seller,
                Borrower = vehicle.Borrower,
                SalesRep = vehicle.SalesRep,
                SalesRepEmail = vehicle.SalesRepEmail,
                LocationId = vehicle.LocationId,
                AllowNewInspections = vehicle.AllowNewInspections,
                Size = vehicle.Size,
                //AvailableInspections = vehicle.AvailableInspections,
                //AvailableInspectionTypes = vehicle.AvailableInspectionTypes,
                //AvailableMasters = vehicle.AvailableMasters,
            };
        }

        public static implicit operator Guid( VehicleRecord vehicle ) => vehicle.Id;
    }
}
