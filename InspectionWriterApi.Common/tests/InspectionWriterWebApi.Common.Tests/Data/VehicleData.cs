using InspectionWriterWebApi.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace InspectionWriterWebApi.Common.Tests.Data
{
    public class VehicleData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator() => _vehicles.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerable<object[]> _vehicles
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        "[{\"vehicleId\":\"6ab59e9f-c2e9-4e43-a889-0cf97cc80be8\",\"vin\":\"JY4AM07Y57C039999\",\"stockNumber\":\"130-10063516\",\"year\":2007,\"brand\":\"YAMAHA\",\"color\":\"GRAY\",\"model\":\"YFM70RSEW RAPTOR SE\",\"vehicleModelId\":\"c40c0907-c1fa-42b3-885e-e00ee02a31a7\",\"vehicleCategoryId\":\"6b95054d-9ebb-45c5-90a4-1f57c162a388\",\"milesHours\":\"OHV\",\"pictureId\":\"7a03c5dd-77f2-4186-940a-408d9fc0958c\",\"vehicleComments\":\"RICK KRUMES\r\n4231 SAN RAMON DRIVE\r\nCORONA, CA 92882\r\n 714-342-1040\r\n7% CAP 350\",\"publicAuctionNotes\":\"\",\"auctioneerNotes\":\"RSV: 2800.00 FIRM\r\n        RICK KRUMES\r\n\",\"vehicleState\":\"InAuction\",\"seller\":\"** CONSIGNMENTS NON-DEALERS\",\"salesRep\":\"Mike Murray\",\"salesRepEmail\":\"MMURRAY@NPAUCTIONS.COM\",\"locationId\":\"c4b1d148-57e2-4fd2-9dae-0121d40854a0\",\"allowNewInspections\":true}]",
                        new Vehicle()
                        {
                            Id = new Guid( "6ab59e9f-c2e9-4e43-a889-0cf97cc80be8" ),
                            Vin = "JY4AM07Y57C039999",
                            StockNumber = "130-10063516",
                            Year = 2007,
                            Brand = "YAMAHA",
                            Color = "GRAY",
                            Model = "YFM70RSEW RAPTOR SE",
                            VehicleModelId = "c40c0907-c1fa-42b3-885e-e00ee02a31a7",
                            VehicleCategoryId = "6b95054d-9ebb-45c5-90a4-1f57c162a388",
                            MilesHours = "OHV",
                            PictureId = new Guid ("7a03c5dd-77f2-4186-940a-408d9fc0958c" ),
                            VehicleComments = "RICK KRUMES\r\n4231 SAN RAMON DRIVE\r\nCORONA, CA 92882\r\n 714-342-1040\r\n7% CAP 350",
                            PublicAuctionNotes = "",
                            AuctioneerNotes = "RSV: 2800.00 FIRM\r\n        RICK KRUMES\r\n",
                            VehicleState = "InAuction",
                            Seller = "** CONSIGNMENTS NON-DEALERS",
                            SalesRep = "Mike Murray",
                            SalesRepEmail = "MMURRAY@NPAUCTIONS.COM",
                            LocationId = new Guid ( "c4b1d148-57e2-4fd2-9dae-0121d40854a0" ),
                            AllowNewInspections = true
                        }
                    }
                };
            }
        }
    }
}
