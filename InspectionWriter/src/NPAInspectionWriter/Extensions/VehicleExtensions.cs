using System.Collections.Generic;
using System.Linq;
using NPAInspectionWriter.Models;
using NPAInspectionWriter.iOS.Models;
using NPAInspectionWriter.Models;

namespace NPAInspectionWriter.Extensions
{
    public static class VehicleExtensions
    {
        public static VehicleDetail GetVehicleDetails( this VehicleRecord vehicle )
        {
            if( vehicle == null ) return null;

            return new VehicleDetail()
            {
                AllowNewInspections = vehicle.AllowNewInspections,
                Color = vehicle.Color,
                DefaultImageSource = vehicle.GetVehicleImageFromUrl(),
                Make = vehicle.Brand,
                MilesHours = vehicle.MilesHours,
                Model = vehicle.Model,
                SalesRep = vehicle.SalesRep,
                Seller = vehicle.Seller,
                StockNumber = vehicle.StockNumber,
                VehicleState = vehicle.VehicleState,
                VIN = vehicle.Vin,
                Year = vehicle.Year,
            };
        }

        public static VehicleDetail GetVehicleDetails( this Vehicle vehicle )
        {
            if( vehicle == null ) return null;

            return new VehicleDetail()
            {
                AllowNewInspections = vehicle.AllowNewInspections,
                Color = vehicle.Color,
                DefaultImageSource = vehicle.GetVehicleImageFromUrl(),
                Make = vehicle.Brand,
                MilesHours = vehicle.MilesHours,
                Model = vehicle.Model,
                SalesRep = vehicle.SalesRep,
                Seller = vehicle.Seller,
                StockNumber = vehicle.StockNumber,
                VehicleState = vehicle.VehicleState,
                VIN = vehicle.Vin,
                Year = vehicle.Year,
            };
        }

        public static void AddOrUpdateInspections( this IList<LocalInspection> existingInspections, IEnumerable<LocalInspection> newInspections )
        {
            if( existingInspections == null || newInspections == null ||
                existingInspections.Count == 0 || newInspections.Count() == 0 ) return;

            foreach( var inspection in newInspections )
            {
                if( !existingInspections.Any( x => x.InspectionId == inspection.InspectionId ) )
                {
                    existingInspections.Add( inspection );
                }

            }
        }

        // Don't this we need this. It can probably be removed later.
        //public static void SetVehicle( this Vehicle objA, Vehicle objB )
        //{
        //    objA.AllowNewInspections = objB.AllowNewInspections;
        //    objA.AuctioneerNotes = objB.AuctioneerNotes;
        //    objA.AvailableInspections = objB.AvailableInspections;
        //    objA.AvailableInspectionTypes = objB.AvailableInspectionTypes;
        //    objA.AvailableMasters = objB.AvailableMasters;
        //    objA.Borrower = objB.Borrower;
        //    objA.Brand = objB.Brand;
        //    objA.Color = objB.Color;
        //    objA.Id = objB.Id;
        //    objA.IsFactoryUnit = objB.IsFactoryUnit;
        //    objA.LocationId = objB.LocationId;
        //    objA.MilesHours = objB.MilesHours;
        //    objA.Model = objB.Model;
        //    objA.NoBattery = objB.NoBattery;
        //    objA.PictureId = objB.PictureId;
        //}
    }
}
