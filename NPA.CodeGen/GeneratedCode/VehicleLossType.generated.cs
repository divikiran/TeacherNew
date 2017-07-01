using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public partial class VehicleLossType : IEquatable<VehicleLossType>, IComparable<VehicleLossType>
    {
        public Guid VehicleLossTypeId { get; private set; }
        public string DisplayName { get; private set; }
        public string ShortName { get; private set; }

        private VehicleLossType() { }

        private VehicleLossType(Guid vehicleLossTypeId, string vehicleLossTypeName, string displayName)
        {
            this.VehicleLossTypeId = vehicleLossTypeId;
            this.ShortName = vehicleLossTypeName;
            this.DisplayName = displayName;
        }

        public bool Equals(VehicleLossType other)
        {
            return this.VehicleLossTypeId.Equals(other.VehicleLossTypeId);
        }

        public int CompareTo(VehicleLossType other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }
        
        public static VehicleLossType FromGuid(Guid guid)
        {
            return GetValues().SingleOrDefault(v => v.VehicleLossTypeId == guid);
        }

        public static VehicleLossType FromDisplayName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, name, true) == 0);
        }

        public static VehicleLossType FromShortName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, name, true) == 0);
        }

        public static VehicleLossType COLLISION = new VehicleLossType(
            new Guid("d14107d1-e15e-4e70-949e-057b67612f65"), "COLLISION", "COLLISION");

        public static VehicleLossType DonatedVehicle = new VehicleLossType(
            new Guid("ac6aa5e6-af72-4e69-90d9-6d4b7ba0f262"), "DonatedVehicle", "Donated Vehicle");

        public static VehicleLossType FireLoss = new VehicleLossType(
            new Guid("f5c33ba9-e2ec-4903-bba9-fe899ea00938"), "FireLoss", "Fire Loss");

        public static VehicleLossType FleetLeaseVehicle = new VehicleLossType(
            new Guid("8cd84e39-5d43-4865-a245-5ea892b4d56f"), "FleetLeaseVehicle", "Fleet/Lease Vehicle");

        public static VehicleLossType ImpoundVehicle = new VehicleLossType(
            new Guid("71a455a8-9521-4ae8-a281-3570b42b269c"), "ImpoundVehicle", "Impound Vehicle");

        public static VehicleLossType NaturalCatastrophe = new VehicleLossType(
            new Guid("6e879652-d436-42c1-aa1f-9b5122a5227a"), "NaturalCatastrophe", "Natural Catastrophe");

        public static VehicleLossType OtherComprehensive = new VehicleLossType(
            new Guid("fa13bc9f-a034-432e-a181-4d091af18489"), "OtherComprehensive", "Other Comprehensive");

        public static VehicleLossType RentalVehicle = new VehicleLossType(
            new Guid("0a484889-09b3-4838-bd23-db7ea5bac608"), "RentalVehicle", "Rental Vehicle");

        public static VehicleLossType REPOSSESSION = new VehicleLossType(
            new Guid("123aa963-61ae-4f16-bde2-e5a05c261e03"), "REPOSSESSION", "REPOSSESSION");

        public static VehicleLossType TheftRecovery = new VehicleLossType(
            new Guid("8d1c26d6-9529-4b8e-82f5-404c11e7c910"), "TheftRecovery", "Theft Recovery");

        public static VehicleLossType UninsuredMotorists = new VehicleLossType(
            new Guid("c333a135-dde4-45cf-8ff3-70805088b6b5"), "UninsuredMotorists", "Uninsured Motorists");

        public static VehicleLossType VANDALISM = new VehicleLossType(
            new Guid("c9dd89f6-91d0-4a92-85f7-1e5bda524a18"), "VANDALISM", "VANDALISM");

        public static VehicleLossType WaterFlood = new VehicleLossType(
            new Guid("cbc49188-3c68-4970-bbd7-aecb719b7218"), "WaterFlood", "Water/Flood");
        
        
        public static IEnumerable<VehicleLossType> GetValues()
        {
            yield return COLLISION;
            yield return DonatedVehicle;
            yield return FireLoss;
            yield return FleetLeaseVehicle;
            yield return ImpoundVehicle;
            yield return NaturalCatastrophe;
            yield return OtherComprehensive;
            yield return RentalVehicle;
            yield return REPOSSESSION;
            yield return TheftRecovery;
            yield return UninsuredMotorists;
            yield return VANDALISM;
            yield return WaterFlood;
        }
    }
}
