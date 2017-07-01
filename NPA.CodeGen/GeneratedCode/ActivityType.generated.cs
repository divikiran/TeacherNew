using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace NPA.CodeGen
{
    public partial class ActivityType : IEquatable<ActivityType>, IComparable<ActivityType>
    {
        public Guid ActivityTypeId { get; private set; }
        public string DisplayName { get; private set; }
        public string ShortName { get; private set; }

        private ActivityType() { }

        private ActivityType(Guid activityTypeId, string activityTypeName, string displayName)
        {
            this.ActivityTypeId = activityTypeId;
            this.ShortName = activityTypeName;
            this.DisplayName = displayName;
        }

        public bool Equals(ActivityType other)
        {
            return this.ActivityTypeId.Equals(other.ActivityTypeId);
        }

        public int CompareTo(ActivityType other)
        {
            return this.ShortName.CompareTo(other.ShortName);
        }

        public override string ToString()
        {
            return DisplayName;
        }
        
        public static ActivityType FromGuid(Guid guid)
        {
            return GetValues().SingleOrDefault(v => v.ActivityTypeId == guid);
        }

        public static ActivityType FromDisplayName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.DisplayName, name, true) == 0);
        }

        public static ActivityType FromShortName(string name)
        {
            return GetValues().SingleOrDefault(v => string.Compare(v.ShortName, name, true) == 0);
        }

        public static ActivityType ARIssue = new ActivityType(
            new Guid("93307ec2-13f7-e311-9404-ac162d7cbbd1"), "ARIssue", "A/R Issue");

        public static ActivityType AimStdNotification = new ActivityType(
            new Guid("c39e07c0-83d5-e311-b02d-005056c00008"), "AimStdNotification", "AimStdNotification");

        public static ActivityType AMS = new ActivityType(
            new Guid("49125b9f-0269-4a3e-abc2-698df6227d27"), "AMS", "AMS");

        public static ActivityType BuyerIfbidSaleNotificationDirect = new ActivityType(
            new Guid("ead0014a-3733-dd11-9dca-0019b9b35da2"), "BuyerIfbidSaleNotificationDirect", "Buyer Ifbid Sale Notification - Direct");

        public static ActivityType BuyerIfbidSaleNotificationEmail = new ActivityType(
            new Guid("ebd0014a-3733-dd11-9dca-0019b9b35da2"), "BuyerIfbidSaleNotificationEmail", "Buyer Ifbid Sale Notification - Email");

        public static ActivityType Call = new ActivityType(
            new Guid("fd464e7a-32a6-4550-9d51-c220c6361799"), "Call", "Call");

        public static ActivityType ClientNote = new ActivityType(
            new Guid("189b3f62-4f93-45a3-a346-e1aa1f683bb4"), "ClientNote", "Client Note");

        public static ActivityType Email = new ActivityType(
            new Guid("0ed15393-1cdc-4074-a416-e99ebd6be1a1"), "Email", "Email");

        public static ActivityType ExtraParts = new ActivityType(
            new Guid("1b72d57c-ba09-e711-80df-1c98ec191a2b"), "ExtraParts", "Extra Parts");

        public static ActivityType Fax = new ActivityType(
            new Guid("7af4b3f4-1822-49cc-82a6-b140ceecf569"), "Fax", "Fax");

        public static ActivityType IfBid = new ActivityType(
            new Guid("e2977e04-8d1f-44ef-b526-3a33c77e093a"), "IfBid", "IfBid");

        public static ActivityType IncomingCall = new ActivityType(
            new Guid("b4e7cd96-3a63-dd11-8b4e-0019b9b35da2"), "IncomingCall", "Incoming Call");

        public static ActivityType Inspection = new ActivityType(
            new Guid("c7fb6fb4-4789-467c-8734-950d960a2b3f"), "Inspection", "Inspection");

        public static ActivityType LeadTransfer = new ActivityType(
            new Guid("6e6c3897-e9a5-4591-8a0c-b90dc986d7c4"), "LeadTransfer", "Lead Transfer");

        public static ActivityType LenderApprovalOverdue = new ActivityType(
            new Guid("cc726f97-ac2c-df11-9f83-000c297eaaa7"), "LenderApprovalOverdue", "Lender Approval Overdue");

        public static ActivityType LenderNoReserve = new ActivityType(
            new Guid("e290a78b-c836-43d3-983a-b7be3ba3e9d0"), "LenderNoReserve", "Lender No-Reserve");

        public static ActivityType LenderRepairApproval = new ActivityType(
            new Guid("ee488136-d02a-41c2-a046-2c68a111c168"), "LenderRepairApproval", "Lender Repair Approval");

        public static ActivityType LenderRepairDeclined = new ActivityType(
            new Guid("8048fb7d-3027-e411-9408-ac162d7cbbd1"), "LenderRepairDeclined", "Lender Repair Declined");

        public static ActivityType LenderRepairNote = new ActivityType(
            new Guid("ef0ac882-9428-df11-8efa-0019b9b35da2"), "LenderRepairNote", "Lender Repair Note");

        public static ActivityType ManagerApproved = new ActivityType(
            new Guid("c3dd8c24-6d5f-48df-8d62-275ca4b7e8e8"), "ManagerApproved", "Manager Approved");

        public static ActivityType ManagerDenied = new ActivityType(
            new Guid("cd5f175e-d77a-4f5e-8ba7-8ba112bfdb76"), "ManagerDenied", "Manager Denied");

        public static ActivityType Memo = new ActivityType(
            new Guid("6e77c750-bf35-de11-bc96-0019b9b35da2"), "Memo", "Memo");

        public static ActivityType PartsBackOrdered = new ActivityType(
            new Guid("7f48fb7d-3027-e411-9408-ac162d7cbbd1"), "PartsBackOrdered", "Parts Back-Ordered");

        public static ActivityType PossibleMileageDiscrepancy = new ActivityType(
            new Guid("b5074069-742f-4eaf-9ede-d5640984b252"), "PossibleMileageDiscrepancy", "Possible Mileage Discrepancy");

        public static ActivityType ProxyBidStatus = new ActivityType(
            new Guid("2ed06d8d-f17f-e011-8852-0019b9b35da2"), "ProxyBidStatus", "Proxy Bid Status");

        public static ActivityType SalvageHistory = new ActivityType(
            new Guid("073e0809-b886-4a4b-b1a3-ed7c51a170e7"), "SalvageHistory", "Salvage History");

        public static ActivityType SellerApproved = new ActivityType(
            new Guid("aab57510-6c09-4526-87e4-69331bafd31c"), "SellerApproved", "Seller Approved");

        public static ActivityType SellerDenied = new ActivityType(
            new Guid("0955439c-f64f-4118-8b8f-df6df579fae3"), "SellerDenied", "Seller Denied");

        public static ActivityType VehicleHistoryCheck = new ActivityType(
            new Guid("350ac372-8ff7-e611-80de-1c98ec191a2b"), "VehicleHistoryCheck", "Vehicle History Check");

        public static ActivityType VehicleIssue = new ActivityType(
            new Guid("d5b361ea-594f-4162-b07e-d0d3013afdbd"), "VehicleIssue", "Vehicle Issue");

        public static ActivityType VehicleModifiedAfterSale = new ActivityType(
            new Guid("20a7c347-c1a3-42c3-8dd1-bd6d84f449e8"), "VehicleModifiedAfterSale", "Vehicle Modified After Sale");

        public static ActivityType VehicleNote = new ActivityType(
            new Guid("a9d5af77-ee81-4b2e-9e71-920a8dd8bb69"), "VehicleNote", "Vehicle Note");

        public static ActivityType VehiclePickup = new ActivityType(
            new Guid("9035475a-1c96-4bdb-b6e4-3332b458e91d"), "VehiclePickup", "Vehicle Pickup");

        public static ActivityType VehicleTransfer = new ActivityType(
            new Guid("056cc87c-5249-41c5-959d-ee6ac0565e6c"), "VehicleTransfer", "Vehicle Transfer");

        public static ActivityType Visit = new ActivityType(
            new Guid("58ec0630-e38e-4e90-81ff-4045b0e57fbd"), "Visit", "Visit");

        public static ActivityType WebInfoChange = new ActivityType(
            new Guid("24fab63c-284e-4fe4-80f6-b4e72df2d055"), "WebInfoChange", "Web Info Change");
        
        
        public static IEnumerable<ActivityType> GetValues()
        {
            yield return ARIssue;
            yield return AimStdNotification;
            yield return AMS;
            yield return BuyerIfbidSaleNotificationDirect;
            yield return BuyerIfbidSaleNotificationEmail;
            yield return Call;
            yield return ClientNote;
            yield return Email;
            yield return ExtraParts;
            yield return Fax;
            yield return IfBid;
            yield return IncomingCall;
            yield return Inspection;
            yield return LeadTransfer;
            yield return LenderApprovalOverdue;
            yield return LenderNoReserve;
            yield return LenderRepairApproval;
            yield return LenderRepairDeclined;
            yield return LenderRepairNote;
            yield return ManagerApproved;
            yield return ManagerDenied;
            yield return Memo;
            yield return PartsBackOrdered;
            yield return PossibleMileageDiscrepancy;
            yield return ProxyBidStatus;
            yield return SalvageHistory;
            yield return SellerApproved;
            yield return SellerDenied;
            yield return VehicleHistoryCheck;
            yield return VehicleIssue;
            yield return VehicleModifiedAfterSale;
            yield return VehicleNote;
            yield return VehiclePickup;
            yield return VehicleTransfer;
            yield return Visit;
            yield return WebInfoChange;
        }
    }
}
