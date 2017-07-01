using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPA.CodeGen
{
    public partial class OnlineVendor : IEquatable<OnlineVendor>, IComparable<OnlineVendor>
    {
        public Guid OnlineVendorId { get; private set; }
        public string ShortName { get; private set; }
        public string DisplayName { get; private set; }
		public int DisplayOrder { get; private set; }
        public int DynamicCloseTime { get; private set; }
        
        private OnlineVendor() { }

        private OnlineVendor(Guid onlineVendorId, string shortName, int displayOrder, int dynamicCloseTime, string displayName)
        {
            OnlineVendorId = onlineVendorId;
            ShortName = shortName;
            DisplayOrder = displayOrder;
            DynamicCloseTime = dynamicCloseTime;
            DisplayName = displayName;
        }
                
        public bool Equals(OnlineVendor other)
        {
            return this.OnlineVendorId.Equals(other.OnlineVendorId);
        }
                
        public int CompareTo(OnlineVendor other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public static OnlineVendor FromGuid(Guid guid)
        {
            return GetValues().Single(v => v.OnlineVendorId == guid);
        }

		public static OnlineVendor FromShortName(string shortName)
		{
		   return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, shortName, true) == 0);
		}

		public static OnlineVendor FromDisplayName(string displayName)
		{
		   return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, displayName, true) == 0);
		}

        public static OnlineVendor VictoryDealerExchange = 
                new OnlineVendor(new Guid("eb6d6915-23ca-e211-9f63-005056c00008"), "VictoryDealerExchange", 9, 0, "Victory Dealer Exchange");

        public static OnlineVendor BRP = 
                new OnlineVendor(new Guid("4bcd9d10-3ef7-4ead-8059-013196532a38"), "BRP", 10, 5, "BRP");

        public static OnlineVendor InsuranceAndTotalLoss = 
                new OnlineVendor(new Guid("de581fe5-f2c3-4abf-be41-128fa20d69bc"), "InsuranceAndTotalLoss", 4, 5, "Insurance & Total Loss");

        public static OnlineVendor BMW = 
                new OnlineVendor(new Guid("06bd5285-d07f-4ff5-a733-1e88641d3520"), "BMW", 11, 5, "BMW Motorrad");

        public static OnlineVendor NpaBlackMarket = 
                new OnlineVendor(new Guid("6f0ed623-8ffe-464d-8d60-2c2cdfe87e7f"), "NpaBlackMarket", 3, 5, "NPA Black Market");

        public static OnlineVendor NpaLiveAuction = 
                new OnlineVendor(new Guid("32fbd714-dad5-4bca-9a12-2f07e8b2fe5c"), "NpaLiveAuction", 1, 5, "NPA Live Auction");

        public static OnlineVendor NpaEsale = 
                new OnlineVendor(new Guid("458f6634-7b1f-40ce-8042-853dde0b0c77"), "NpaEsale", 2, 5, "NPA eSale");

        public static OnlineVendor HDDX = 
                new OnlineVendor(new Guid("90841cef-1e21-4ba8-ab60-b148e9bc13e7"), "HDDX", 5, 5, "H-D Dealer Exchange");

        public static OnlineVendor IndianDealerExchange = 
                new OnlineVendor(new Guid("d2276188-b8b8-4d02-afed-bb6b3361410b"), "IndianDealerExchange", 8, 0, "Indian Dealer Exchange");

        public static OnlineVendor PenskeCarRental = 
                new OnlineVendor(new Guid("32948286-974b-4b4d-9cb9-d64da74531ec"), "PenskeCarRental", 6, 5, "Penske Direct");

        public static IEnumerable<OnlineVendor> GetValues()
        {
            yield return VictoryDealerExchange;
            yield return BRP;
            yield return InsuranceAndTotalLoss;
            yield return BMW;
            yield return NpaBlackMarket;
            yield return NpaLiveAuction;
            yield return NpaEsale;
            yield return HDDX;
            yield return IndianDealerExchange;
            yield return PenskeCarRental;
        }
    }
}
    