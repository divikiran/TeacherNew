using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public partial class VehicleDocType : IEquatable<VehicleDocType>, IComparable<VehicleDocType>
    {
        public Guid VehicleDocTypeId { get; private set; }
        public string DisplayName { get; private set; }
        public string ShortName { get; private set; }

        private VehicleDocType() { }

        private VehicleDocType(Guid vehicleDocTypeId, string vehicleDocTypeName, string displayName)
        {
            this.VehicleDocTypeId = vehicleDocTypeId;
            this.ShortName = vehicleDocTypeName;
            this.DisplayName = displayName;
        }

        public bool Equals(VehicleDocType other)
        {
            return this.VehicleDocTypeId.Equals(other.VehicleDocTypeId);
        }

        public int CompareTo(VehicleDocType other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }
        
        public static VehicleDocType FromGuid(Guid guid)
        {
            return GetValues().SingleOrDefault(v => v.VehicleDocTypeId == guid);
        }

        public static VehicleDocType FromDisplayName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, name, true) == 0);
        }

        public static VehicleDocType FromShortName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, name, true) == 0);
        }

        public static VehicleDocType ActiveTheftHistory = new VehicleDocType(
            new Guid("56794883-113f-42c4-99b1-6f1eebfaf05f"), "ActiveTheftHistory", "Active Theft History");

        public static VehicleDocType BkClearance = new VehicleDocType(
            new Guid("99a9fb9f-3f2c-dd11-9dca-0019b9b35da2"), "BkClearance", "Bk Clearance");

        public static VehicleDocType BkTransfer = new VehicleDocType(
            new Guid("9aa9fb9f-3f2c-dd11-9dca-0019b9b35da2"), "BkTransfer", "Bk Transfer");

        public static VehicleDocType BrandedVehicle = new VehicleDocType(
            new Guid("87ca3e0b-5484-4c6a-93e1-287315c9c580"), "BrandedVehicle", "Branded Vehicle");

        public static VehicleDocType PublicNotice = new VehicleDocType(
            new Guid("50b7817b-2821-dd11-9dca-0019b9b35da2"), "PublicNotice", "Public Notice");

        public static VehicleDocType SalvageCertificate = new VehicleDocType(
            new Guid("d0d5db7d-930d-4a88-89f0-cbfad7f1f349"), "SalvageCertificate", "Salvage Certificate");

        public static VehicleDocType SalvageHistory = new VehicleDocType(
            new Guid("fe94ff04-a333-4c57-bd33-7706aa06f55c"), "SalvageHistory", "Salvage History");

        public static VehicleDocType SupportingDocs = new VehicleDocType(
            new Guid("6fa5c4bd-4656-e011-857a-0019b9b35da2"), "SupportingDocs", "Supporting Docs");

        public static VehicleDocType TITLE = new VehicleDocType(
            new Guid("98a9fb9f-3f2c-dd11-9dca-0019b9b35da2"), "TITLE", "TITLE");
        
        
        public static IEnumerable<VehicleDocType> GetValues()
        {
            yield return ActiveTheftHistory;
            yield return BkClearance;
            yield return BkTransfer;
            yield return BrandedVehicle;
            yield return PublicNotice;
            yield return SalvageCertificate;
            yield return SalvageHistory;
            yield return SupportingDocs;
            yield return TITLE;
        }
    }
}
