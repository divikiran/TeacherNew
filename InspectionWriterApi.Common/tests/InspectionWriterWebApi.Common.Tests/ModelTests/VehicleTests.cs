using InspectionWriterWebApi.Common.Tests.Data;
using InspectionWriterWebApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace InspectionWriterWebApi.Common.Tests
{
    public class VehicleTests
    {
        private readonly ITestOutputHelper output;

        public VehicleTests( ITestOutputHelper outputHelper )
        {
            output = outputHelper;
        }

        [Theory]
        [ClassData( typeof( VehicleData ) )]
        public void VehicleSerializationTest( string jsonResponse, Vehicle vehicle )
        {
            var expected = JArray.Parse( jsonResponse ).First;
            var actual = JArray.FromObject( new Vehicle[] { vehicle } ).First;

            output.WriteLine( $"Expected Children: {expected.Children().Count()}" );
            output.WriteLine( $"Actual Children: {actual.Children().Count()}" );

            Assert.Equal( expected.Children().Count(), actual.Children().Count() );
            Assert.True( JToken.DeepEquals( expected, actual ) );

            // Replaced by DeepEquals. Leaving in for debugging purposes.
            //Assert.Equal( expected[ "vehicleId" ], actual[ "vehicleId" ] );
            //Assert.Equal( expected[ "vin" ], actual[ "vin" ] );
            //Assert.Equal( expected[ "stockNumber" ], actual[ "stockNumber" ] );
            //Assert.Equal( expected[ "year" ], actual[ "year" ] );
            //Assert.Equal( expected[ "brand" ], actual[ "brand" ] );
            //Assert.Equal( expected[ "color" ], actual[ "color" ] );
            //Assert.Equal( expected[ "model" ], actual[ "model" ] );
            //Assert.Equal( expected[ "vehicleModelId" ], actual[ "vehicleModelId" ] );
            //Assert.Equal( expected[ "vehicleCategoryId" ], actual[ "vehicleCategoryId" ] );
            //Assert.Equal( expected[ "milesHours" ], actual[ "milesHours" ] );
            //Assert.Equal( expected[ "vehicleComments" ], actual[ "vehicleComments" ] );
            //Assert.Equal( expected[ "publicAuctionNotes" ], actual[ "publicAuctionNotes" ] );
            //Assert.Equal( expected[ "auctioneerNotes" ], actual[ "auctioneerNotes" ] );
            //Assert.Equal( expected[ "vehicleState" ], actual[ "vehicleState" ] );
            //Assert.Equal( expected[ "seller" ], actual[ "seller" ] );
            //Assert.Equal( expected[ "salesRep" ], actual[ "salesRep" ] );
            //Assert.Equal( expected[ "salesRepEmail" ], actual[ "salesRepEmail" ] );
            //Assert.Equal( expected[ "locationId" ], actual[ "locationId" ] );
            //Assert.Equal( expected[ "allowNewInspections" ], actual[ "allowNewInspections" ] );
        }

        [Theory]
        [ClassData( typeof( VehicleData ) )]
        public void VehicleDeserializationTest( string jsonResponse, Vehicle vehicle )
        {
            var response = JsonConvert.DeserializeObject<IEnumerable<Vehicle>>( jsonResponse ).FirstOrDefault();

            Assert.Equal( vehicle.Id, response.Id );
            Assert.Equal( vehicle.Vin, response.Vin );
            Assert.Equal( vehicle.StockNumber, response.StockNumber );
            Assert.Equal( vehicle.Year, response.Year );
            Assert.Equal( vehicle.Brand, response.Brand );
            Assert.Equal( vehicle.Color, response.Color );
            Assert.Equal( vehicle.Model, response.Model );
            Assert.Equal( vehicle.VehicleModelId, response.VehicleModelId );
            Assert.Equal( vehicle.VehicleCategoryId, response.VehicleCategoryId );
            Assert.Equal( vehicle.MilesHours, response.MilesHours );
            Assert.Equal( vehicle.PictureId, response.PictureId );
            Assert.Equal( vehicle.VehicleComments, response.VehicleComments, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true );
            Assert.Equal( vehicle.PublicAuctionNotes, response.PublicAuctionNotes );
            Assert.Equal( vehicle.AuctioneerNotes, response.AuctioneerNotes );
            Assert.Equal( vehicle.VehicleState, response.VehicleState );
            Assert.Equal( vehicle.Seller, response.Seller );
            Assert.Equal( vehicle.SalesRep, response.SalesRep );
            Assert.Equal( vehicle.SalesRepEmail, response.SalesRepEmail );
            Assert.Equal( vehicle.LocationId, response.LocationId );
            Assert.Equal( vehicle.AllowNewInspections, response.AllowNewInspections );
        }
    }
}
