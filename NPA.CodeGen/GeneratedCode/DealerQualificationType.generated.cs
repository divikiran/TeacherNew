using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public partial class DealerQualificationType : IEquatable<DealerQualificationType>, IComparable<DealerQualificationType>
    {
        public Guid DealerQualificationTypeId { get; private set; }
        public string DisplayName { get; private set; }
        public string ShortName { get; private set; }

        private DealerQualificationType() { }

        private DealerQualificationType(Guid dealerQualificationTypeId, string dealerQualificationTypeName, string displayName)
        {
            this.DealerQualificationTypeId = dealerQualificationTypeId;
            this.ShortName = dealerQualificationTypeName;
            this.DisplayName = displayName;
        }

        public bool Equals(DealerQualificationType other)
        {
            return this.DealerQualificationTypeId.Equals(other.DealerQualificationTypeId);
        }

        public int CompareTo(DealerQualificationType other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }
        
        public static DealerQualificationType FromGuid(Guid guid)
        {
            return GetValues().SingleOrDefault(v => v.DealerQualificationTypeId == guid);
        }

        public static DealerQualificationType FromDisplayName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, name, true) == 0);
        }

        public static DealerQualificationType FromShortName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, name, true) == 0);
        }

        public static DealerQualificationType BankLetterGuarantee = new DealerQualificationType(
            new Guid("fc531379-d08b-4671-aef5-7f515198efeb"), "BankLetterGuarantee", "Bank Letter Guarantee");

        public static DealerQualificationType BusinessLicense = new DealerQualificationType(
            new Guid("f7ea7c50-1152-4c8f-abf7-6384201e4b7a"), "BusinessLicense", "Business License");

        public static DealerQualificationType DealerLicenseNumber = new DealerQualificationType(
            new Guid("2fc2e453-431f-43c3-b087-b104c64e5fac"), "DealerLicenseNumber", "Dealer License#");

        public static DealerQualificationType DismantlerLicenseNumber = new DealerQualificationType(
            new Guid("a004cd9f-d93c-487c-8f51-73136ec79498"), "DismantlerLicenseNumber", "Dismantler License#");

        public static DealerQualificationType FederalIdNumber = new DealerQualificationType(
            new Guid("302c605b-6220-4b48-84f7-6155a0ca969f"), "FederalIdNumber", "Federal Id#");

        public static DealerQualificationType IrsW9Form = new DealerQualificationType(
            new Guid("f3fd1ed0-c90c-4dab-bd7f-9149876311d0"), "IrsW9Form", "Irs W-9 Form");

        public static DealerQualificationType NpaDealerAgreement = new DealerQualificationType(
            new Guid("85d11026-c593-48dd-bcbd-aa5b37695b92"), "NpaDealerAgreement", "Npa Dealer Agreement");

        public static DealerQualificationType Other = new DealerQualificationType(
            new Guid("b6e6ddb1-9189-48bd-b295-f778910ef4f4"), "Other", "Other");

        public static DealerQualificationType ResaleLicenseNumber = new DealerQualificationType(
            new Guid("16ee0ea1-0a78-4e8b-b169-b6786ecad743"), "ResaleLicenseNumber", "Resale License#");

        public static DealerQualificationType SaleFeeAgreement = new DealerQualificationType(
            new Guid("3271aadc-c8aa-4bad-9dfd-975a7b7168bc"), "SaleFeeAgreement", "Sale Fee Agreement");

        public static DealerQualificationType SuretyBondNumber = new DealerQualificationType(
            new Guid("a1a468b9-8247-4363-830d-de42a7458972"), "SuretyBondNumber", "Surety Bond#");
        
        
        public static IEnumerable<DealerQualificationType> GetValues()
        {
            yield return BankLetterGuarantee;
            yield return BusinessLicense;
            yield return DealerLicenseNumber;
            yield return DismantlerLicenseNumber;
            yield return FederalIdNumber;
            yield return IrsW9Form;
            yield return NpaDealerAgreement;
            yield return Other;
            yield return ResaleLicenseNumber;
            yield return SaleFeeAgreement;
            yield return SuretyBondNumber;
        }
    }
}
