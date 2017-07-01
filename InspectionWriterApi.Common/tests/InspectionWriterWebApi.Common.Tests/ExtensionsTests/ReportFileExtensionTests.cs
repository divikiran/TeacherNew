using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InspectionWriterWebApi.Extensions;
using InspectionWriterWebApi.Models;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions;
using Xunit.Sdk;

namespace InspectionWriterWebApi.Common.Tests.ExtensionsTests
{
    public class ReportFileExtensionTests
    {
        private readonly ITestOutputHelper output;

        public ReportFileExtensionTests( ITestOutputHelper outputHelper )
        {
            output = outputHelper;
        }

        [Fact]
        public void AuctionLabelDisplayText()
        {
            var rpt = ReportFile.AuctionLabel;
            Assert.Equal( "AuctionLabel", $"{rpt}" );
            Assert.Equal( "Auction Label", rpt.GetDisplayText() );
        }

        [Fact]
        public void AuctionLabelFileName()
        {
            var rpt = ReportFile.AuctionLabel;
            Assert.Equal( "AuctionLabel", $"{rpt}" );
            Assert.Equal( "AuctionLabel.rpt", rpt.GetReportFileName() );
        }
    }
}
