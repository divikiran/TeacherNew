using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public partial class InspectionType : IEquatable<InspectionType>, IComparable<InspectionType>
    {
        public Guid InspectionTypeGuid { get; private set; }
        public Int32 InspectionTypeId { get; private set; }
        public string DisplayName { get; private set; }
        public string ShortName { get; private set; }

        private InspectionType() { }

        private InspectionType(Guid inspectionTypeGuid, Int32 inspectionTypeId, string inspectionTypeName, string displayName)
        {
            this.InspectionTypeGuid = inspectionTypeGuid;
            this.InspectionTypeId = inspectionTypeId;
            this.ShortName = inspectionTypeName;
            this.DisplayName = displayName;
        }

        public bool Equals(InspectionType other)
        {
            return this.InspectionTypeGuid.Equals(other.InspectionTypeGuid);
        }

        public int CompareTo(InspectionType other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }
        
        public static InspectionType FromId(Int32 id)
        {
            return GetValues().SingleOrDefault(v => v.InspectionTypeId == id);
        }
        
        public static InspectionType FromGuid(Guid guid)
        {
            return GetValues().SingleOrDefault(v => v.InspectionTypeGuid == guid);
        }

        public static InspectionType FromDisplayName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, name, true) == 0);
        }

        public static InspectionType FromShortName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, name, true) == 0);
        }

        public static InspectionType PostInspection = new InspectionType(
            new Guid("bb6eae39-70f8-4a8e-899e-68b7e3b6b778"), 2, "PostInspection", "Post-Inspection");

        public static InspectionType PreInspection = new InspectionType(
            new Guid("d93433e0-5564-4680-89fa-1b1bbd38d47e"), 1, "PreInspection", "Pre-Inspection");

        public static InspectionType Redemption = new InspectionType(
            new Guid("ec429290-35ed-4aab-81df-abb65aac2ea3"), 3, "Redemption", "Redemption");

        public static InspectionType TransferInspection = new InspectionType(
            new Guid("12b4dc86-1a04-4576-b731-393d6b08ea61"), 4, "TransferInspection", "Transfer Inspection");
        
        
        public static IEnumerable<InspectionType> GetValues()
        {
            yield return PostInspection;
            yield return PreInspection;
            yield return Redemption;
            yield return TransferInspection;
        }
    }
}
