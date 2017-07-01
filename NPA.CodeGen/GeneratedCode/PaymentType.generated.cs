using System;
using System.Linq;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public class PaymentType : IEquatable<PaymentType>, IComparable<PaymentType>
    {
        public Guid PaymentTypeId { get; private set; }
        public string DisplayName { get; private set; }
		public string ShortName { get; private set; }
        public bool IsCashEquivalent { get; private set; }

        private PaymentType() { }

        private PaymentType(Guid paymentTypeId, string displayName, string shortName, bool isCashEquivalent)
        {
            PaymentTypeId = paymentTypeId;
            DisplayName = displayName;
			ShortName = shortName;
            IsCashEquivalent = isCashEquivalent;
        }

        public bool Equals(PaymentType other)
        {
            return this.PaymentTypeId.Equals(other.PaymentTypeId);
        }

        public int CompareTo(PaymentType other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }

		public static PaymentType FromShortName(string shortName)
		{
		   return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, shortName, true) == 0);
		}

		public static PaymentType FromDisplayName(string displayName)
		{
		   return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, displayName, true) == 0);
		}

        public static PaymentType Cash = new PaymentType(new Guid("721b811b-fd8b-47cd-9265-a3ce6c3f92ad"), "Cash", "Cash", true);

        public static PaymentType CashBack = new PaymentType(new Guid("f10f4443-b075-433c-b31b-5852ee711a80"), "Cash Back", "CashBack", true);

        public static PaymentType CashiersCheck = new PaymentType(new Guid("16fcf402-00ee-4936-89ca-678c240c0839"), "Cashiers Check", "CashiersCheck", false);

        public static PaymentType Check = new PaymentType(new Guid("a7e44248-8a63-4b51-bb31-21e660fd1658"), "Check", "Check", false);

        public static PaymentType MoneyOrder = new PaymentType(new Guid("a038ce60-db05-45df-8d12-c4e8c7ac2fd9"), "Money Order", "MoneyOrder", false);

        public static PaymentType TravelersCheck = new PaymentType(new Guid("8ea97f15-75f2-448b-bed6-b47b2b76d0ff"), "Travelers Check", "TravelersCheck", false);

        public static PaymentType WireTransfer = new PaymentType(new Guid("541a28e6-90c6-438f-9626-89b480c9fa72"), "Wire Transfer", "WireTransfer", false);

        public static IEnumerable<PaymentType> GetValues()
        {
            yield return Cash;
            yield return CashBack;
            yield return CashiersCheck;
            yield return Check;
            yield return MoneyOrder;
            yield return TravelersCheck;
            yield return WireTransfer;
    
        }
    }
}
