using System;

namespace NPAInspectionWriter.DTO
{
    public class VehicleDTO : BaseDTO
    {
        public string VIN { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string MilesHours { get; set; }
        public string AccountNumber { get; set; }
        public string StockNumber { get; set; }
        public string StartingBid { get; set; }
        public string Reserve { get; set; }
        public string BuyNowAmount { get; set; }
        public string BidIncrement { get; set; }
        public string Transit { get; set; }
        public string SellerCode { get; set; }
        public string VehicleState { get; set; }
        public string DealerName { get; set; }
        public string AuctionItem { get; set; }
        public bool ShowDamageType { get; set; }
        public string DamageType { get; set; }
        public bool ShowLossType { get; set; }
        public string LossType { get; set; }
        public int? InstaVINAvailable { get; set; }
        public decimal? BookValue { get; set; }
        public int? InspectionScore { get; set; }
        public string Weight { get; set; }

        public Guid? VehicleID { get; set; }
        public Guid? ProductSegmentID { get; set; }
        public Guid? DealerID { get; set; }
        public Guid? LenderID { get; set; }
        public Guid? AuctionID { get; set; }
        public Guid? PictureID { get; set; }
        public Guid? VehicleBrandID { get; set; }
        public Guid? VehicleModelID { get; set; }
        public Guid? VehicleCategoryID { get; set; }
        public Guid? LocationID { get; set; }
        public Guid? SubLocationID { get; set; }

        public DateTime? SaleDate { get; set; }
        public string SaleAmount { get; set; }
        public DateTime? PayoffDate { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
    }
}
