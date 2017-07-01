using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using InspectionWriterWebApi.Models;
using InspectionWriterWebApi.Utilities;
using NPA.Common;

namespace InspectionWriterWebApi.Controllers
{
    [Authorize]
    [RoutePrefix("vehicles")]
    public class VehiclesController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetVehicleSummaries([FromUri] VehicleSearchRequest request)
        {
            // Get requested vehicles.
            // Notes:
            //   Exclude Remote location vehicles from NotAtLocation check, allowing them for CRs.
            try
            {
                var sql =
                "SELECT v.VehicleID, v.VIN, ISNULL(dbo.fn_GetSellerCode(v.VehicleID) + '-', '') + sn.StockNumber AS StockNumber, v.MilesHours, " +
                "v.Color, vb.VehicleBrand, vm.VehicleModel, vm.VehicleModelID, v.Year, COALESCE(v.VehicleCategoryID,vm.DefaultVehicleCategoryID) AS VehicleCategoryID, " +
                "dbo.fn_GetPrimaryPictureID(v.VehicleID) AS DefaultPictureID, v.LocationID, v.VehicleState, " +
                "ISNULL(CAST(v.noBattery AS bit),0) AS NoBattery, CASE WHEN r.RepairID IS NOT NULL THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS HasOpenRepair, " +
                "r.Comments AS RepairComments, COALESCE(d.Dealer, l2.Lender) AS Seller, v.Size, v.Comments as VehicleComments, " +
                "v.AuctionNotes as PublicAuctionNotes, dbo.fn_BuildAuctioneerNotes(v.VehicleID) AS AuctioneerNotes, " +
                "v.HasExtraParts, v.ExtraPartsList, " +
                "CASE WHEN EXISTS (SELECT * FROM Sale WITH (NOLOCK) WHERE VehicleID = v.VehicleID AND ISNULL(Redemption, 0) = 0) THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS IsSold, " +
                "CASE WHEN t.ReceivingDate IS NULL THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS NotReceived, " +
                "CASE WHEN v.LocationID <> @ServerLocationID AND v.LocationID <> '923D12B2-F63A-4313-B9B6-D52D9C5F565C' THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS NotAtLocation, " +
                "src.Contact AS SalesRep, src.Email AS SalesRepEmail, " +
                "ISNULL(CAST((SELECT Redemption FROM Sale WITH (NOLOCK) WHERE VehicleID = v.VehicleID) AS bit), 0) AS IsRedemption " +
                "FROM Vehicle v WITH (NOLOCK) " +
                "INNER JOIN Account a WITH (NOLOCK) ON a.VehicleID = v.VehicleID " +
                "INNER JOIN StockNumber sn WITH (NOLOCK) ON sn.StockNumberID = v.StockNumberID " +
                "INNER JOIN Lender l1 WITH (NOLOCK) ON l1.LenderID = a.LenderID " +
                "LEFT OUTER JOIN Repair r WITH (NOLOCK) ON r.VehicleID = v.VehicleID " +
                "LEFT OUTER JOIN VehicleModel vm WITH (NOLOCK) ON v.VehicleModelID = vm.VehicleModelID " +
                "LEFT OUTER JOIN VehicleBrand vb WITH (NOLOCK) ON vb.VehicleBrandID = vm.VehicleBrandID " +
                "LEFT OUTER JOIN Transit t WITH (NOLOCK) ON t.VehicleID = v.VehicleID AND ISNULL(t.TransitTypeID, 0) = 0 " +
                "LEFT OUTER JOIN Dealer d WITH (NOLOCK) ON d.DealerID = a.SellerLinkID AND a.SellerLinkTypeID = 2 " +
                "LEFT OUTER JOIN Lender l2 WITH (NOLOCK) ON l2.LenderID = a.SellerLinkID AND a.SellerLinkTypeID = 1 " +
                "LEFT OUTER JOIN UserAccount sru WITH (NOLOCK) ON sru.UserAccountID = COALESCE(d.SalesRepID, l2.SalesRepID) " +
                "LEFT OUTER JOIN Contact src WITH (NOLOCK) ON src.ContactID = sru.ContactID " +
                "WHERE 1 = 1 ";

                sql += "AND (dbo.fn_IsSold(v.VehicleID, getdate()) = 0 OR dbo.fn_IsRedeemed(v.VehicleID) = 1 ) ";
                sql += "AND dbo.fn_IsCanceled(v.VehicleID) = 0 ";

                var ret = new Collection<Vehicle>();

                var @params = new Collection<SqlParameter>();

                if (request.VehicleId.HasValue)
                {
                    sql += "AND v.VehicleID = @VehicleID ";
                    @params.Add(new SqlParameter("@VehicleID", request.VehicleId.Value));
                }

                if (!string.IsNullOrWhiteSpace(request.Vin))
                {
                    sql += "AND v.VIN LIKE '%' + @VIN ";
                    @params.Add(new SqlParameter("@VIN", request.Vin));
                }

                if (!string.IsNullOrWhiteSpace(request.StockNumber))
                {
                    sql += "AND sn.StockNumber LIKE '%' + @StockNumber + '%' ";
                    @params.Add(new SqlParameter("@StockNumber", request.StockNumber));
                }

                if (request.LocationId.HasValue)
                {
                    sql += "AND v.LocationID = @LocationID ";
                    @params.Add(new SqlParameter("@LocationID", request.LocationId.Value));
                }

                if (request.BrandId.HasValue)
                {
                    sql += "AND vb.VehicleBrandID = @VehicleBrandID ";
                    @params.Add(new SqlParameter("@VehicleBrandID", request.BrandId.Value));
                }

                if (request.FromYear.HasValue)
                {
                    sql += "AND v.Year >= @FromYear ";
                    @params.Add(new SqlParameter("@FromYear", request.FromYear.Value));
                }

                if (request.ToYear.HasValue)
                {
                    sql += "AND v.Year <= @ToYear ";
                    @params.Add(new SqlParameter("@ToYear", request.ToYear.Value));
                }

                if (!string.IsNullOrWhiteSpace(request.Model))
                {
                    sql += "AND vm.VehicleModel LIKE @Model + '%' ";
                    @params.Add(new SqlParameter("@Model", request.Model));
                }

                if (request.VehicleIds != null && request.VehicleIds.Any())
                {
                    sql += "AND v.VehicleID IN (SELECT Item FROM dbo.fn_List2Table(',', @VehicleIDs)) ";
                    @params.Add(new SqlParameter("@VehicleIDs", string.Join(",", request.VehicleIds)));
                }

                if (!@params.Any())
                {
                    return BadRequest("Must Provide at Least One Criteria");
                }

                // the ServerLocationID is San Diego and is the wrong place for anyone other than san diego. besides, this is already 
                // being taken care of by the location test in the CR Writer.
                @params.Add(new SqlParameter("@ServerLocationID", WebConfigurationManager.AppSettings["ServerLocationId"].ParseTo<Guid>()));

                //LogUtil.WriteLogEntry("InspectionWriterWebApi", "sql: " + sql, System.Diagnostics.EventLogEntryType.Information);

                using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddRange(@params.ToArray());

                        await cn.OpenAsync();

                        try
                        {

                            //set VinCheckExclude
                            sql = "SELECT Value FROM Setting WITH (NOLOCK) WHERE Setting = 'VinCheckCategoryExcludeList'";

                            string[] sCategoryExcludeList = NPA.Core.SqlHelper.ExecuteScalar(ApiSettings.ConnectionString, CommandType.Text, sql).TryToString().Split(',');

                            using (var dr = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult))
                            {
                                while (await dr.ReadAsync())
                                {
                                    bool vinCheckExclude = false;
                                    if (dr["VehicleCategoryID"] == null || sCategoryExcludeList.Contains(dr["VehicleCategoryID"].TryToString().ToUpper()))
                                    {
                                        vinCheckExclude = true;
                                    }
                                    //TODO: cannot get AsyncHelpers.RunSync to work. dont have time to fix right now
                                    string primaryPicureUrl = GetPrimaryPictureFullURLSync((Guid)dr["VehicleID"]);

                                    ret.Add(new Vehicle
                                    {
                                        Id = (Guid)dr["VehicleID"],
                                        Vin = dr["VIN"].ToString(),
                                        StockNumber = dr["StockNumber"].ToString(),
                                        Brand = dr["VehicleBrand"].ToString(),
                                        Color = dr["Color"].ToString(),
                                        Model = dr["VehicleModel"].ToString(),
                                        VehicleModelId = dr["VehicleModelID"].ToString(),
                                        VehicleCategoryId = dr["VehicleCategoryID"].ToString(),
                                        MilesHours = dr["MilesHours"].ToString(),
                                        Year = dr["Year"] as int?,
                                        NoBattery = (bool)dr["NoBattery"],
                                        PictureId = dr["DefaultPictureID"] as Guid?,
                                        PrimaryPictureUrl = primaryPicureUrl,
                                        VehicleState = dr["VehicleState"].ToString(),
                                        Seller = dr["Seller"].ToString().ToNullIfWhiteSpace(),
                                        SalesRep = dr["SalesRep"].ToString().ToNullIfWhiteSpace(),
                                        SalesRepEmail = dr["SalesRepEmail"].ToString().ToNullIfWhiteSpace(),
                                        VehicleComments = dr["VehicleComments"].ToString(),
                                        PublicAuctionNotes = dr["PublicAuctionNotes"].ToString(),
                                        AuctioneerNotes = dr["AuctioneerNotes"].ToString(),
                                        LocationId = dr["LocationID"] as Guid?,
                                        RepairExists = (bool)dr["HasOpenRepair"],
                                        RepairComments = dr["RepairComments"].ToString().ToNullIfWhiteSpace(),
                                        Size = dr["Size"].ToString().ToNullIfWhiteSpace(),
                                        VinCheckExclude = vinCheckExclude,
                                        AllowNewInspections = (((bool)dr["IsRedemption"] || !(bool)dr["IsSold"]) && !(bool)dr["NotReceived"] && !(bool)dr["NotAtLocation"]),
                                        HasExtraParts = TypeUtil.GetSafeBoolean(dr["HasExtraParts"]),
                                        ExtraPartsList = dr["ExtraPartsList"].TryToString()
                                    });
                                }
                            }


                        }
                        catch (SqlException ex)
                        {
                            if (ex.Number == -2)
                            {
                                IwLogUtil.SendExceptionNotification(ex);
                                return
                                    InternalServerError(
                                        new Exception(
                                            "The request timed out, most likely as a result of using too few characters in the search criteria."));
                            }
                        }
                        catch (Exception ex2)
                        {
                            IwLogUtil.SendExceptionNotification(ex2);
                            return InternalServerError(ex2);
                        }
                    }
                }

                if (ret.Count < 1)
                {
                    return NotFound();
                }

                return Ok(ret);
            }
            catch (Exception exOuter)
            {
                IwLogUtil.SendExceptionNotification(exOuter);
                return InternalServerError(exOuter);
            }
            
        }
        
        [HttpGet]
        [Route("{vehicleId}/masters")]
        public async Task<IHttpActionResult> MastersForVehicle(Guid vehicleId)
        {
            var ret = await InspectionsController.GetMasters(null, vehicleId);

            return Ok(ret);
        }

        [NonAction]
        private async Task<string> GetVinFromStockNumber(string stockNumber)
        {
            var sql = "SELECT VIN FROM Vehicle v WITH (NOLOCK) " +
                      "JOIN StockNumber sn WITH (NOLOCK) ON v.StockNumberID = sn.StockNumberID " +
                      "WHERE sn.StockNumber = @StockNumber";

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add(new SqlParameter("@StockNumber", stockNumber));

                    await cn.OpenAsync();

                    var result = await cmd.ExecuteScalarAsync() as string;

                    return result;
                }
            }
        }

        [HttpGet]
        [Route("{vehicleId}/typesAndMasters")]
        public async Task<IHttpActionResult> GetAvailableInspectionTypesAndMasters(Guid vehicleId)
        {
            var ret = new Dictionary<string, object>();

            var masters = await InspectionsController.GetMasters(null, vehicleId);

            var types = await InspectionsController.AvailableInspectionTypes(vehicleId, null);

            ret.Add("masters", masters);
            ret.Add("types", types);

            return Ok(ret);
        }

        
        [HttpGet]
        [Route("{vehicleId}/inspections")]
        public async Task<IHttpActionResult> GetInspectionsForVehicle(Guid vehicleId, string username)
        {
            var ret = await InspectionsController.GetInspectionsForVehicle(vehicleId, username);

            if (ret.Count < 1)
            {
                return NotFound();
            }

            return Ok(ret);
        }
        
        [HttpGet]
        [Route("{vehicleId}/primaryPictureId")]
        public async Task<IHttpActionResult> GetPrimaryPictureURL(Guid vehicleId)
        {
            var sql = "SELECT dbo.fn_GetPrimaryPictureID(@VehicleID)";

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@VehicleID", vehicleId);

                    await cn.OpenAsync();

                    var result = await cmd.ExecuteScalarAsync();

                    return Ok(new {result});
                }
            }
        }

        [HttpGet]
        [Route("getPrimaryPictureUrl/{vehicleId}")]
        public async Task<IHttpActionResult> GetPrimaryPictureFullURL(Guid vehicleId)
        {
            var sql = @"SELECT LocationID, dbo.fn_GetPrimaryPictureID(@VehicleID) PictureID 
                        FROM PictureLocation WHERE PictureID = dbo.fn_GetPrimaryPictureID(@VehicleID)";
            
            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                await cn.OpenAsync();

                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@VehicleID", vehicleId);

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        string ret = string.Empty;

                        while (await dr.ReadAsync())
                        {
                            string picId = dr["PictureID"].TryToString();
                            string locId = dr["LocationID"].TryToString();

                            ret = string.Format(GetLocationImageBaseURL(locId), picId) + "&Width=320&Height=240";
                        }

                        return Ok(new { ret });
                    }
                }
            }

        }

        public string GetLocationImageBaseURL(string locationId)
        {
            if (locationId.IsEmpty()) { return null; }

            string amsBaseUrl = "";
            string imageBaseUrl = "/library/Image.aspx?PictureID={0}";

            string sql = "SELECT LocalURL FROM Location WITH (NOLOCK) WHERE LocationID = @LocationID";

            amsBaseUrl = NPA.Core.SqlHelper.ExecuteScalar(ApiSettings.ConnectionString, CommandType.Text, sql, new[] { new SqlParameter("@LocationID", locationId) }).TryToString();

            return amsBaseUrl + imageBaseUrl;
        }

        public string GetPrimaryPictureFullURLSync(Guid vehicleId)
        {
            var sql = @"SELECT LocationID, dbo.fn_GetPrimaryPictureID(@VehicleID) PictureID 
                        FROM PictureLocation WHERE PictureID = dbo.fn_GetPrimaryPictureID(@VehicleID)";

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                cn.Open();

                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@VehicleID", vehicleId);

                    using (var dr = cmd.ExecuteReader())
                    {
                        string ret = string.Empty;

                        while (dr.Read())
                        {
                            string picId = dr["PictureID"].TryToString();
                            string locId = dr["LocationID"].TryToString();
                            ret = string.Format(GetLocationImageBaseURL(locId), picId) + "&Width=320&Height=240";
                        }

                        return ret;
                    }
                }
            }

        }

        [HttpGet]
        [Route("{vehicleId}/vehicleAlerts")]
        public IHttpActionResult GetVehicleAlerts(Guid vehicleId)
        {
            //string[] result = new string[] { };
            //return Ok(result);

            string requestParams = string.Empty;
            // *** http://10.3.0.27:8095/Vehicle.asmx/VehicleAlerts ***
            string uri = WebConfigurationManager.AppSettings["VehicleServicesPath"].TryToString() + "VehicleAlerts"; 
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                wc.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                requestParams = "{'vehicleId':'" + vehicleId + "'}";
                var json = wc.UploadString(new Uri(uri), requestParams);
                dynamic val = System.Web.Helpers.Json.Decode(json);
                return Ok(val.d);
            }
            catch (Exception ex)
            {
                IwLogUtil.SendExceptionNotification(ex, "URI:" + uri + " params:" + requestParams);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("vinAlerts/{vin}/{vehicleModelId}")]
        public IHttpActionResult GetVINAlerts(string vin, string vehicleModelId)
        {
            //// for testing only
            //string[] result = new string[] { "vin:" + vin, "vehicleModelId:" + vehicleModelId };
            //return Ok(result);
            try
            {
                string uri = WebConfigurationManager.AppSettings["VehicleServicesPath"].TryToString() + "VinAlerts";
                System.Net.WebClient wc = new System.Net.WebClient();
                wc.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                string requestParams = "{'vin':'" + vin + "', 'vehicleModelId':'" + vehicleModelId + "'}";
                var json = wc.UploadString(new Uri(uri), requestParams);
                dynamic val = System.Web.Helpers.Json.Decode(json);
                return Ok(val.d);
            }
            catch (Exception ex)
            {
                IwLogUtil.SendExceptionNotification(ex);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("isValidVin/{vin}")]
        public IHttpActionResult GetIsValidVin(string vin)
        {
            try
            {
                //string uri = WebConfigurationManager.AppSettings["VehicleServicesPath"].TryToString() + "IsValidVin";
                //System.Net.WebClient wc = new System.Net.WebClient();
                //wc.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                //string requestParams = "{'vin':'" + vin + "'}";
                //var json = wc.UploadString(new Uri(uri), requestParams);
                //dynamic val = System.Web.Helpers.Json.Decode(json);
                //return Ok(val.d);

                var sql = string.Format("SELECT dbo.fn_IsValidVIN('{0}') ", vin);
                bool validVin = TypeUtil.GetSafeBoolean(NPA.Core.SqlHelper.ExecuteScalar(ApiSettings.ConnectionString, CommandType.Text, sql));
                return Ok(validVin);
            }
            catch (Exception ex)
            {
                IwLogUtil.SendExceptionNotification(ex);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("amsReceivingPage/{vin}/{stocknumber}")]
        public IHttpActionResult GetAmsPdaReceivingPage(string vin, string stocknumber)
        {
            string receive = "pda-ReceiveVehicle.aspx?vehicleid={0}";
            string unknown = "pda-ReceiveUnknown.aspx?VIN={0}";
            string crossDock = "pda-ReceiveCrossDock.aspx?VIN={0}";
            string transfer = "pda-ReceiveTransfer.aspx?transitid={0}";
            string returnVal = "";

            var sql = string.Format("SELECT dbo.fn_IsValidVIN('{0}') AS IsValidVIN", vin);
            bool validVin = TypeUtil.GetSafeBoolean( NPA.Core.SqlHelper.ExecuteScalar(ApiSettings.ConnectionString, CommandType.Text, sql) );



            return Ok(returnVal);
        }

        private int GetTransitType(string vin)
        {
            string sSQL = "SELECT VehicleID, TransitID, ReceivingDate, ReceivingUser, VIN, Year, VehicleBrand, VehicleModel, IsNull(TransitTypeID,0) as TransitTypeID "
                    + "FROM TransitQuickBrowseView WITH (NOLOCK)  "
                    + "WHERE (1 = 1) "
                    + "AND IsNull(TransitTypeID,0) = 0 "
                    + "AND VehicleState NOT LIKE '%Complete%' "
                    + "AND SaleDate IS NULL "
                    + "AND CancelDate IS NULL "
                    + "AND TransitCancelDate IS NULL "
                    + "AND VIN LIKE '%" + vin + "' "
                    + "AND DestinationLinkTypeID = '40' "
                    + "AND DestinationLinkID='" + WebConfigurationManager.AppSettings["ServerLocationId"].TryToString() + "' "
                    + "AND (SELECT COUNT(*) FROM Transit T2 WITH (NOLOCK) WHERE T2.VehicleID = TransitQuickBrowseView.VehicleID) = 1 "
                    + "ORDER BY DateCreated DESC";


            return 0;
        }

        [HttpPost]
        [Route("updateExtraParts")]
        public async Task<IHttpActionResult> UpdateExtraParts(Models.ExtraParts parts)
        {
            try
            {
                var user = System.Web.HttpContext.Current.User;
                string username = user.Identity.Name;
                string previousVal = await GetExtraPartsList(parts.VehicleId);

                await AddExtraPartsActivity(parts.VehicleId, username, TypeUtil.GetSafeBoolean(parts.HasExtraParts), parts.ExtraPartsList, previousVal);

                string sql = "UPDATE Vehicle SET HasExtraParts=@HasExtraParts, ExtraPartsList=@ExtraPartsList WHERE VehicleID=@VehicleID";
                var @params = new Collection<SqlParameter>();
                @params.Add(new SqlParameter("@VehicleID", parts.VehicleId));
                @params.Add(new SqlParameter("@HasExtraParts",TypeUtil.GetSafeBoolean(parts.HasExtraParts) ? 1 : 0));
                @params.Add(new SqlParameter("@ExtraPartsList", parts.ExtraPartsList));

                using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddRange(@params.ToArray());

                        await cn.OpenAsync();
                        var result = await cmd.ExecuteNonQueryAsync();
                        return Ok(new { result });
                    }
                }
            }
            catch (Exception ex)
            {
                IwLogUtil.SendExceptionNotification(ex);
                return InternalServerError(ex);
            }
        }

        private static async Task<string> GetExtraPartsList(string vehicleId)
        {
            string sql = "SELECT ExtraPartsList FROM Vehicle WHERE VehicleID=@VehicleID";

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add(new SqlParameter("@VehicleID", vehicleId));
                    await cn.OpenAsync();
                    var val = await cmd.ExecuteScalarAsync();
                    return val.TryToString();
                }
            }
        }

        private static async Task AddExtraPartsActivity(string vehicleID, string username, bool hasExtraParts, string newVal, string previousVal)
        {
            try
            {
                //string formattedDateTime = DateTime.Now.ToString("MM/dd/yy H:mm:ss");
                string xtraParts = string.Empty;
                string partsList = string.Empty;
                if (!TypeUtil.GetSafeBoolean(hasExtraParts)) { xtraParts = "HasExtraParts = False. "; }
                if(previousVal != newVal) { string.Format("Extra Parts List changed from ({0}) to ({1})", previousVal, newVal); }
                string userId = DBHelpers.GetUserAccountID(username);

                string comments = string.Format("{0}{1}", xtraParts, partsList);

                var @params = new[]
                {
                    new SqlParameter("@Activity", "[UPDATED EXTRA PARTS (IPAD)]"),
                    new SqlParameter("@ActivityTypeID",
                        WebConfigurationManager.AppSettings["ExtraPartsActivityTypeId"].ParseTo<Guid>()),
                    new SqlParameter("@ActivityUserID", userId),
                    new SqlParameter("@ActivityDate", DateTime.Now),
                    new SqlParameter("@Comments", comments),
                    new SqlParameter("@LinkEntity", "Vehicle"),
                    new SqlParameter("@LinkEntityID", vehicleID),
                    new SqlParameter("@LinkTypeID",
                        WebConfigurationManager.AppSettings["VehicleLinkTypeId"].ParseTo<int>()),
                    new SqlParameter
                    {
                        ParameterName = "@Result",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Direction = ParameterDirection.Output
                    },
                };

                using (var txnScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                    {
                        using (var cmd = new SqlCommand("spAddOrUpdateActivity", cn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddRange(@params);

                            await cn.OpenAsync();
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }

                    txnScope.Complete();
                }
            }
            catch (Exception ex)
            {
                IwLogUtil.SendExceptionNotification(ex, "Failed during call to AddExtraPartsActivity");
                throw;
            }
        }
    }
}