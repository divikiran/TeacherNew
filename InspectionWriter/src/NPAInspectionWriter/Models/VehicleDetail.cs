using Xamarin.Forms;

namespace NPAInspectionWriter.iOS.Models
{
    public class VehicleDetail
    {
        public string StockNumber { get; set; }

        public string VIN { get; set; }

        public ImageSource DefaultImageSource { get; set; }

        public int? Year { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string VehicleState { get; set; }

        public string MilesHours { get; set; }

        public string Color { get; set; }

        public string Seller { get; set; }

        public string SalesRep { get; set; }

        public bool AllowNewInspections { get; set; }
    }
}
