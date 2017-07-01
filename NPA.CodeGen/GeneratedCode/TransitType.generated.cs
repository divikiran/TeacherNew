using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public partial class TransitType : IEquatable<TransitType>, IComparable<TransitType>
    {
        public Guid TransitTypeGuid { get; private set; }
        public Int32 TransitTypeId { get; private set; }
        public string DisplayName { get; private set; }
        public string ShortName { get; private set; }

        private TransitType() { }

        private TransitType(Guid transitTypeGuid, Int32 transitTypeId, string transitTypeName, string displayName)
        {
            this.TransitTypeGuid = transitTypeGuid;
            this.TransitTypeId = transitTypeId;
            this.ShortName = transitTypeName;
            this.DisplayName = displayName;
        }

        public bool Equals(TransitType other)
        {
            return this.TransitTypeGuid.Equals(other.TransitTypeGuid);
        }

        public int CompareTo(TransitType other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }
        
        public static TransitType FromId(Int32 id)
        {
            return GetValues().SingleOrDefault(v => v.TransitTypeId == id);
        }
        
        public static TransitType FromGuid(Guid guid)
        {
            return GetValues().SingleOrDefault(v => v.TransitTypeGuid == guid);
        }

        public static TransitType FromDisplayName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, name, true) == 0);
        }

        public static TransitType FromShortName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, name, true) == 0);
        }

        public static TransitType Inbound = new TransitType(
            new Guid("7c97bb78-7c08-42b3-99f0-e78f37288046"), 0, "Inbound", "Inbound");

        public static TransitType InterFacility = new TransitType(
            new Guid("cf27cc66-5ab9-4c4b-bf6b-c8bd2b29357e"), 3, "InterFacility", "InterFacility");

        public static TransitType OutboundRedemption = new TransitType(
            new Guid("fde41199-31aa-4424-8f26-7c638a7cdb7e"), 2, "OutboundRedemption", "OutboundRedemption");

        public static TransitType OutboundSold = new TransitType(
            new Guid("2138dab7-43fb-4eb4-b470-ceba9fae4868"), 1, "OutboundSold", "OutboundSold");

        public static TransitType PendingInbound = new TransitType(
            new Guid("42a297a9-b3b8-4a20-af7e-9032d923148f"), 4, "PendingInbound", "Pending Inbound");

        public static TransitType PendingInterFacility = new TransitType(
            new Guid("eef00317-76d7-4b21-a561-b600de166fb8"), 7, "PendingInterFacility", "Pending InterFacility");

        public static TransitType PendingOutboundRedemption = new TransitType(
            new Guid("0a3d865e-4367-4bc8-a4b2-50738ea4c69a"), 6, "PendingOutboundRedemption", "Pending OutboundRedemption");

        public static TransitType PendingOutboundSold = new TransitType(
            new Guid("5e46f5fe-6f64-4319-902b-96feacc14a95"), 5, "PendingOutboundSold", "Pending OutboundSold");
        
        
        public static IEnumerable<TransitType> GetValues()
        {
            yield return Inbound;
            yield return InterFacility;
            yield return OutboundRedemption;
            yield return OutboundSold;
            yield return PendingInbound;
            yield return PendingInterFacility;
            yield return PendingOutboundRedemption;
            yield return PendingOutboundSold;
        }
    }
}
