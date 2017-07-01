using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace InspectionWriterWebApi.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Vehicle
    {
        [JsonProperty(PropertyName = "vehicleId", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "vin", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Vin { get; set; }

        [JsonProperty(PropertyName = "stockNumber", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string StockNumber { get; set; }

        [JsonProperty(PropertyName = "year", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Year { get; set; }

        [JsonProperty(PropertyName = "brand", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Brand { get; set; }

        [JsonProperty(PropertyName = "color", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "model", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Model { get; set; }

        [JsonProperty(PropertyName = "vehicleModelId", NullValueHandling = NullValueHandling.Ignore,
           DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string VehicleModelId { get; set; }

        [JsonProperty(PropertyName = "vehicleCategoryId", NullValueHandling = NullValueHandling.Ignore,
           DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string VehicleCategoryId { get; set; }

        [JsonProperty(PropertyName = "vinCheckExclude", NullValueHandling = NullValueHandling.Ignore,
           DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool VinCheckExclude { get; set; }

        [JsonProperty(PropertyName = "milesHours", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string MilesHours { get; set; }

        [JsonProperty(PropertyName = "noBattery", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool NoBattery { get; set; }

        [JsonProperty(PropertyName = "pictureId", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? PictureId { get; set; }

        [JsonProperty(PropertyName = "primaryPictureUrl", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string PrimaryPictureUrl { get; set; }

        [JsonProperty(PropertyName = "vehicleComments", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string VehicleComments { get; set; }

        [JsonProperty(PropertyName = "publicAuctionNotes", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string PublicAuctionNotes { get; set; }

        [JsonProperty(PropertyName = "auctioneerNotes", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string AuctioneerNotes { get; set; }

        [JsonProperty(PropertyName = "repairExists", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool RepairExists { get; set; }

        [JsonProperty(PropertyName = "repairComments", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string RepairComments { get; set; }

        [JsonProperty(PropertyName = "vehicleState", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string VehicleState { get; set; }

        [JsonProperty(PropertyName = "isFactoryUnit", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsFactoryUnit { get; set; }

        [JsonProperty(PropertyName = "seller", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Seller { get; set; }

        [JsonProperty(PropertyName = "borrower", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Borrower { get; set; }

        [JsonProperty(PropertyName = "salesRep", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string SalesRep { get; set; }

        [JsonProperty(PropertyName = "salesRepEmail", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string SalesRepEmail { get; set; }

        [JsonProperty(PropertyName = "locationId", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid? LocationId { get; set; }

        [JsonProperty(PropertyName = "allowNewInspections", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool AllowNewInspections { get; set; }

        [JsonProperty(PropertyName = "size", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Size { get; set; }

        [JsonProperty(PropertyName = "hasExtraParts", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool HasExtraParts { get; set; }

        [JsonProperty(PropertyName = "extraPartsList", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ExtraPartsList { get; set; }

        [JsonProperty(PropertyName = "inspections", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<Inspection> AvailableInspections { get; set; }

        [JsonProperty(PropertyName = "inspectionTypes", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<InspectionType> AvailableInspectionTypes { get; set; }

        [JsonProperty(PropertyName = "inspectionMasters", NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<InspectionMaster> AvailableMasters { get; set; }
    }
}
