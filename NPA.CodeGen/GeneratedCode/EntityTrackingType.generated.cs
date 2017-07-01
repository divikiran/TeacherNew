using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public partial class EntityTrackingType : IEquatable<EntityTrackingType>, IComparable<EntityTrackingType>
    {
        public Guid EntityTrackingTypeId { get; private set; }
        public string DisplayName { get; private set; }
        public string ShortName { get; private set; }

        private EntityTrackingType() { }

        private EntityTrackingType(Guid entityTrackingTypeId, string entityTrackingTypeName, string displayName)
        {
            this.EntityTrackingTypeId = entityTrackingTypeId;
            this.ShortName = entityTrackingTypeName;
            this.DisplayName = displayName;
        }

        public bool Equals(EntityTrackingType other)
        {
            return this.EntityTrackingTypeId.Equals(other.EntityTrackingTypeId);
        }

        public int CompareTo(EntityTrackingType other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }
        
        public static EntityTrackingType FromGuid(Guid guid)
        {
            return GetValues().SingleOrDefault(v => v.EntityTrackingTypeId == guid);
        }

        public static EntityTrackingType FromDisplayName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, name, true) == 0);
        }

        public static EntityTrackingType FromShortName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, name, true) == 0);
        }

        public static EntityTrackingType AcceptTermsConditions = new EntityTrackingType(
            new Guid("1f4ae87a-76f0-430e-a66d-dc82a8a8fafd"), "AcceptTermsConditions", "AcceptTermsConditions");

        public static EntityTrackingType AMSLogin = new EntityTrackingType(
            new Guid("a89ea822-b45a-e211-a4b2-0019b9b35da2"), "AMSLogin", "AMSLogin");

        public static EntityTrackingType BlackMarketLogin = new EntityTrackingType(
            new Guid("684e07df-d0d1-429e-97f3-d8c7c7a1c3ac"), "BlackMarketLogin", "BlackMarketLogin");

        public static EntityTrackingType BlackMarketView = new EntityTrackingType(
            new Guid("94f33f14-f12d-43e0-b24b-6e8a7f84a3d3"), "BlackMarketView", "BlackMarketView");

        public static EntityTrackingType BMWItemView = new EntityTrackingType(
            new Guid("35e9e1fb-1f74-4958-acda-b3b0f9b44c3f"), "BMWItemView", "BMWItemView");

        public static EntityTrackingType BMWLogin = new EntityTrackingType(
            new Guid("ed631091-3aa4-4ff4-8975-33005996b55e"), "BMWLogin", "BMWLogin");

        public static EntityTrackingType BRPItemView = new EntityTrackingType(
            new Guid("17dc68be-4350-48db-97ef-a55a0ff31422"), "BRPItemView", "BRPItemView");

        public static EntityTrackingType BRPLogin = new EntityTrackingType(
            new Guid("7db824e4-b52d-469e-9cb1-b74bb5ae9126"), "BRPLogin", "BRPLogin");

        public static EntityTrackingType CarrierLogin = new EntityTrackingType(
            new Guid("fcb8b600-b35a-e211-a4b2-0019b9b35da2"), "CarrierLogin", "CarrierLogin");

        public static EntityTrackingType CompetitiveBike = new EntityTrackingType(
            new Guid("01ca8672-dbbe-e411-940f-ac162d7cbbd1"), "CompetitiveBike", "Competitive Bike");

        public static EntityTrackingType CompetitiveBikeConsignment = new EntityTrackingType(
            new Guid("825361c6-09d4-e411-940f-ac162d7cbbd1"), "CompetitiveBikeConsignment", "Competitive Bike Consignment");

        public static EntityTrackingType CrWriterLogin = new EntityTrackingType(
            new Guid("d3f2c7a4-3d9e-e311-93fc-ac162d7cbbd1"), "CrWriterLogin", "CrWriterLogin");

        public static EntityTrackingType CrWriterNewInspection = new EntityTrackingType(
            new Guid("b5091f6c-fa34-4201-851a-f4f6a2ca76ae"), "CrWriterNewInspection", "CrWriterNewInspection");

        public static EntityTrackingType DataservicesGetmultiplevalueguides = new EntityTrackingType(
            new Guid("a7595091-0de7-446a-b9e8-4fd956ae5f50"), "DataservicesGetmultiplevalueguides", "Dataservices: Getmultiplevalueguides");

        public static EntityTrackingType DataservicesGetvalueguide = new EntityTrackingType(
            new Guid("a9240e07-913d-4663-ac32-bdef76a393c3"), "DataservicesGetvalueguide", "Dataservices: Getvalueguide");

        public static EntityTrackingType DealerExpoItemView = new EntityTrackingType(
            new Guid("94b6698f-c5cd-4b5e-a860-15fee8305252"), "DealerExpoItemView", "DealerExpoItemView");

        public static EntityTrackingType DPInventoryAcceptST = new EntityTrackingType(
            new Guid("522c6332-793a-e711-80e3-1c98ec191a2b"), "DPInventoryAcceptST", "DPInventoryAcceptST");

        public static EntityTrackingType DPInventoryAcceptSuggestedReserve = new EntityTrackingType(
            new Guid("532c6332-793a-e711-80e3-1c98ec191a2b"), "DPInventoryAcceptSuggestedReserve", "DPInventoryAcceptSuggestedReserve");

        public static EntityTrackingType DPPurchaseAndBidsAcceptLastChanceOffer = new EntityTrackingType(
            new Guid("552c6332-793a-e711-80e3-1c98ec191a2b"), "DPPurchaseAndBidsAcceptLastChanceOffer", "DPPurchaseAndBidsAcceptLastChanceOffer");

        public static EntityTrackingType DPPurchaseAndBidsAcceptST = new EntityTrackingType(
            new Guid("542c6332-793a-e711-80e3-1c98ec191a2b"), "DPPurchaseAndBidsAcceptST", "DPPurchaseAndBidsAcceptST");

        public static EntityTrackingType DPPurchaseAndBidsDeclineLastChanceOffer = new EntityTrackingType(
            new Guid("562c6332-793a-e711-80e3-1c98ec191a2b"), "DPPurchaseAndBidsDeclineLastChanceOffer", "DPPurchaseAndBidsDeclineLastChanceOffer");

        public static EntityTrackingType DucatiItemView = new EntityTrackingType(
            new Guid("fa0943d0-b2e1-4cef-b2f8-ef9411321806"), "DucatiItemView", "DucatiItemView");

        public static EntityTrackingType DucatiLogin = new EntityTrackingType(
            new Guid("de7e3178-8f7b-42fa-84a6-d0a1cdf397c7"), "DucatiLogin", "DucatiLogin");

        public static EntityTrackingType ESaleItemView = new EntityTrackingType(
            new Guid("aa3c4926-d494-4536-bed2-577f5cdcd202"), "ESaleItemView", "ESaleItemView");

        public static EntityTrackingType HDDXItemView = new EntityTrackingType(
            new Guid("3df21803-577a-4397-9e2b-bde7ad59f875"), "HDDXItemView", "HDDXItemView");

        public static EntityTrackingType HDDXLogin = new EntityTrackingType(
            new Guid("e1ca2cf1-047e-4f1d-ac2e-07feea2cffa8"), "HDDXLogin", "HDDXLogin");

        public static EntityTrackingType IndianItemView = new EntityTrackingType(
            new Guid("b05c502a-fc0a-467d-8293-4699932c1bdc"), "IndianItemView", "IndianItemView");

        public static EntityTrackingType IndianLogin = new EntityTrackingType(
            new Guid("0c5c86ba-c376-480b-878c-49686e44d5d0"), "IndianLogin", "IndianLogin");

        public static EntityTrackingType Instavinorder = new EntityTrackingType(
            new Guid("d4ba21cf-efa9-4141-aff4-00948d4bf9ff"), "Instavinorder", "Instavinorder");

        public static EntityTrackingType Instavinredirect = new EntityTrackingType(
            new Guid("72b602ac-248c-e211-9616-0019b9b35da2"), "Instavinredirect", "Instavinredirect");

        public static EntityTrackingType Instavinview = new EntityTrackingType(
            new Guid("5a2fc05c-e853-46d7-a13a-8d534fa514e4"), "Instavinview", "Instavinview");

        public static EntityTrackingType InsurerLogin = new EntityTrackingType(
            new Guid("feb8b600-b35a-e211-a4b2-0019b9b35da2"), "InsurerLogin", "InsurerLogin");

        public static EntityTrackingType LenderLogin = new EntityTrackingType(
            new Guid("fdb8b600-b35a-e211-a4b2-0019b9b35da2"), "LenderLogin", "LenderLogin");

        public static EntityTrackingType LenderValueGuideView = new EntityTrackingType(
            new Guid("39f8fdee-4d50-4cd6-9f5a-8b0c8572d0bc"), "LenderValueGuideView", "LenderValueGuideView");

        public static EntityTrackingType Login = new EntityTrackingType(
            new Guid("fd9b76ab-4029-4fce-a1f8-c6d0ef335d2b"), "Login", "Login");

        public static EntityTrackingType MobileAcceptTermsConditions = new EntityTrackingType(
            new Guid("04ae4fac-2e37-4d51-bcaa-c3529b84b9d4"), "MobileAcceptTermsConditions", "MobileAcceptTermsConditions");

        public static EntityTrackingType MobileActivity = new EntityTrackingType(
            new Guid("092ccedd-99f4-e211-93fa-ac162d7cbbd1"), "MobileActivity", "MobileActivity");

        public static EntityTrackingType MobileBlackMarketItemView = new EntityTrackingType(
            new Guid("064cdfcf-fdee-e611-80de-1c98ec191a2b"), "MobileBlackMarketItemView", "MobileBlackMarketItemView");

        public static EntityTrackingType MobileConsignment = new EntityTrackingType(
            new Guid("1a68e034-d171-e411-940d-ac162d7cbbd1"), "MobileConsignment", "MobileConsignment");

        public static EntityTrackingType MobileDucatiItemView = new EntityTrackingType(
            new Guid("a2197ec8-bde9-4089-9f06-bed6cca2e818"), "MobileDucatiItemView", "MobileDucatiItemView");

        public static EntityTrackingType MobileESaleItemView = new EntityTrackingType(
            new Guid("711cc047-f47a-426b-ad98-2a00659c79e8"), "MobileESaleItemView", "MobileESaleItemView");

        public static EntityTrackingType MobileHDDXItemView = new EntityTrackingType(
            new Guid("7a154ff2-b154-4e8d-ad48-e009191a6ed4"), "MobileHDDXItemView", "MobileHDDXItemView");

        public static EntityTrackingType MobileIfBidAccepted = new EntityTrackingType(
            new Guid("9e40096e-4400-e311-93fa-ac162d7cbbd1"), "MobileIfBidAccepted", "MobileIfBidAccepted");

        public static EntityTrackingType MobileIfBidCounterOffer = new EntityTrackingType(
            new Guid("1dbb25d8-7b45-e311-93fb-ac162d7cbbd1"), "MobileIfBidCounterOffer", "MobileIfBidCounterOffer");

        public static EntityTrackingType MobileIfBidDenied = new EntityTrackingType(
            new Guid("9f40096e-4400-e311-93fa-ac162d7cbbd1"), "MobileIfBidDenied", "MobileIfBidDenied");

        public static EntityTrackingType MobileLenderLogin = new EntityTrackingType(
            new Guid("9c40096e-4400-e311-93fa-ac162d7cbbd1"), "MobileLenderLogin", "MobileLenderLogin");

        public static EntityTrackingType MobileLogin = new EntityTrackingType(
            new Guid("aae61268-ba21-4b15-b3b4-7ee590e13afa"), "MobileLogin", "MobileLogin");

        public static EntityTrackingType MobileOnlineBid = new EntityTrackingType(
            new Guid("afff3663-d1ed-4af8-b1a1-8cfae795ca49"), "MobileOnlineBid", "MobileOnlineBid");

        public static EntityTrackingType MobileProxyBid = new EntityTrackingType(
            new Guid("6bd6d1b8-ced5-4584-a506-5033f5ea9399"), "MobileProxyBid", "MobileProxyBid");

        public static EntityTrackingType MobileReserveSet = new EntityTrackingType(
            new Guid("9d40096e-4400-e311-93fa-ac162d7cbbd1"), "MobileReserveSet", "MobileReserveSet");

        public static EntityTrackingType MobileSalesLogin = new EntityTrackingType(
            new Guid("3b1bd84c-9aaf-e311-93fd-ac162d7cbbd1"), "MobileSalesLogin", "MobileSalesLogin");

        public static EntityTrackingType MobileSalvageItemView = new EntityTrackingType(
            new Guid("8b0e6e8f-6a9d-4a1e-b55c-05ac5faffc6e"), "MobileSalvageItemView", "MobileSalvageItemView");

        public static EntityTrackingType MobileScratchDentItemView = new EntityTrackingType(
            new Guid("39cba915-64cb-4e35-9a54-54d4f4eab8fa"), "MobileScratchDentItemView", "MobileScratchDentItemView");

        public static EntityTrackingType MobileSimulcastItemView = new EntityTrackingType(
            new Guid("898f66c0-6338-4947-81d6-ad8aeaf9e480"), "MobileSimulcastItemView", "MobileSimulcastItemView");

        public static EntityTrackingType MobileValueGuideView = new EntityTrackingType(
            new Guid("991e594e-2bb3-4487-8931-ea499756f63f"), "MobileValueGuideView", "MobileValueGuideView");

        public static EntityTrackingType MotoLeaseItemView = new EntityTrackingType(
            new Guid("ae2bd52e-c099-4f78-aef9-ff723a65fc3d"), "MotoLeaseItemView", "MotoLeaseItemView");

        public static EntityTrackingType MotoLeaseLogin = new EntityTrackingType(
            new Guid("1ce21156-fe64-4257-8d6a-e027ca9e84c2"), "MotoLeaseLogin", "MotoLeaseLogin");

        public static EntityTrackingType PenskeDirectItemView = new EntityTrackingType(
            new Guid("8e0022ef-6b0a-4005-868e-7a926607654f"), "PenskeDirectItemView", "PenskeDirectItemView");

        public static EntityTrackingType PenskeDirectLogin = new EntityTrackingType(
            new Guid("993341b5-ffbd-4da4-bad1-df32448382a9"), "PenskeDirectLogin", "PenskeDirectLogin");

        public static EntityTrackingType SalvageItemView = new EntityTrackingType(
            new Guid("a527c86b-a129-e011-b902-0019b9b35da2"), "SalvageItemView", "SalvageItemView");

        public static EntityTrackingType SalvageLogin = new EntityTrackingType(
            new Guid("a627c86b-a129-e011-b902-0019b9b35da2"), "SalvageLogin", "SalvageLogin");

        public static EntityTrackingType ScratchDentItemView = new EntityTrackingType(
            new Guid("91a05de7-a9b5-4a6f-9410-712f63abbbe5"), "ScratchDentItemView", "ScratchDentItemView");

        public static EntityTrackingType SimulcastBidAccess = new EntityTrackingType(
            new Guid("08d9b8bc-8015-4d88-8c48-89370aafd1cb"), "SimulcastBidAccess", "SimulcastBidAccess");

        public static EntityTrackingType SimulcastItemView = new EntityTrackingType(
            new Guid("2d165fea-1217-4df0-9d94-57d132b108da"), "SimulcastItemView", "SimulcastItemView");

        public static EntityTrackingType SimulcastSoftwareAlert = new EntityTrackingType(
            new Guid("edca71a2-e9be-4a6e-be47-515560685961"), "SimulcastSoftwareAlert", "SimulcastSoftwareAlert");

        public static EntityTrackingType SuggestedReserveChange = new EntityTrackingType(
            new Guid("6c7cf0e2-42e8-e411-940f-ac162d7cbbd1"), "SuggestedReserveChange", "Suggested Reserve Change");

        public static EntityTrackingType ValueGuideView = new EntityTrackingType(
            new Guid("e6f48b52-d7a5-4c54-ab70-96510ca749ee"), "ValueGuideView", "ValueGuideView");

        public static EntityTrackingType VictoryItemView = new EntityTrackingType(
            new Guid("9e1f4d7c-74e3-4718-9dd2-bffff113d7cc"), "VictoryItemView", "VictoryItemView");

        public static EntityTrackingType VictoryLogin = new EntityTrackingType(
            new Guid("9e54a2a8-91d0-4dd2-8143-37b7aeb061fa"), "VictoryLogin", "VictoryLogin");

        public static EntityTrackingType WebConsignment = new EntityTrackingType(
            new Guid("1c68e034-d171-e411-940d-ac162d7cbbd1"), "WebConsignment", "WebConsignment");
        
        
        public static IEnumerable<EntityTrackingType> GetValues()
        {
            yield return AcceptTermsConditions;
            yield return AMSLogin;
            yield return BlackMarketLogin;
            yield return BlackMarketView;
            yield return BMWItemView;
            yield return BMWLogin;
            yield return BRPItemView;
            yield return BRPLogin;
            yield return CarrierLogin;
            yield return CompetitiveBike;
            yield return CompetitiveBikeConsignment;
            yield return CrWriterLogin;
            yield return CrWriterNewInspection;
            yield return DataservicesGetmultiplevalueguides;
            yield return DataservicesGetvalueguide;
            yield return DealerExpoItemView;
            yield return DPInventoryAcceptST;
            yield return DPInventoryAcceptSuggestedReserve;
            yield return DPPurchaseAndBidsAcceptLastChanceOffer;
            yield return DPPurchaseAndBidsAcceptST;
            yield return DPPurchaseAndBidsDeclineLastChanceOffer;
            yield return DucatiItemView;
            yield return DucatiLogin;
            yield return ESaleItemView;
            yield return HDDXItemView;
            yield return HDDXLogin;
            yield return IndianItemView;
            yield return IndianLogin;
            yield return Instavinorder;
            yield return Instavinredirect;
            yield return Instavinview;
            yield return InsurerLogin;
            yield return LenderLogin;
            yield return LenderValueGuideView;
            yield return Login;
            yield return MobileAcceptTermsConditions;
            yield return MobileActivity;
            yield return MobileBlackMarketItemView;
            yield return MobileConsignment;
            yield return MobileDucatiItemView;
            yield return MobileESaleItemView;
            yield return MobileHDDXItemView;
            yield return MobileIfBidAccepted;
            yield return MobileIfBidCounterOffer;
            yield return MobileIfBidDenied;
            yield return MobileLenderLogin;
            yield return MobileLogin;
            yield return MobileOnlineBid;
            yield return MobileProxyBid;
            yield return MobileReserveSet;
            yield return MobileSalesLogin;
            yield return MobileSalvageItemView;
            yield return MobileScratchDentItemView;
            yield return MobileSimulcastItemView;
            yield return MobileValueGuideView;
            yield return MotoLeaseItemView;
            yield return MotoLeaseLogin;
            yield return PenskeDirectItemView;
            yield return PenskeDirectLogin;
            yield return SalvageItemView;
            yield return SalvageLogin;
            yield return ScratchDentItemView;
            yield return SimulcastBidAccess;
            yield return SimulcastItemView;
            yield return SimulcastSoftwareAlert;
            yield return SuggestedReserveChange;
            yield return ValueGuideView;
            yield return VictoryItemView;
            yield return VictoryLogin;
            yield return WebConsignment;
        }
    }
}
