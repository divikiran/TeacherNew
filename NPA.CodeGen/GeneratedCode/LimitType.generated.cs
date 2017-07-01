using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public partial class LimitType : IEquatable<LimitType>, IComparable<LimitType>
    {
        public Guid LimitTypeGuid { get; private set; }
        public Int32 LimitTypeId { get; private set; }
        public string DisplayName { get; private set; }
        public string ShortName { get; private set; }

        private LimitType() { }

        private LimitType(Guid limitTypeGuid, Int32 limitTypeId, string limitTypeName, string displayName)
        {
            this.LimitTypeGuid = limitTypeGuid;
            this.LimitTypeId = limitTypeId;
            this.ShortName = limitTypeName;
            this.DisplayName = displayName;
        }

        public bool Equals(LimitType other)
        {
            return this.LimitTypeGuid.Equals(other.LimitTypeGuid);
        }

        public int CompareTo(LimitType other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }
        
        public static LimitType FromId(Int32 id)
        {
            return GetValues().SingleOrDefault(v => v.LimitTypeId == id);
        }
        
        public static LimitType FromGuid(Guid guid)
        {
            return GetValues().SingleOrDefault(v => v.LimitTypeGuid == guid);
        }

        public static LimitType FromDisplayName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, name, true) == 0);
        }

        public static LimitType FromShortName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, name, true) == 0);
        }

        public static LimitType Fee = new LimitType(
            new Guid("4061c3bd-7d3a-4348-b7b6-81f5b9e6a6cc"), 1, "Fee", "Fee");

        public static LimitType Limit = new LimitType(
            new Guid("2256f183-6f68-4ecb-a990-915a5408103f"), 0, "Limit", "Limit");
        
        
        public static IEnumerable<LimitType> GetValues()
        {
            yield return Fee;
            yield return Limit;
        }
    }
}
