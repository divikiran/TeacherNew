using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public partial class VINType : IEquatable<VINType>, IComparable<VINType>
    {
        public Guid VINTypeGuid { get; private set; }
        public Int32 VINTypeId { get; private set; }
        public string DisplayName { get; private set; }
        public string ShortName { get; private set; }

        private VINType() { }

        private VINType(Guid vinTypeGuid, Int32 vinTypeId, string vinTypeName, string displayName)
        {
            this.VINTypeGuid = vinTypeGuid;
            this.VINTypeId = vinTypeId;
            this.ShortName = vinTypeName;
            this.DisplayName = displayName;
        }

        public bool Equals(VINType other)
        {
            return this.VINTypeGuid.Equals(other.VINTypeGuid);
        }

        public int CompareTo(VINType other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }
        
        public static VINType FromId(Int32 id)
        {
            return GetValues().SingleOrDefault(v => v.VINTypeId == id);
        }
        
        public static VINType FromGuid(Guid guid)
        {
            return GetValues().SingleOrDefault(v => v.VINTypeGuid == guid);
        }

        public static VINType FromDisplayName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, name, true) == 0);
        }

        public static VINType FromShortName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, name, true) == 0);
        }

        public static VINType Marine = new VINType(
            new Guid("d4d71be5-8a0c-49dc-bc8f-15f6096bb275"), 1, "Marine", "Marine");

        public static VINType Standard = new VINType(
            new Guid("fd666540-fe04-4e0c-8188-aab06ecb4e46"), 0, "Standard", "Standard");
        
        
        public static IEnumerable<VINType> GetValues()
        {
            yield return Marine;
            yield return Standard;
        }
    }
}
