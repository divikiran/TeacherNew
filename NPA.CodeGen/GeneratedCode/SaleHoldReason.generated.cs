using System;
using System.Linq;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public partial class SaleHoldReason : IEquatable<SaleHoldReason>, IComparable<SaleHoldReason>
    {
        public Guid SaleHoldReasonId { get; private set; }
        public int SaleHoldReasonCode { get; private set; }
        public bool HoldFromSale { get; private set; }
        public string DisplayName { get; private set; }
        public bool IsCashEquivalent { get; private set; }

        private SaleHoldReason() { }

        private SaleHoldReason(Guid saleHoldReasonId, int saleHoldReasonCode, bool holdFromSale, string displayName)
        {
            SaleHoldReasonId = saleHoldReasonId;
            SaleHoldReasonCode = saleHoldReasonCode;
            HoldFromSale = holdFromSale;
            DisplayName = displayName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(Object other)
        {
            if (other is SaleHoldReason) return this.Equals((SaleHoldReason)other);

            return false;
        }

        public bool Equals(SaleHoldReason other)
        {
            return this.SaleHoldReasonId.Equals(other.SaleHoldReasonId);
        }

        public int CompareTo(SaleHoldReason other)
        {
            return this.DisplayName.CompareTo(other.DisplayName);
        }

        public static bool operator == (SaleHoldReason lhs, SaleHoldReason rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator != (SaleHoldReason lhs, SaleHoldReason rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public static SaleHoldReason FromDisplayName(string displayName)
        {
           return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, displayName, true) == 0);
        }

	public static SaleHoldReason FromReasonCode(int reasonCode)
	{
		return GetValues().SingleOrDefault(v => v.SaleHoldReasonCode == reasonCode);
	}

        public static SaleHoldReason NoHold = new SaleHoldReason(new Guid("0afd6516-3ed3-e011-aec2-0019b9b35da2"), 0, false, "NoHold");

        public static SaleHoldReason Bankruptcy = new SaleHoldReason(new Guid("0bfd6516-3ed3-e011-aec2-0019b9b35da2"), 1, true, "Bankruptcy");

        public static SaleHoldReason Error = new SaleHoldReason(new Guid("13fd6516-3ed3-e011-aec2-0019b9b35da2"), 10, true, "Error");

        public static SaleHoldReason Inspection = new SaleHoldReason(new Guid("14fd6516-3ed3-e011-aec2-0019b9b35da2"), 11, false, "Inspection");

        public static SaleHoldReason TitleProblem = new SaleHoldReason(new Guid("15fd6516-3ed3-e011-aec2-0019b9b35da2"), 12, true, "TitleProblem");

        public static SaleHoldReason Redemption = new SaleHoldReason(new Guid("16fd6516-3ed3-e011-aec2-0019b9b35da2"), 13, true, "Redemption");

        public static SaleHoldReason DealerRepurchase = new SaleHoldReason(new Guid("17fd6516-3ed3-e011-aec2-0019b9b35da2"), 14, false, "DealerRepurchase");

        public static SaleHoldReason Keys = new SaleHoldReason(new Guid("18fd6516-3ed3-e011-aec2-0019b9b35da2"), 15, false, "Keys");

        public static SaleHoldReason Mechanical = new SaleHoldReason(new Guid("19fd6516-3ed3-e011-aec2-0019b9b35da2"), 16, false, "Mechanical");

        public static SaleHoldReason PaintandBody = new SaleHoldReason(new Guid("1afd6516-3ed3-e011-aec2-0019b9b35da2"), 17, false, "PaintandBody");

        public static SaleHoldReason TransportationDelay = new SaleHoldReason(new Guid("1bfd6516-3ed3-e011-aec2-0019b9b35da2"), 18, false, "TransportationDelay");

        public static SaleHoldReason Corrections = new SaleHoldReason(new Guid("1cfd6516-3ed3-e011-aec2-0019b9b35da2"), 19, false, "Corrections");

        public static SaleHoldReason Insurance = new SaleHoldReason(new Guid("0cfd6516-3ed3-e011-aec2-0019b9b35da2"), 2, false, "Insurance");

        public static SaleHoldReason Military = new SaleHoldReason(new Guid("aeb237ce-b79b-e211-bfc2-0019b9b35da2"), 20, false, "Military");

        public static SaleHoldReason NoticeOfIntent = new SaleHoldReason(new Guid("afb237ce-b79b-e211-bfc2-0019b9b35da2"), 21, true, "NoticeOfIntent");

        public static SaleHoldReason NoticeOfSale = new SaleHoldReason(new Guid("b0b237ce-b79b-e211-bfc2-0019b9b35da2"), 22, true, "NoticeOfSale");

        public static SaleHoldReason Legal = new SaleHoldReason(new Guid("0dfd6516-3ed3-e011-aec2-0019b9b35da2"), 3, true, "Legal");

        public static SaleHoldReason OtherBuyer = new SaleHoldReason(new Guid("0efd6516-3ed3-e011-aec2-0019b9b35da2"), 4, false, "OtherBuyer");

        public static SaleHoldReason Repo = new SaleHoldReason(new Guid("0ffd6516-3ed3-e011-aec2-0019b9b35da2"), 5, false, "Repo");

        public static SaleHoldReason SpecialPromotion = new SaleHoldReason(new Guid("10fd6516-3ed3-e011-aec2-0019b9b35da2"), 6, false, "SpecialPromotion");

        public static SaleHoldReason Moved = new SaleHoldReason(new Guid("11fd6516-3ed3-e011-aec2-0019b9b35da2"), 7, false, "Moved");

        public static SaleHoldReason Other = new SaleHoldReason(new Guid("12fd6516-3ed3-e011-aec2-0019b9b35da2"), 9, false, "Other");

        public static SaleHoldReason ClearedForSale = new SaleHoldReason(new Guid("04721e4b-1b33-e611-9417-ac162d7cbbd1"), 99, true, "ClearedForSale");

        public static IEnumerable<SaleHoldReason> GetValues()
        {
            yield return NoHold;
            yield return Bankruptcy;
            yield return Error;
            yield return Inspection;
            yield return TitleProblem;
            yield return Redemption;
            yield return DealerRepurchase;
            yield return Keys;
            yield return Mechanical;
            yield return PaintandBody;
            yield return TransportationDelay;
            yield return Corrections;
            yield return Insurance;
            yield return Military;
            yield return NoticeOfIntent;
            yield return NoticeOfSale;
            yield return Legal;
            yield return OtherBuyer;
            yield return Repo;
            yield return SpecialPromotion;
            yield return Moved;
            yield return Other;
            yield return ClearedForSale;
    
        }
    }
}
