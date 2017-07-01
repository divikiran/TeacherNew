using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NPAInspectionWriter.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPAInspectionWriter.iOS.Extensions;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.Extensions;
using NPAInspectionWriter.Helpers;
using SQLite.Net;
using Xamarin.Forms;
using static System.Net.WebUtility;

namespace NPAInspectionWriter.Services
{
#pragma warning disable 1998
    public class VehicleService : IVehicleService
    {
        const string ControllerRoot = "vehicles";
        static int sqlLock = 0;

        InspectionWriterClient client { get; }
        AppRepository db { get; }

        public VehicleService(InspectionWriterClient inspectionWriterClient, AppRepository appRepository)
        {
            client = inspectionWriterClient;
            db = appRepository;
        }

        // Maps to "{API BASE}/vehicles"
        public async Task<List<Vehicle>> SearchAsync(VehicleSearchRequest searchRequest)
        {
            try
            {
                //var vehicleId = await db.GetCurrentSettingAsync( Constants.CurrentVehicleKey );
                //if( string.IsNullOrWhiteSpace( vehicleId ) ) throw new Exception( "Vehicle not found." );

                var vehicles = await client.GetAsync<IEnumerable<Vehicle>>(ControllerRoot, failedResponse: new List<Vehicle>(), requestObject: searchRequest);
                // remove completed items. need to move this into the webapi
                vehicles = vehicles.Where(v => !v.VehicleState.Contains("Complete")).ToList();

                var currentVehicle = vehicles.FirstOrDefault();
                if (currentVehicle == null)
                {
                    switch (client.LastResponse.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            throw new VehicleNotFoundException();
                        case HttpStatusCode.OK:
                            throw new ObjectNotFoundException("Vehicle not found");
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedAccessException();
                        default:
                            var lastCode = client.LastResponse.StatusCode;
                            throw new WebException($"{(int)lastCode} - {lastCode.ToString()}");
                    }
                }

                // Store the vehicle in the Local Vehicles Table
                //await db.InsertOrReplaceAsync( (VehicleRecord)currentVehicle );

                // Store the vehicle id in the current settings
                await db.AddCurrentObjectAsync(currentVehicle);

                return vehicles.ToList(); // currentVehicle;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw e;
            }
        }

        public async Task<IEnumerable<InspectionMaster>> GetMastersForVehicleAsync(Vehicle vehicle) =>
            await client.GetAsync<IEnumerable<InspectionMaster>>($"{ControllerRoot}/{vehicle.Id}/masters", failedResponse: new List<InspectionMaster>());

        public async Task<InspectionTypesAndMasters> GetAvailableInspectionTypesAndMastersAsync(Vehicle vehicle)
        {
            var response = new InspectionTypesAndMasters();
            try
            {
                response = await client.GetAsync($"{ControllerRoot}/{vehicle.Id}/typesAndMasters", failedResponse: response);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            //logger.Log( await client?.LastResponse?.Content?.ReadAsStringAsync() );
            return response;
        }

        public async Task<IDictionary<string, IEnumerable<Inspection>>> GetInspectionsAsync(Vehicle vehicle)
        {
            var response = await client.GetAsync($"{ControllerRoot}/{vehicle.Id}/inspections?userName={UrlEncode(Settings.Current.UserName)}");
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Unable to retrieve Vehicle Inspections. {response.StatusCode}");
                return new Dictionary<string, IEnumerable<Inspection>>();
            }

            //var id = vehicle.StockNumber.Split( '-' )[ 1 ] ?? vehicle.StockNumber;
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IDictionary<string, IEnumerable<Inspection>>>(json);

            //var dict = new Dictionary<string,IEnumerable<Inspection>>();
            //try
            //{
            //    dict = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<Inspection>>>( json );
            //}
            //catch( Exception e )
            //{
            //    logger.Log( $"Unable to deserialize Inspections: {e}", Category.Warn );
            //    var jObject = JObject.Parse( json );
            //    foreach( var pair in jObject )
            //    {
            //        var list = new List<Inspection>();
            //        foreach( JToken inspection in pair.Value )
            //        {
            //            var newInspection = new Inspection()
            //            {
            //                // HACK: This should simply deserialize.
            //                InspectionId = new Guid( inspection[ "inspectionId" ].ToString() ),
            //                LocationId = new Guid( inspection[ "locationId" ].ToString() ),
            //                MasterId = new Guid( inspection[ "masterId" ].ToString() ),
            //                MasterDisplayName = inspection[ "master" ].ToString(),
            //                InspectionType = inspection[ "inspectionType" ].ToString(),
            //                InspectionTypeId = int.Parse( inspection[ "inspectionTypeId" ].ToString() ),
            //                InspectionUser = inspection[ "inspectionUser" ].ToString(),
            //                Score = int.Parse( inspection[ "score" ].ToString() ),
            //                InspectionDate = DateTime.Parse( inspection[ "inspectionDate" ].ToString() ),
            //                InspectionMilesHours = inspection[ "inspectionMilesHours" ].ToString(),
            //                InspectionColor = inspection[ "inspectionColor" ].ToString()
            //            };

            //            list.Add( newInspection );

            //            if( !await db.ExistsAsync<InspectionMasterRecord>( x => x.MasterId == newInspection.MasterId ) )
            //            {
            //                var master = await client.GetAsync<InspectionMaster>( $"inspections/masters/{newInspection.MasterId}" );

            //            }
            //        }

            //        dict.Add( pair.Key, list );
            //    }
            //}
            //return dict;
            //return await client.GetAsync<IDictionary<string, IEnumerable<Inspection>>>( $"vehicles/{vehicle.Id}/inspections?userName={UrlEncode( Settings.Current.UserName )}", failedResponse: new Dictionary<string, IEnumerable<Inspection>>() );
        }

        public async Task<Guid> GetPrimaryPictureIdAsync(Vehicle vehicle) =>
            await client.GetResultAsync<Guid>("result", $"{ControllerRoot}/{vehicle.Id}/primaryPictureId");

        public async Task<string> GetPrimaryPictureAsync(Vehicle vehicle)
        {
            try
            {
                Debug.WriteLine("Looking up Vehicle Image from database");
                string result = await db.GetCurrentSettingAsync(Constants.CurrentVehicleImageKey);
                Debug.WriteLine("Retrieved Vehicle Image from database");

                // HACK: OnNavigated{From/To} not being hit on either the VehicleDetailPage or VehicleSearchPage
                //if( string.IsNullOrWhiteSpace( result ) )
                {
                    result = await client.GetResultAsync("ret", $"{ControllerRoot}/getPrimaryPictureUrl/{vehicle.Id}");
                    await db.AddCurrentObjectAsync(Constants.CurrentVehicleImageKey, result);
                }

                Uri imageUri;
                if (Uri.TryCreate(result, UriKind.Absolute, out imageUri))
                {
                    return result;
                }
            }
            catch (SQLiteException sqlException)
            {
                if (sqlException.Message.Contains("database is locked"))
                {
                    sqlLock++;
                    if (sqlLock > 3)
                    {
                        throw new Exception("SQLite database lock retry unresolved after 3 retries.", sqlException);
                    }

                    return await TaskUtils.WaitAndRun(750, () => GetPrimaryPictureAsync(vehicle));
                }
                else
                {
                    Debug.WriteLine(sqlException);
                }
            }
            catch (ArgumentNullException ane)
            {
                Debug.WriteLine(ane);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return string.Empty;
        }

        public async Task<IEnumerable<string>> GetVehicleAlertsAsync(Vehicle vehicle) =>
            await client.GetAsync<IEnumerable<string>>($"vehicles/{vehicle.Id}/vehicleAlerts", failedResponse: new List<string>());

        public async Task<IEnumerable<string>> GetVinAlertsAsync(Vehicle vehicle) =>
            await client.GetAsync<IEnumerable<string>>($"vehicles/vinAlerts/{vehicle.Vin}/{vehicle.VehicleModelId}", failedResponse: new List<string>());

        public async Task<IEnumerable<string>> GetIsVinValid(string vin) =>
            await client.GetAsync<IEnumerable<string>>($"vehicles/isValidVin/{vin}", failedResponse: new List<string>());

        //public async Task<object> GetVehicleImageAsync()
        //{
        //   // InspectionsService inspectionsService = new InspectionsService();
        //}
    }
}
