using NPAInspectionWriter.Helpers;

namespace NPAInspectionWriter.Helpers
{
    public enum SearchType
    {
        //TODO: Refractor to use AppResources
        [Placeholder("Enter Stock #")]
        [Description("Stock Number")]
        StockNumber,

        [Placeholder("Enter VIN #")]
        [Description("VIN")]
        VIN
    }
}
