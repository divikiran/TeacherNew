using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public partial class LinkType : IEquatable<LinkType>, IComparable<LinkType>
    {
        public Guid LinkTypeGuid { get; private set; }
        public Int32 LinkTypeId { get; private set; }
        public string DisplayName { get; private set; }
        public string ShortName { get; private set; }

        private LinkType() { }

        private LinkType(Guid linkTypeGuid, Int32 linkTypeId, string linkTypeName, string displayName)
        {
            this.LinkTypeGuid = linkTypeGuid;
            this.LinkTypeId = linkTypeId;
            this.ShortName = linkTypeName;
            this.DisplayName = displayName;
        }

        public bool Equals(LinkType other)
        {
            return this.LinkTypeGuid.Equals(other.LinkTypeGuid);
        }

        public int CompareTo(LinkType other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }
        
        public static LinkType FromId(Int32 id)
        {
            return GetValues().SingleOrDefault(v => v.LinkTypeId == id);
        }
        
        public static LinkType FromGuid(Guid guid)
        {
            return GetValues().SingleOrDefault(v => v.LinkTypeGuid == guid);
        }

        public static LinkType FromDisplayName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, name, true) == 0);
        }

        public static LinkType FromShortName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, name, true) == 0);
        }

        public static LinkType AccountingCustomer = new LinkType(
            new Guid("6049c505-c8d5-4ac0-a18f-ad8e9694fbc2"), 55, "AccountingCustomer", "Accounting Customer");

        public static LinkType AccountingVendor = new LinkType(
            new Guid("dd0bbad4-89f5-4741-b943-41ae7230dbed"), 56, "AccountingVendor", "Accounting Vendor");

        public static LinkType Activity = new LinkType(
            new Guid("0979e485-9ef4-e211-93fa-ac162d7cbbd1"), 45, "Activity", "Activity");

        public static LinkType AMSUser = new LinkType(
            new Guid("6d79579a-7b0f-46e2-9b57-464d6a58467d"), 20, "AMSUser", "AMSUser");

        public static LinkType Auction = new LinkType(
            new Guid("8f94b3fe-2b3f-4950-b337-ca165d514538"), 6, "Auction", "Auction");

        public static LinkType AuctionVendor = new LinkType(
            new Guid("e601d098-6ba1-e211-8f0d-005056c00008"), 43, "AuctionVendor", "AuctionVendor");

        public static LinkType Bidder = new LinkType(
            new Guid("71ca8a98-f201-400d-8753-9c378e2a6426"), 64, "Bidder", "Bidder");

        public static LinkType Billing = new LinkType(
            new Guid("c6e4d5a8-be09-4aa5-9662-a64ab2098bc9"), 61, "Billing", "Billing");

        public static LinkType BookProviderBrand = new LinkType(
            new Guid("fd44eb7d-fb32-4e87-9ce2-3064cfa2312a"), 31, "BookProviderBrand", "BookProviderBrand");

        public static LinkType BookProviderModel = new LinkType(
            new Guid("8c0d69e7-1d61-4e56-b46f-284c6ae99a92"), 32, "BookProviderModel", "BookProviderModel");

        public static LinkType Buyer = new LinkType(
            new Guid("f4bd2ea2-a263-42db-bdae-1d0263392d48"), 48, "Buyer", "Buyer");

        public static LinkType Carrier = new LinkType(
            new Guid("dfb9b885-c1fb-dd11-b442-0019b9b35da2"), 12, "Carrier", "Carrier");

        public static LinkType CarrierAlternate = new LinkType(
            new Guid("2e4d9bcc-5692-e511-9413-ac162d7cbbd1"), 58, "CarrierAlternate", "CarrierAlternate");

        public static LinkType Consumer = new LinkType(
            new Guid("2b6be1a2-745f-495c-858a-2a722ac01136"), 4, "Consumer", "Consumer");

        public static LinkType Contact = new LinkType(
            new Guid("aca90dd9-0605-de11-b442-0019b9b35da2"), 18, "Contact", "Contact");

        public static LinkType DataProvider = new LinkType(
            new Guid("d44eaa90-8cb5-403d-8ef2-1a648556b60d"), 44, "DataProvider", "DataProvider");

        public static LinkType Dealer = new LinkType(
            new Guid("f241d7dd-1ddb-4dfb-86be-04253d8d9ff4"), 2, "Dealer", "Dealer");

        public static LinkType DealerAlternateAddress = new LinkType(
            new Guid("e9aed15c-c85e-e411-940b-ac162d7cbbd1"), 50, "DealerAlternateAddress", "DealerAlternateAddress");

        public static LinkType FollowUp = new LinkType(
            new Guid("7e1df379-c1fb-dd11-b442-0019b9b35da2"), 9, "FollowUp", "FollowUp");

        public static LinkType Franchise = new LinkType(
            new Guid("b453e550-7ef0-49a6-bbb1-66016fa82203"), 5, "Franchise", "Franchise");

        public static LinkType FreightForwarder = new LinkType(
            new Guid("9e171cf1-394b-e411-940a-ac162d7cbbd1"), 49, "FreightForwarder", "FreightForwarder");

        public static LinkType General = new LinkType(
            new Guid("d9297900-ff59-4cea-add9-bdb1ddf23b7d"), 3, "General", "General");

        public static LinkType Guest = new LinkType(
            new Guid("c0c30d22-0210-de11-b442-0019b9b35da2"), 23, "Guest", "Guest");

        public static LinkType IfBid = new LinkType(
            new Guid("7f1df379-c1fb-dd11-b442-0019b9b35da2"), 10, "IfBid", "IfBid");

        public static LinkType Inspection = new LinkType(
            new Guid("20107c7b-be39-42b9-9fed-f9e3c1cab90a"), 37, "Inspection", "Inspection");

        public static LinkType Invoice = new LinkType(
            new Guid("e0b9b885-c1fb-dd11-b442-0019b9b35da2"), 13, "Invoice", "Invoice");

        public static LinkType Lender = new LinkType(
            new Guid("7d77ac1d-4b43-4d28-a253-9c525d30fa9b"), 1, "Lender", "Lender");

        public static LinkType Limit = new LinkType(
            new Guid("5bb75c56-a853-44c7-9b67-1ce1e2684b9f"), 35, "Limit", "Limit");

        public static LinkType Location = new LinkType(
            new Guid("2228744f-d476-e211-88fe-0019b9b35da2"), 40, "Location", "Location");

        public static LinkType MarketingPartner = new LinkType(
            new Guid("34a845e6-0860-dd11-8b4e-0019b9b35da2"), 7, "MarketingPartner", "MarketingPartner");

        public static LinkType NonDealerBuyerSeller = new LinkType(
            new Guid("5bec3ba3-b530-e611-9417-ac162d7cbbd1"), 60, "NonDealerBuyerSeller", "NonDealerBuyerSeller");

        public static LinkType Notification = new LinkType(
            new Guid("01788bc7-f9ad-489a-95cc-0b95699a4dee"), 36, "Notification", "Notification");

        public static LinkType Ns_Flooring_Customer = new LinkType(
            new Guid("c8018bc1-b266-4263-b130-09c5a985cb3b"), 62, "Ns_Flooring_Customer", "Ns_Flooring_Customer");

        public static LinkType Ns_Flooring_Vendor = new LinkType(
            new Guid("8d955f84-009c-4b44-9cc8-45dc9444d2e8"), 63, "Ns_Flooring_Vendor", "Ns_Flooring_Vendor");

        public static LinkType OnlineBidder = new LinkType(
            new Guid("ada90dd9-0605-de11-b442-0019b9b35da2"), 19, "OnlineBidder", "OnlineBidder");

        public static LinkType OnlineBidderAsDealer = new LinkType(
            new Guid("2e8f4cc3-d50f-de11-b442-0019b9b35da2"), 22, "OnlineBidderAsDealer", "OnlineBidderAsDealer");

        public static LinkType OnlineVendor = new LinkType(
            new Guid("d9d714f6-7f72-481b-8936-94bd8187a62f"), 21, "OnlineVendor", "OnlineVendor");

        public static LinkType Part = new LinkType(
            new Guid("1f618dc9-fcbd-de11-ac66-0019b9b35da2"), 26, "Part", "Part");

        public static LinkType PartInventory = new LinkType(
            new Guid("1e618dc9-fcbd-de11-ac66-0019b9b35da2"), 25, "PartInventory", "PartInventory");

        public static LinkType PartOrder = new LinkType(
            new Guid("796e6381-73cf-4e25-8024-b48e01ddaf8d"), 46, "PartOrder", "PartOrder");

        public static LinkType PartType = new LinkType(
            new Guid("20618dc9-fcbd-de11-ac66-0019b9b35da2"), 27, "PartType", "PartType");

        public static LinkType Payoff = new LinkType(
            new Guid("18f5b68f-c1fb-dd11-b442-0019b9b35da2"), 14, "Payoff", "Payoff");

        public static LinkType ProgramVehicle = new LinkType(
            new Guid("7282b50a-29e0-4d17-b86a-bdb4b5f02322"), 54, "ProgramVehicle", "ProgramVehicle");

        public static LinkType ProxyBid = new LinkType(
            new Guid("7c776287-f17f-e011-8852-0019b9b35da2"), 34, "ProxyBid", "ProxyBid");

        public static LinkType Repair = new LinkType(
            new Guid("ed1f0ffa-2c0d-df11-aa9e-0019b9b35da2"), 28, "Repair", "Repair");

        public static LinkType RepairItem = new LinkType(
            new Guid("1d618dc9-fcbd-de11-ac66-0019b9b35da2"), 24, "RepairItem", "RepairItem");

        public static LinkType RepoBroker = new LinkType(
            new Guid("de2ef69b-a20a-468d-a7b4-7cce45698ffe"), 42, "RepoBroker", "RepoBroker");

        public static LinkType RepoRequest = new LinkType(
            new Guid("deb9b885-c1fb-dd11-b442-0019b9b35da2"), 11, "RepoRequest", "RepoRequest");

        public static LinkType SalesLead = new LinkType(
            new Guid("35a845e6-0860-dd11-8b4e-0019b9b35da2"), 8, "SalesLead", "SalesLead");

        public static LinkType Seller = new LinkType(
            new Guid("4636d510-51bc-42fc-823b-4ddeb44def67"), 47, "Seller", "Seller");

        public static LinkType SourceCompany = new LinkType(
            new Guid("19f5b68f-c1fb-dd11-b442-0019b9b35da2"), 15, "SourceCompany", "SourceCompany");

        public static LinkType SubLocation = new LinkType(
            new Guid("4f7a4071-d476-e211-88fe-0019b9b35da2"), 41, "SubLocation", "SubLocation");

        public static LinkType Transit = new LinkType(
            new Guid("f228aa98-c1fb-dd11-b442-0019b9b35da2"), 16, "Transit", "Transit");

        public static LinkType UserAccountGroup = new LinkType(
            new Guid("02575c06-f3db-4f20-ae2e-f189594cdb12"), 57, "UserAccountGroup", "UserAccountGroup");

        public static LinkType Vehicle = new LinkType(
            new Guid("1e895836-64ef-4061-bcd0-84cc9ab9af8d"), 0, "Vehicle", "Vehicle");

        public static LinkType VehicleBrand = new LinkType(
            new Guid("efebdaca-ddea-4172-8e00-b22ad644aafb"), 29, "VehicleBrand", "VehicleBrand");

        public static LinkType VehicleDoc = new LinkType(
            new Guid("c6695055-1360-4dff-8a2e-17a446c09585"), 33, "VehicleDoc", "VehicleDoc");

        public static LinkType VehicleModel = new LinkType(
            new Guid("a62e9d04-9cee-4a1e-8438-9a93af1bd77d"), 30, "VehicleModel", "VehicleModel");

        public static LinkType VehicleReserve = new LinkType(
            new Guid("b9c9b5f4-c82a-41d0-9bf7-f7de35214796"), 38, "VehicleReserve", "VehicleReserve");

        public static LinkType Vendor = new LinkType(
            new Guid("f328aa98-c1fb-dd11-b442-0019b9b35da2"), 17, "Vendor", "Vendor");

        public static LinkType VINCode = new LinkType(
            new Guid("e3a9cdba-dfd5-4345-999c-b575dce715e5"), 51, "VINCode", "VINCode");

        public static LinkType VINCodeVehicleModel = new LinkType(
            new Guid("ac717f03-2e15-4e51-a126-da50932d6cb6"), 52, "VINCodeVehicleModel", "VINCodeVehicleModel");

        public static LinkType VINCodeVehicleModelTemp = new LinkType(
            new Guid("e6fa4230-7b92-469e-86f0-9fafe68bc10a"), 53, "VINCodeVehicleModelTemp", "VINCodeVehicleModelTemp");

        public static LinkType Vqs_Vehicle = new LinkType(
            new Guid("fcfd39fe-bc9c-42c2-a0d0-ee855ca1b250"), 59, "Vqs_Vehicle", "Vqs_Vehicle");

        public static LinkType WorkItem = new LinkType(
            new Guid("7fcdeea3-0a6d-4670-b1e2-b42d4de58faf"), 39, "WorkItem", "WorkItem");
        
        
        public static IEnumerable<LinkType> GetValues()
        {
            yield return AccountingCustomer;
            yield return AccountingVendor;
            yield return Activity;
            yield return AMSUser;
            yield return Auction;
            yield return AuctionVendor;
            yield return Bidder;
            yield return Billing;
            yield return BookProviderBrand;
            yield return BookProviderModel;
            yield return Buyer;
            yield return Carrier;
            yield return CarrierAlternate;
            yield return Consumer;
            yield return Contact;
            yield return DataProvider;
            yield return Dealer;
            yield return DealerAlternateAddress;
            yield return FollowUp;
            yield return Franchise;
            yield return FreightForwarder;
            yield return General;
            yield return Guest;
            yield return IfBid;
            yield return Inspection;
            yield return Invoice;
            yield return Lender;
            yield return Limit;
            yield return Location;
            yield return MarketingPartner;
            yield return NonDealerBuyerSeller;
            yield return Notification;
            yield return Ns_Flooring_Customer;
            yield return Ns_Flooring_Vendor;
            yield return OnlineBidder;
            yield return OnlineBidderAsDealer;
            yield return OnlineVendor;
            yield return Part;
            yield return PartInventory;
            yield return PartOrder;
            yield return PartType;
            yield return Payoff;
            yield return ProgramVehicle;
            yield return ProxyBid;
            yield return Repair;
            yield return RepairItem;
            yield return RepoBroker;
            yield return RepoRequest;
            yield return SalesLead;
            yield return Seller;
            yield return SourceCompany;
            yield return SubLocation;
            yield return Transit;
            yield return UserAccountGroup;
            yield return Vehicle;
            yield return VehicleBrand;
            yield return VehicleDoc;
            yield return VehicleModel;
            yield return VehicleReserve;
            yield return Vendor;
            yield return VINCode;
            yield return VINCodeVehicleModel;
            yield return VINCodeVehicleModelTemp;
            yield return Vqs_Vehicle;
            yield return WorkItem;
        }
    }
}
