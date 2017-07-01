#if USE_MOCKS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPAInspectionWriter.Models;
using Newtonsoft.Json;
using NPAInspectionWriter.Models;
using Xamarin.Forms;

namespace NPAInspectionWriter.Services
{
#pragma warning disable 1998
    public class VehicleServiceMock : IVehicleService
    {
        public static bool UseVehicleWithoutInspections { get; set; }

        public async Task<IEnumerable<Inspection>> GetInspectionsAsync( Vehicle vehicle )
        {
            // TODO: Update Mock response with corrected output.
            string response = UseVehicleWithoutInspections ?
                "[]" :
                "{\"10064209\":[{\"inspectionId\":\"d7935c31-d157-4131-abec-91fdd555c627\",\"locationId\":\"c4b1d148-57e2-4fd2-9dae-0121d40854a0\",\"masterId\":\"357415cd-b96c-4075-a47d-1750e663e5e5\",\"master\":\"*CRUISER DOMESTIC/METRIC\",\"inspectionType\":\"Pre-Inspection\",\"inspectionTypeId\":1,\"inspectionUser\":\"tbrake\",\"score\":84,\"inspectionDate\":\"2016-05-02T21:44:11\",\"inspectionMilesHours\":\"11632\",\"inspectionColor\":\"BLK\"}]}";
            var settings = new JsonSerializerSettings()
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
            };
            return JsonConvert.DeserializeObject<Dictionary<string,IEnumerable<Inspection>>>( response, settings )["10064209"];
        }

        public async Task<IEnumerable<InspectionMaster>> GetMastersForVehicleAsync( Vehicle vehicle )
        {
            string response = UseVehicleWithoutInspections ?
                "[{\"masterId\":\"7f8b0fa7-23aa-4bdf-a3cd-dbea228d7e79\",\"displayName\":\"*SPORT M/C\",\"defaultForCategory\":true,\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"8713ae8a-de8c-4ead-a5a4-3683c5990464\",\"c89414b7-7991-4cb9-9551-728bb0a93be5\",\"0e96ed45-e72e-4d42-ab53-53d8e310bba3\"]},{\"masterId\":\"e2795052-2263-49e5-932b-0304d9e9ca50\",\"displayName\":\"SALVAGE SPORT BIKE\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"9c4aa8aa-75c2-4fab-9448-d48bac25f7d1\",\"714b3043-4565-4da8-8d97-159bac97e5f6\"]},{\"masterId\":\"3b870a28-4a7c-dc11-a551-0019b9b35da2\",\"displayName\":\"FACTORY - SPORT - CRATED\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"38870a28-4a7c-dc11-a551-0019b9b35da2\"]},{\"masterId\":\"27ffe76a-f980-40b7-b885-fc97e4b11cc2\",\"displayName\":\"AVP - SPORT M/C\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"be9c7ebb-a39e-4d34-aecf-4e314da51b42\",\"7ed299e9-6013-4357-bd79-192df8306ff3\",\"82259bf3-04c1-4f80-94bd-87f1e1f9ee90\"]},{\"masterId\":\"f849dc93-506f-4c19-819f-e285ee49e64d\",\"displayName\":\"FACTORY-DUCATI SPORT\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"452f4162-9ae4-4cbd-98dd-f7e07a2047f4\",\"0a724460-3103-4c90-953a-235e732333af\"]},{\"masterId\":\"2f9a57d5-40e4-48b1-b930-dbedfad81f40\",\"displayName\":\"*SPYDER/FRONT WHEEL TRIKE/OEM\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"5582f047-8984-4504-90cd-2eb6c65dbb6c\",\"52a8b436-d027-4b40-8f08-45fc1c4550fa\",\"dad3ac68-6177-410b-a883-b6c67dd55589\"]},{\"masterId\":\"20ee4a37-2f10-463f-ad29-b84a312fbf99\",\"displayName\":\"*SPYDER/FRONT WHEEL TRIKE\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"8713ae8a-de8c-4ead-a5a4-3683c5990464\",\"1f897ac6-701f-4380-ba6e-6180ffa220ea\",\"0e96ed45-e72e-4d42-ab53-53d8e310bba3\"]},{\"masterId\":\"fa1a44ff-0e8f-45a1-b45a-a5be00c0a867\",\"displayName\":\"CQC SPORT M/C\",\"maxInspectionScore\":100,\"hidden\":true,\"requiredCategoryIds\":[\"cc303251-fc71-410e-85d0-fd94ca263596\",\"2cebc59c-2a8d-48b7-a309-3059a27cbcc6\",\"ee9bb860-cbdf-4871-83ec-233cfd81da28\"]},{\"masterId\":\"75581a6e-0825-4848-93c6-fa860994e9ab\",\"displayName\":\"SPORT BIKES\",\"maxInspectionScore\":100,\"hidden\":true,\"requiredCategoryIds\":[\"90fd72e1-d444-4ce9-92ed-1f4f97214b8e\",\"1bf34c01-e516-4962-8554-cf69fcc03fea\"]},{\"masterId\":\"43b08453-d94a-48a3-8d83-0b3cd3028324\",\"displayName\":\"FACTORY - SPORT - UNCRATED\",\"maxInspectionScore\":100,\"hidden\":true,\"requiredCategoryIds\":[\"b0ac5ed3-cc84-438b-aba3-2afa6cde9e6f\",\"34d05ae5-f0c2-432e-a000-c36d4f058aa3\"]},{\"masterId\":\"bc98a949-421f-4b5a-be03-80e932d55bed\",\"displayName\":\"SPORT BIKES\",\"maxInspectionScore\":100,\"hidden\":true,\"requiredCategoryIds\":[\"8ec9c377-af33-4324-ab8e-50b52657e341\",\"40ff63a3-89d1-4f88-998c-9bb53f95252d\"]}]" :
                "[{\"masterId\":\"357415cd-b96c-4075-a47d-1750e663e5e5\",\"displayName\":\"*CRUISER DOMESTIC/METRIC\",\"defaultForCategory\":true,\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"8713ae8a-de8c-4ead-a5a4-3683c5990464\",\"0e96ed45-e72e-4d42-ab53-53d8e310bba3\",\"c89414b7-7991-4cb9-9551-728bb0a93be5\"]},{\"masterId\":\"95c074d2-3401-4957-bbc2-5a1cd3f3cd59\",\"displayName\":\"CRUISER ALL\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"8ec9c377-af33-4324-ab8e-50b52657e341\",\"40ff63a3-89d1-4f88-998c-9bb53f95252d\"]},{\"masterId\":\"1985801f-2e83-4906-a624-2c986ae97487\",\"displayName\":\"CUSTOM CRUISER PARTS\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[]},{\"masterId\":\"c17ce805-4a7c-dc11-a551-0019b9b35da2\",\"displayName\":\"FACTORY - CRUISER - CRATED\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"be7ce805-4a7c-dc11-a551-0019b9b35da2\"]},{\"masterId\":\"d4a5bfbd-e025-40ce-827c-9ea424888dd2\",\"displayName\":\"SALVAGE CRUISER\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"9c4aa8aa-75c2-4fab-9448-d48bac25f7d1\",\"714b3043-4565-4da8-8d97-159bac97e5f6\"]},{\"masterId\":\"b8c5ff78-0053-413f-b586-95af41d9649a\",\"displayName\":\"AVP CRUISER DOMESTIC/METRIC\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"be9c7ebb-a39e-4d34-aecf-4e314da51b42\",\"7ed299e9-6013-4357-bd79-192df8306ff3\",\"82259bf3-04c1-4f80-94bd-87f1e1f9ee90\"]},{\"masterId\":\"79e1ec5e-c14f-4daa-b7e9-8d6f801739f1\",\"displayName\":\"TEST ONLY\",\"maxInspectionScore\":100,\"hidden\":true,\"requiredCategoryIds\":[\"8ec9c377-af33-4324-ab8e-50b52657e341\",\"40ff63a3-89d1-4f88-998c-9bb53f95252d\"]},{\"masterId\":\"483450cd-033a-40d5-9d07-b264d7f58004\",\"displayName\":\"LONG CRUISER\",\"maxInspectionScore\":100,\"hidden\":true,\"requiredCategoryIds\":[\"4cf64331-13fa-4c35-abe6-e56447930a2d\",\"1bf34c01-e516-4962-8554-cf69fcc03fea\"]},{\"masterId\":\"357b34e6-d87f-487c-9b15-29dc4f33dd72\",\"displayName\":\"CQC CRUISER\",\"maxInspectionScore\":100,\"hidden\":true,\"requiredCategoryIds\":[\"cc303251-fc71-410e-85d0-fd94ca263596\",\"2cebc59c-2a8d-48b7-a309-3059a27cbcc6\",\"ee9bb860-cbdf-4871-83ec-233cfd81da28\"]},{\"masterId\":\"fb40caa5-a227-4dab-8eea-21241a650168\",\"displayName\":\"FACTORY - CRUISER - UNCRATED\",\"maxInspectionScore\":100,\"hidden\":true,\"requiredCategoryIds\":[\"c5f0e457-191f-4e19-b453-4253efc7bc76\",\"ff400175-edb5-47fb-9d75-96a1c4366392\"]},{\"masterId\":\"206d8cf7-c0d6-4a56-8a44-749ef7840395\",\"displayName\":\"H-D WEB M/C--CRUISER\",\"maxInspectionScore\":100,\"hidden\":true,\"requiredCategoryIds\":[\"90fd72e1-d444-4ce9-92ed-1f4f97214b8e\",\"1bf34c01-e516-4962-8554-cf69fcc03fea\"]}]";
            return JsonConvert.DeserializeObject<IEnumerable<InspectionMaster>>( response );
        }

        public async Task<ImageSource> GetPrimaryPictureAsync( Vehicle vehicle )
        {
            return ImageSource.FromResource( "NPAInspectionWriter.Assets.Images.no-image.jpg", GetType() );
        }

        public async Task<Guid> GetPrimaryPictureIdAsync( Vehicle vehicle )
        {
            return UseVehicleWithoutInspections ? new Guid( "aaa99a43-d2b5-e311-93fd-ac162d7cbbd1" ) : new Guid( "d0c726a0-0b64-4617-b89d-52ed5fade66e" );
        }

        public async Task<InspectionTypesAndMasters> GetAvailableInspectionTypesAndMastersAsync( Vehicle vehicle )
        {
            string response = UseVehicleWithoutInspections ?
                "{\"masters\":[{\"masterId\":\"7f8b0fa7-23aa-4bdf-a3cd-dbea228d7e79\",\"displayName\":\"*SPORT M/C\",\"defaultForCategory\":true,\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"8713ae8a-de8c-4ead-a5a4-3683c5990464\",\"c89414b7-7991-4cb9-9551-728bb0a93be5\",\"0e96ed45-e72e-4d42-ab53-53d8e310bba3\"]},{\"masterId\":\"e2795052-2263-49e5-932b-0304d9e9ca50\",\"displayName\":\"SALVAGE SPORT BIKE\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"9c4aa8aa-75c2-4fab-9448-d48bac25f7d1\",\"714b3043-4565-4da8-8d97-159bac97e5f6\"]},{\"masterId\":\"3b870a28-4a7c-dc11-a551-0019b9b35da2\",\"displayName\":\"FACTORY - SPORT - CRATED\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"38870a28-4a7c-dc11-a551-0019b9b35da2\"]},{\"masterId\":\"27ffe76a-f980-40b7-b885-fc97e4b11cc2\",\"displayName\":\"AVP - SPORT M/C\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"be9c7ebb-a39e-4d34-aecf-4e314da51b42\",\"7ed299e9-6013-4357-bd79-192df8306ff3\",\"82259bf3-04c1-4f80-94bd-87f1e1f9ee90\"]},{\"masterId\":\"f849dc93-506f-4c19-819f-e285ee49e64d\",\"displayName\":\"FACTORY-DUCATI SPORT\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"452f4162-9ae4-4cbd-98dd-f7e07a2047f4\",\"0a724460-3103-4c90-953a-235e732333af\"]},{\"masterId\":\"2f9a57d5-40e4-48b1-b930-dbedfad81f40\",\"displayName\":\"*SPYDER/FRONT WHEEL TRIKE/OEM\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"5582f047-8984-4504-90cd-2eb6c65dbb6c\",\"52a8b436-d027-4b40-8f08-45fc1c4550fa\",\"dad3ac68-6177-410b-a883-b6c67dd55589\"]},{\"masterId\":\"20ee4a37-2f10-463f-ad29-b84a312fbf99\",\"displayName\":\"*SPYDER/FRONT WHEEL TRIKE\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"8713ae8a-de8c-4ead-a5a4-3683c5990464\",\"1f897ac6-701f-4380-ba6e-6180ffa220ea\",\"0e96ed45-e72e-4d42-ab53-53d8e310bba3\"]},{\"masterId\":\"fa1a44ff-0e8f-45a1-b45a-a5be00c0a867\",\"displayName\":\"CQC SPORT M/C\",\"maxInspectionScore\":100,\"hidden\":true,\"requiredCategoryIds\":[\"cc303251-fc71-410e-85d0-fd94ca263596\",\"2cebc59c-2a8d-48b7-a309-3059a27cbcc6\",\"ee9bb860-cbdf-4871-83ec-233cfd81da28\"]},{\"masterId\":\"75581a6e-0825-4848-93c6-fa860994e9ab\",\"displayName\":\"SPORT BIKES\",\"maxInspectionScore\":100,\"hidden\":true,\"requiredCategoryIds\":[\"90fd72e1-d444-4ce9-92ed-1f4f97214b8e\",\"1bf34c01-e516-4962-8554-cf69fcc03fea\"]},{\"masterId\":\"43b08453-d94a-48a3-8d83-0b3cd3028324\",\"displayName\":\"FACTORY - SPORT - UNCRATED\",\"maxInspectionScore\":100,\"hidden\":true,\"requiredCategoryIds\":[\"b0ac5ed3-cc84-438b-aba3-2afa6cde9e6f\",\"34d05ae5-f0c2-432e-a000-c36d4f058aa3\"]},{\"masterId\":\"bc98a949-421f-4b5a-be03-80e932d55bed\",\"displayName\":\"SPORT BIKES\",\"maxInspectionScore\":100,\"hidden\":true,\"requiredCategoryIds\":[\"8ec9c377-af33-4324-ab8e-50b52657e341\",\"40ff63a3-89d1-4f88-998c-9bb53f95252d\"]}],\"types\":[{\"inspectionTypeId\":2,\"displayName\":\"Post-Inspection\"},{\"inspectionTypeId\":3,\"displayName\":\"Redemption\"},{\"inspectionTypeId\":4,\"displayName\":\"Transfer Inspection\"}]}" :
                "{\"masters\":[{\"masterId\":\"357415cd-b96c-4075-a47d-1750e663e5e5\",\"displayName\":\"*CRUISER DOMESTIC/METRIC\",\"defaultForCategory\":true,\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"8713ae8a-de8c-4ead-a5a4-3683c5990464\",\"0e96ed45-e72e-4d42-ab53-53d8e310bba3\",\"c89414b7-7991-4cb9-9551-728bb0a93be5\"]},{\"masterId\":\"95c074d2-3401-4957-bbc2-5a1cd3f3cd59\",\"displayName\":\"CRUISER ALL\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"8ec9c377-af33-4324-ab8e-50b52657e341\",\"40ff63a3-89d1-4f88-998c-9bb53f95252d\"]},{\"masterId\":\"1985801f-2e83-4906-a624-2c986ae97487\",\"displayName\":\"CUSTOM CRUISER PARTS\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[]},{\"masterId\":\"c17ce805-4a7c-dc11-a551-0019b9b35da2\",\"displayName\":\"FACTORY - CRUISER - CRATED\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"be7ce805-4a7c-dc11-a551-0019b9b35da2\"]},{\"masterId\":\"d4a5bfbd-e025-40ce-827c-9ea424888dd2\",\"displayName\":\"SALVAGE CRUISER\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"9c4aa8aa-75c2-4fab-9448-d48bac25f7d1\",\"714b3043-4565-4da8-8d97-159bac97e5f6\"]},{\"masterId\":\"b8c5ff78-0053-413f-b586-95af41d9649a\",\"displayName\":\"AVP CRUISER DOMESTIC/METRIC\",\"maxInspectionScore\":100,\"requiredCategoryIds\":[\"be9c7ebb-a39e-4d34-aecf-4e314da51b42\",\"7ed299e9-6013-4357-bd79-192df8306ff3\",\"82259bf3-04c1-4f80-94bd-87f1e1f9ee90\"]},{\"masterId\":\"79e1ec5e-c14f-4daa-b7e9-8d6f801739f1\",\"displayName\":\"TEST ONLY\",\"maxInspectionScore\":100,\"hidden\":true,\"requiredCategoryIds\":[\"8ec9c377-af33-4324-ab8e-50b52657e341\",\"40ff63a3-89d1-4f88-998c-9bb53f95252d\"]},{\"masterId\":\"483450cd-033a-40d5-9d07-b264d7f58004\",\"displayName\":\"LONG CRUISER\",\"maxInspectionScore\":100,\"hidden\":true,\"requiredCategoryIds\":[\"4cf64331-13fa-4c35-abe6-e56447930a2d\",\"1bf34c01-e516-4962-8554-cf69fcc03fea\"]},{\"masterId\":\"357b34e6-d87f-487c-9b15-29dc4f33dd72\",\"displayName\":\"CQC CRUISER\",\"maxInspectionScore\":100,\"hidden\":true,\"requiredCategoryIds\":[\"cc303251-fc71-410e-85d0-fd94ca263596\",\"2cebc59c-2a8d-48b7-a309-3059a27cbcc6\",\"ee9bb860-cbdf-4871-83ec-233cfd81da28\"]},{\"masterId\":\"fb40caa5-a227-4dab-8eea-21241a650168\",\"displayName\":\"FACTORY - CRUISER - UNCRATED\",\"maxInspectionScore\":100,\"hidden\":true,\"requiredCategoryIds\":[\"c5f0e457-191f-4e19-b453-4253efc7bc76\",\"ff400175-edb5-47fb-9d75-96a1c4366392\"]},{\"masterId\":\"206d8cf7-c0d6-4a56-8a44-749ef7840395\",\"displayName\":\"H-D WEB M/C--CRUISER\",\"maxInspectionScore\":100,\"hidden\":true,\"requiredCategoryIds\":[\"90fd72e1-d444-4ce9-92ed-1f4f97214b8e\",\"1bf34c01-e516-4962-8554-cf69fcc03fea\"]}],\"types\":[{\"inspectionTypeId\":2,\"displayName\":\"Post-Inspection\"},{\"inspectionTypeId\":3,\"displayName\":\"Redemption\"},{\"inspectionTypeId\":4,\"displayName\":\"Transfer Inspection\"}]}";
            return JsonConvert.DeserializeObject<InspectionTypesAndMasters>( response );
        }

        public async Task<IEnumerable<string>> GetVehicleAlertsAsync( Vehicle vehicle )
        {
            string response = "[\"This vehicle has the 'No Battery' set\"]";
            return JsonConvert.DeserializeObject<IEnumerable<string>>( response );
        }

        public async Task<IEnumerable<string>> GetVinAlertsAsync( Vehicle vehicle )
        {
            string response = "[\"The Year/Make/Model seem inconsistent with your VIN code.\"]";
            return JsonConvert.DeserializeObject<IEnumerable<string>>( response );
        }

        public async Task<Vehicle> SearchAsync( VehicleSearchRequest searchRequest )
        {
            string response = UseVehicleWithoutInspections ?
                "[{\"vehicleId\":\"79d86e32-38f0-4cf8-9fb5-3c01fe799d7f\",\"vin\":\"JYARJ16E69A016140\",\"stockNumber\":\"265-10037672\",\"year\":2009,\"brand\":\"YAMAHA\",\"color\":\"BLUE\",\"model\":\"YZF-R6 TEAM YAMAHA\",\"vehicleModelId\":\"80a17ab2-b036-de11-bc96-0019b9b35da2\",\"vehicleCategoryId\":\"eaf0e6b3-ccae-4bbf-ac8a-d46f4df4321f\",\"milesHours\":\"5441\",\"noBattery\":true,\"pictureId\":\"aaa99a43-d2b5-e311-93fd-ac162d7cbbd1\",\"vehicleComments\":\"**DECLINED BATTERY (NPA - 3/25/14)**\r\n\r\n5440 ACTUAL\r\nTEMECULA MOTORSPORTS, INC\r\n26860 JEFFERSON AVE.\r\n***DONT HOLD TITLES- MAIL OUT WHEN PAID**\r\nMURRIETA, CA 92562\r\n858 442 5125 JERRY GILDING\r\n\r\n\",\"publicAuctionNotes\":\"\",\"auctioneerNotes\":\"RSV: 5500.00 CLOSE\r\n        TEMECULA MOTORSPORTS, INC\r\n\",\"vehicleState\":\"Complete\",\"seller\":\"TEMECULA MOTORSPORTS, INC\",\"salesRep\":\"Colton Clifford\",\"salesRepEmail\":\"coclifford@npauctions.com\",\"locationId\":\"c4b1d148-57e2-4fd2-9dae-0121d40854a0\"}]" :
                "[{\"vehicleId\":\"25df1290-7a5e-4bb4-8361-a3d2cf79eb64\",\"vin\":\"1HD1GZM3XDC320048\",\"stockNumber\":\"282-10064209\",\"year\":2013,\"brand\":\"HARLEY-DAVIDSON\",\"color\":\"BLK\",\"model\":\"FLD-103 DYNA SWITCHBACK\",\"vehicleModelId\":\"f034570f-501b-426d-8470-e60bb26b6773\",\"vehicleCategoryId\":\"da744e98-392a-4ef9-aedc-3e1e231ceeaf\",\"milesHours\":\"11632\",\"noBattery\":true,\"pictureId\":\"d0c726a0-0b64-4617-b89d-52ed5fade66e\",\"vehicleComments\":\"**DECLINED BATTERY (NPA - 5/2/16)**\r\n\r\n11631 ACTUAL\r\nDRC LEASING INC - ARIZONA\r\n6463 INDEPENDENCE AVENUE\r\nWOODLAND HILLS, CA 91367\r\n480-620-0209 TIM WALGREN\r\n\r\n\",\"publicAuctionNotes\":\"\",\"auctioneerNotes\":\"RSV: 10400.00 FIRM\r\n        DRC LEASING INC - ARIZONA\r\n\",\"vehicleState\":\"AuctionReady\",\"seller\":\"DRC LEASING INC - ARIZONA\",\"salesRep\":\"Colton Clifford\",\"salesRepEmail\":\"coclifford@npauctions.com\",\"locationId\":\"c4b1d148-57e2-4fd2-9dae-0121d40854a0\",\"allowNewInspections\":true}]";

            return JsonConvert.DeserializeObject<IEnumerable<Vehicle>>( response ).FirstOrDefault();
        }

        Task<List<Vehicle>> IVehicleService.SearchAsync(VehicleSearchRequest searchRequest)
        {
            throw new NotImplementedException();
        }

        Task<IDictionary<string, IEnumerable<Inspection>>> IVehicleService.GetInspectionsAsync(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        Task<string> IVehicleService.GetPrimaryPictureAsync(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }
    }
}
#endif
