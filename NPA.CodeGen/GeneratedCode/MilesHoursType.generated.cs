using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public partial class MilesHoursType : IEquatable<MilesHoursType>, IComparable<MilesHoursType>
    {
        public Guid MilesHoursTypeId { get; private set; }
        public string DisplayName { get; private set; }
        public string ShortName { get; private set; }

        private MilesHoursType() { }

        private MilesHoursType(Guid milesHoursTypeId, string milesHoursTypeName, string displayName)
        {
            this.MilesHoursTypeId = milesHoursTypeId;
            this.ShortName = milesHoursTypeName;
            this.DisplayName = displayName;
        }

        public bool Equals(MilesHoursType other)
        {
            return this.MilesHoursTypeId.Equals(other.MilesHoursTypeId);
        }

        public int CompareTo(MilesHoursType other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }
        
        public static MilesHoursType FromGuid(Guid guid)
        {
            return GetValues().SingleOrDefault(v => v.MilesHoursTypeId == guid);
        }

        public static MilesHoursType FromDisplayName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, name, true) == 0);
        }

        public static MilesHoursType FromShortName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, name, true) == 0);
        }

        public static MilesHoursType ActualMilesHours = new MilesHoursType(
            new Guid("647ed75c-9910-4ee7-8412-357a49a576dc"), "ActualMilesHours", "Actual Miles/Hours");

        public static MilesHoursType ExceedsMechanicalLimits = new MilesHoursType(
            new Guid("f1bff7f7-91ff-42e5-8a7f-3c3a9916a629"), "ExceedsMechanicalLimits", "Exceeds Mechanical Limits");

        public static MilesHoursType MileageExempt = new MilesHoursType(
            new Guid("9cbe7e17-4535-460e-8062-350a1b7d886c"), "MileageExempt", "Mileage Exempt");

        public static MilesHoursType TotalMilesHoursUnknown = new MilesHoursType(
            new Guid("176466d3-3f55-48ed-a950-3578fa45ec68"), "TotalMilesHoursUnknown", "Total Miles/Hours Unknown");
        
        
        public static IEnumerable<MilesHoursType> GetValues()
        {
            yield return ActualMilesHours;
            yield return ExceedsMechanicalLimits;
            yield return MileageExempt;
            yield return TotalMilesHoursUnknown;
        }
    }
}
