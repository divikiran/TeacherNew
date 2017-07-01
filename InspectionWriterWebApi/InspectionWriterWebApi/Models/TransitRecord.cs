using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspectionWriterWebApi.Models
{
    public class TransitRecord
    {
        public Guid VehicleId { get; set; }
        public string Vin { get; set; }
        public string StockNo { get; set; }
        public int? TransitType { get; set; }
        public string TransitDate { get; set; }
        public string PickupDate { get; set; }
        public string ReceivingDate { get; set; }
        public string ReceivingUser { get; set; }
        public string CancelDate { get; set; }
    }
}