using NPAInspectionWriter.Helpers;

namespace NPAInspectionWriter.Models
{
    public enum ReportFile
    {
        [DisplayText( "Auction Label" )]
        [ReportFileName( "AuctionLabel.rpt" )]
        AuctionLabel,

        [DisplayText( "Compression Test Label" )]
        [ReportFileName( "CompressionTestLabel.rpt" )]
        CompressionTestLabel,

        [DisplayText( "Transfer Label" )]
        [ReportFileName( "VehicleTransferLabel.rpt" )]
        TransferLabel,

        [DisplayText( "Transfer Label w/ comp" )]
        [ReportFileName( "VehicleTransferLabelWithComp.rpt" )]
        VehicleTransferLabelWithComp,

        [DisplayText( "Vehicle Label")]
        [ReportFileName( "VehicleLabel.rpt" )]
        VehicleLabel,

        [DisplayText( "Vehicle Label w/ comp" )]
        [ReportFileName( "VehicleLabelWithComp.rpt" )]
        VehicleLabelWithComp
    }
}
