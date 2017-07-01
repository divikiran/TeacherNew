using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public partial class SaleRuleType : IEquatable<SaleRuleType>, IComparable<SaleRuleType>
    {
        public Guid SaleRuleTypeId { get; private set; }
        public string DisplayName { get; private set; }
        public string ShortName { get; private set; }

        private SaleRuleType() { }

        private SaleRuleType(Guid saleRuleTypeId, string saleRuleTypeName, string displayName)
        {
            this.SaleRuleTypeId = saleRuleTypeId;
            this.ShortName = saleRuleTypeName;
            this.DisplayName = displayName;
        }

        public bool Equals(SaleRuleType other)
        {
            return this.SaleRuleTypeId.Equals(other.SaleRuleTypeId);
        }

        public int CompareTo(SaleRuleType other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }
        
        public static SaleRuleType FromGuid(Guid guid)
        {
            return GetValues().SingleOrDefault(v => v.SaleRuleTypeId == guid);
        }

        public static SaleRuleType FromDisplayName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, name, true) == 0);
        }

        public static SaleRuleType FromShortName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, name, true) == 0);
        }

        public static SaleRuleType AcceptTermsConditions = new SaleRuleType(
            new Guid("4fbaead1-b0cc-488f-9210-717fec618ebb"), "AcceptTermsConditions", "AcceptTermsConditions");

        public static SaleRuleType ByPassQualifications = new SaleRuleType(
            new Guid("72e20a0e-4acd-dd11-aca2-0019b9b35da2"), "ByPassQualifications", "ByPassQualifications");

        public static SaleRuleType Country = new SaleRuleType(
            new Guid("e5d78e54-1c8a-43ec-b523-0064de434a81"), "Country", "Country");

        public static SaleRuleType Emissions = new SaleRuleType(
            new Guid("a20e4d1a-9910-dd11-9dca-0019b9b35da2"), "Emissions", "Emissions");

        public static SaleRuleType FranchiseOnly = new SaleRuleType(
            new Guid("a9f89542-a04b-40de-a4cb-fb5b5f1f6d12"), "FranchiseOnly", "Franchise Only");

        public static SaleRuleType Invitation = new SaleRuleType(
            new Guid("1024306c-c526-dd11-9dca-0019b9b35da2"), "Invitation", "Invitation");

        public static SaleRuleType ProductCode = new SaleRuleType(
            new Guid("6fba8fe6-90bf-4c06-a6ac-0c9c7e8e0d20"), "ProductCode", "ProductCode");

        public static SaleRuleType PublicSale = new SaleRuleType(
            new Guid("33016f0c-9910-dd11-9dca-0019b9b35da2"), "PublicSale", "PublicSale");

        public static SaleRuleType SaleReportExclude = new SaleRuleType(
            new Guid("950b5a59-338f-e611-941b-00090ffe0001"), "SaleReportExclude", "SaleReportExclude");
        
        
        public static IEnumerable<SaleRuleType> GetValues()
        {
            yield return AcceptTermsConditions;
            yield return ByPassQualifications;
            yield return Country;
            yield return Emissions;
            yield return FranchiseOnly;
            yield return Invitation;
            yield return ProductCode;
            yield return PublicSale;
            yield return SaleReportExclude;
        }
    }
}
