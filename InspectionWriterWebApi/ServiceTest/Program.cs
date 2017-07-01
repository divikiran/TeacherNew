using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InspectionWriterWebApi.Utilities;

namespace ServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.testImageUtils();
            //p.testSaveNewInspection();
        }

        private void testImageUtils()
        {
            //var v = new InspectionWriterWebApi.Controllers.VehiclesController();
            //var tmp = AsyncHelpers.RunSync<System.Web.Http.IHttpActionResult>(() => v.GetVehicleSummaries(new InspectionWriterWebApi.Models.VehicleSearchRequest() { StockNumber= "10067010" }));

            //string source = @"C:\Users\wsteed\Pictures\Saved Pictures\I{638939FE-3951-4496-BEB4-FD5C7F91DB7C}_320_240.jpg";
            //string destination = @"C:\Users\wsteed\Pictures\Saved Pictures\processed_320_240.jpg";
            //ImageUtils.ShrinkAndSaveImage(source, destination, 320, 240);

            //var i = new InspectionWriterWebApi.Controllers.InspectionsController();
            //var tmp = i.GetInspectionImagesWithURL(new Guid("D86DB2A0-A0C3-4D96-974B-53DB51ABD13F"));
            //var tmp =  i.MasterAndTypeForInspection(1, new Guid("{357415cd-b96c-4075-a47d-1750e663e5e5}"));

            //var v = new InspectionWriterWebApi.Controllers.VehiclesController();
            //var tmp = v.GetPrimaryPictureFullURL(new Guid("82A62672-F813-4FFD-8754-5D551FED7633"));
            //var t = tmp;

            var v = new InspectionWriterWebApi.Controllers.VehiclesController();
            var tmp = AsyncHelpers.RunSync<System.Web.Http.IHttpActionResult>(() =>
                        v.GetInspectionsForVehicle (new Guid("82A62672-F813-4FFD-8754-5D551FED7633"), "testUser"));
            var t = tmp;

            //var v = new InspectionWriterWebApi.Controllers.VehiclesController();
            //var tmp = v.GetVINAlerts("1HD4CR2177K467187", "8C014347-4CBD-48B6-89CD-B71BAFBCD865");

            //var i = new InspectionWriterWebApi.Controllers.InspectionsController();
            //var tmp = i.GetMasterWithPrevious(new Guid("59E859AB-ECA0-43D8-81D2-2A32745765F5"), new Guid("7F8B0FA7-23AA-4BDF-A3CD-DBEA228D7E79"), 15);

        }

        private void testSaveNewInspection()
        {
            string json = "{\"contextAppVersion\":\"4.0.0A(Stage)\",\"contextiOSVersion\":\"10.1.1\",\"contextiPadModel\":\"iPad mini with Retina display(Wi-Fi)\",\"contextUsername\":\"wsteed\",\"contextLocation\":\"San Diego\",\"inspectionId\":\"4f2ab93b-ae02-495d-9bee-21178365c18f\",\"inspection\":\"00001\",\"vehicleId\":\"b00190d6-4e5f-42a2-a210-025dcc35d667\",\"locationId\":\"c4b1d148-57e2-4fd2-9dae-0121d40854a0\",\"masterId\":\"357415cd-b96c-4075-a47d-1750e663e5e5\",\"inspectionType\":\"Post-Inspection\",\"inspectionUser\":\"wsteed\",\"inspectionUserId\":\"009f6d94-14eb-463e-abd6-13058c731cca\",\"score\":84,\"comments\":\"\",\"inspectionDate\":\"2016-11-18T16: 27:49.4124Z\",\"inspectionItems\":[{\"categoryId\":\"8713ae8a-de8c-4ead-a5a4-3683c5990464\",\"optionId\":\"dfa1bbcd-b30e-e311-93f7-ac162d7cc3cb\",\"itemScore\":9},{\"categoryId\":\"0e96ed45-e72e-4d42-ab53-53d8e310bba3\",\"optionId\":\"b786fa6b-dc10-e311-93fb-ac162d7cbbd1\",\"comments\":\"TICKING NOISE\",\"itemScore\":9},{\"categoryId\":\"c89414b7-7991-4cb9-9551-728bb0a93be5\",\"optionId\":\"41b72659-f718-e511-9412-ac162d7cbbd1\",\"itemScore\":8},{\"categoryId\":\"52e1b177-690c-4023-88bc-acaa2a5d6ace\",\"optionId\":\"912b6a60-b40e-e311-93f7-ac162d7cc3cb\",\"itemScore\":9},{\"categoryId\":\"635b1808-2394-4332-a4f3-df80bdb74935\",\"optionId\":\"bb7564e5-b90e-e311-93f7-ac162d7cc3cb\",\"itemScore\":8},{\"categoryId\":\"e39224d3-5800-49e7-b3f6-4373a26800b4\",\"optionId\":\"6af7b906-be0e-e311-93f7-ac162d7cc3cb\",\"itemScore\":7},{\"categoryId\":\"dcfb1100-8f8e-4700-9b1c-81f28967c5dc\",\"optionId\":\"b357f3cb-e910-e311-93fb-ac162d7cbbd1\",\"itemScore\":7},{\"categoryId\":\"3a88ca6a-5a14-43ee-892b-daea911bff2a\",\"optionId\":\"f161503b-bd11-e311-93fb-ac162d7cbbd1\",\"itemScore\":8},{\"categoryId\":\"8ae9fdf2-436f-401e-b55b-3ebe983cdf4b\",\"optionId\":\"95f952f9-bc0e-e311-93f7-ac162d7cc3cb\",\"itemScore\":9},{\"categoryId\":\"c7c05dbc-8136-4fee-bf9e-b54ca4c4e24a\",\"optionId\":\"acf0be6b-bd0e-e311-93f7-ac162d7cc3cb\",\"itemScore\":9}],\"repairComments\":\"\",\"newColor\":\"BLK / BLUE\",\"newMileage\":\"15624\",\"newVIN\":\"1HD1CT3179K407508\",\"newVehicleModelId\":\"771246e2-cbdb-dd11-aca2-0019b9b35da2\",\"elapsedTime\":50,\"allowEditing\":true,\"inspectionMilesHours\":\"15624\",\"inspectionColor\":\"BLK / BLUE\"}";
            var insp = Newtonsoft.Json.JsonConvert.DeserializeObject<InspectionWriterWebApi.Models.Inspection>(json);
            var i = new InspectionWriterWebApi.Controllers.InspectionsController();
            var tmp = i.SaveNewInspection(insp);
        }
    }
}
