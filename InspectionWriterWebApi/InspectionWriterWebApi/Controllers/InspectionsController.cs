using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using InspectionWriterWebApi.Models;
using InspectionWriterWebApi.Utilities;
using NPA.Common;
using Location = NPA.CodeGen.Location;
using InspectionWriterWebApi.Configuration;

namespace InspectionWriterWebApi.Controllers
{

    [Authorize]
    [RoutePrefix("inspections")]
    public class InspectionsController : ApiController
    {
        [HttpGet]
        [Route("{inspectionId}/masters")]
        public async Task<IHttpActionResult> MastersForInspection(Guid inspectionId)
        {
            var ret = await GetMasters(inspectionId, null);

            return Ok(ret);
        }

        [HttpGet]
        [Route("{inspectionId}/typesAndMasters")]
        public async Task<IHttpActionResult> MastersAndTypesForInspection(Guid inspectionId)
        {
            var ret = new Dictionary<string, object>();

            var masters = await GetMasters(inspectionId, null);

            var types = await AvailableInspectionTypes(null, inspectionId); 

            ret.Add("masters", masters);
            ret.Add("types", types);

            return Ok(ret);
        }

        [HttpGet]
        [Route("{inspectionTypeId}/{masterId}/typeAndMaster")]
        public async Task<IHttpActionResult> MasterAndTypeForInspection(int inspectionTypeId, Guid masterId)
        {
            var ret = new Dictionary<string, object>();
            List<InspectionMaster> masters = new List<InspectionMaster>();
            List<InspectionType> types = new List<InspectionType>();

            try
            {
                masters.Add( await GetMasterById(masterId));

                types.Add(GetInspectionTypeById(inspectionTypeId));
                //LogUtil.WriteLogEntry("InspectionWriterWebApi", "Masters " + master.DisplayName + " types " + types.DisplayName, System.Diagnostics.EventLogEntryType.Information);
                ret.Add("masters", masters);
                ret.Add("types", types);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLogEntry("InspectionWriterWebApi", ex.TryToString(), System.Diagnostics.EventLogEntryType.Information);
            }

            return Ok(ret);
        }


        [HttpGet]
        [Route("masterWithPreviousVal/{vehicleId}/{masterId}/{numDays}")]
        public IHttpActionResult GetMasterWithPrevious(Guid vehicleId, Guid masterId, int numDays)
        {
            InspectionMaster master = ((System.Web.Http.Results.OkNegotiatedContentResult<InspectionMaster>)AsyncHelpers.RunSync<IHttpActionResult>(() => GetMaster(masterId))).Content;
            var inspections = ((Dictionary<string, IList<Inspection>>)AsyncHelpers.RunSync<IDictionary<string, IList<Inspection>>>(() => GetInspectionsForVehicle(vehicleId, "")));

            if (inspections != null && inspections.Count() > 0)
            {
                var inspection = inspections.First().Value.FirstOrDefault(i => ((Inspection)i).InspectionDate.Date > DateTime.Now.AddDays(-1 * numDays).Date);

                if (inspection != null)
                {
                    List<InspectionItem> lastItems = ((System.Web.Http.Results.OkNegotiatedContentResult<List<InspectionItem>>)AsyncHelpers
                        .RunSync<IHttpActionResult>(() => GetInspectionItems(inspection.InspectionId))).Content;

                    foreach (var c in master.Categories)
                    {
                        var lastVal = lastItems.Where(i => i.CategoryId == c.CategoryId && i.OptionId != null).FirstOrDefault();
                        if (lastVal != null)
                        {
                            InspectionOption opt = c.Options.Where(o => o.OptionId == (Guid)lastVal.OptionId).FirstOrDefault();
                            c.PreviousValue = new InspectionOption()
                            {
                                
                                OptionId = (Guid)lastVal.OptionId,
                                DisplayName = opt.DisplayName,
                                Value = opt.Value
                            };
                        }
                    }
                }
            }

            return Ok(master);
        }



        [HttpGet]
        [Route("inspectionWithSelectedVal/{inspectionId}")]
        public IHttpActionResult GetInspectionWithSelectedVal(Guid inspectionId)
        {
            InspectionMaster master = null;
            Inspection inspection = (Inspection)AsyncHelpers.RunSync<Inspection>(() => GetInspectionForVehicle(inspectionId));

            if (inspection != null)
            {
                Guid masterId = inspection.MasterId;
                master = ((System.Web.Http.Results.OkNegotiatedContentResult<InspectionMaster>)AsyncHelpers.RunSync<IHttpActionResult>(() => GetMaster(masterId))).Content;

                List<InspectionItem> lastItems = ((System.Web.Http.Results.OkNegotiatedContentResult<List<InspectionItem>>)AsyncHelpers
                    .RunSync<IHttpActionResult>(() => GetInspectionItems(inspection.InspectionId))).Content;

                foreach (var c in master.Categories)
                {
                    var val = lastItems.Where(i => i.CategoryId == c.CategoryId && i.OptionId != null).FirstOrDefault();
                    if (val != null)
                    {
                        c.CurrentValue = new InspectionOption()
                        {
                            OptionId = (Guid)val.OptionId,
                            Value = val.ItemScore,
                            DisplayName = (Guid)val.OptionId != null ? DBHelpers.OptionIdToDescription((Guid)val.OptionId) : string.Empty
                        };
                        c.Comments = val.ItemComments;
                    }
                }
            }

            return Ok(master);
        }


        [Route("masters/{masterId}")]
        public async Task<IHttpActionResult> GetMaster(Guid masterId)
        {
            var sql =
                "SELECT InspectionMasterID, InspectionMaster, CAST(CategoryDefault AS bit) AS DefaultForCategory, " +
                "CAST(MaxInspectionScore AS int) AS MaxInspectionScore " +
                "FROM InspectionMasterDetailView " +
                "WHERE InspectionMasterID = @MasterID";

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@MasterID", masterId);

                    await cn.OpenAsync();

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        if (!await dr.ReadAsync())
                            return NotFound();

                        var master = new InspectionMaster
                        {
                            MasterId = masterId,
                            DisplayName = dr["InspectionMaster"].ToString(),
                            DefaultForCategory = (bool)dr["DefaultForCategory"],
                            MaxInspectionScore = (int)dr["MaxInspectionScore"],
                            Categories = await CategoriesForMasterWithId(masterId),
                        };

                        return Ok(master);
                    }
                }
            }
        }

        public async Task<InspectionMaster> GetMasterById(Guid masterId)
        {
            var sql =
                "SELECT InspectionMasterID, InspectionMaster, CAST(CategoryDefault AS bit) AS DefaultForCategory, " +
                "CAST(MaxInspectionScore AS int) AS MaxInspectionScore " +
                "FROM InspectionMasterDetailView " +
                "WHERE InspectionMasterID = @MasterID";

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@MasterID", masterId);

                    await cn.OpenAsync();

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        if (!await dr.ReadAsync())
                            return null;

                        var master = new InspectionMaster
                        {
                            MasterId = masterId,
                            DisplayName = dr["InspectionMaster"].ToString(),
                            DefaultForCategory = (bool)dr["DefaultForCategory"],
                            MaxInspectionScore = (int)dr["MaxInspectionScore"],
                            Categories = await CategoriesForMasterWithId(masterId),
                        };

                        return master;
                    }
                }
            }
        }


        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> SaveNewInspection(Inspection inspection)
        {
            if (await InspectionExists(inspection.InspectionId))
            {
                return BadRequest("Inspection with ID " + inspection.InspectionId + " already exists.");
            }

            // Log user context for this new inspection.
            await Task.Run(() => IwLogUtil.LogContext(inspection.ContextAppVersion, inspection.ContextiOSVersion.TryToString(""), inspection.ContextiPadModel.TryToString(),
                inspection.ContextUsername, inspection.ContextLocation, inspection.InspectionId,
                37,  // Inspection
                new Guid("B5091F6C-FA34-4201-851A-F4F6A2CA76AE")  // CrWriterNewInspection
                ));

            var sql = 
                "INSERT INTO Inspection (" +
                "   InspectionID, DateCreated, DateModified, Inspection, InspectionDate, InspectionUserID, InspectionUser, " +
                "   Score, InspectionType, InspectionLocationID, Comments, VehicleID, InspectionMasterID" +
                ") VALUES (" +
                "   @InspectionID, GETDATE(), GETDATE(), '00001', @InspectionDate, @InspectionUserID, @InspectionUser, " +
                "   @Score, @InspectionTypeID, @InspectionLocationID, @Comments, @VehicleID, @MasterID" +
                ")";

            try
            {
                using (var txnScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                    {
                        using (var cmd = new SqlCommand(sql, cn))
                        {
                            cmd.Parameters.AddWithValue("@InspectionID", inspection.InspectionId);
                            cmd.Parameters.AddWithValue("@InspectionDate", inspection.InspectionDate);
                            cmd.Parameters.AddWithValue("@Inspection", inspection.InspectionValue);
                            cmd.Parameters.AddWithValue("@InspectionUserID", inspection.InspectionUserId);
                            cmd.Parameters.AddWithValue("@InspectionUser", inspection.InspectionUser);
                            cmd.Parameters.AddWithValue("@Score", inspection.Score);
                            cmd.Parameters.AddWithValue("@InspectionTypeID", inspection.InspectionTypeId);
                            cmd.Parameters.AddWithValue("@InspectionLocationID", inspection.LocationId.ToDbNullIfNoValue());
                            cmd.Parameters.AddWithValue("@Comments", inspection.Comments.TryToString("").ToUpper().ToDbNullIfNull());
                            cmd.Parameters.AddWithValue("@VehicleID", inspection.VehicleId);
                            cmd.Parameters.AddWithValue("@MasterID", inspection.MasterId);

                            await cn.OpenAsync();

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }

                    sql =
                        @"DECLARE @TempIDTable table (NewID uniqueidentifier); 
                        INSERT INTO InspectionItem (
                            InspectionID, 
                            InspectionItem, 
                            InspectionCategoryID, 
                            InspectionOptionID, 
                            Score, 
                            ItemComments, 
                            DateCreated, 
                            DateModified
                        ) 
                        OUTPUT inserted.InspectionItemID INTO @TempIDTable 
                        VALUES (
                            @InspectionID, 
                            (
                                SELECT InspectionCategory 
                                FROM InspectionCategory 
                                WHERE InspectionCategoryID = @InspectionCategoryID
                            ), 
                            @InspectionCategoryID, 
                            @InspectionOptionID, 
                            @Score, 
                            @ItemComments, 
                            GETDATE(), 
                            GETDATE()
                        ); 
                        SELECT NewID FROM @TempIDTable";

                    using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                    {
                        using (var cmd = new SqlCommand(sql, cn))
                        {
                            await cn.OpenAsync();

                            foreach (var inspectionItem in inspection.InspectionItems)
                            {
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddRange(new[]
                                    {
                                        new SqlParameter("@InspectionID", inspection.InspectionId),
                                        new SqlParameter("@InspectionCategoryID", inspectionItem.CategoryId),
                                        new SqlParameter("@Score", inspectionItem.ItemScore),
                                        new SqlParameter("@InspectionOptionID", inspectionItem.OptionId),
                                        new SqlParameter("@ItemComments", inspectionItem.ItemComments.TryToString("").ToUpper().ToDbNullIfNull()),
                                    });

                                inspectionItem.ItemId = (Guid)await cmd.ExecuteScalarAsync();
                            }
                        }
                    }

                    foreach (var inspectionItem in inspection.InspectionItems)
                    {
                        await UpdateInspectionItemWithCategoryTitle(inspectionItem.ItemId);
                    }

                    await BackfillMissingInspectionItems(inspection.InspectionId, inspection.MasterId);

                    if (inspection.ElapsedTime.HasValue)
                    {
                        await
                            LogCrElapsedTime(inspection.InspectionId, inspection.InspectionUserId, inspection.InspectionDate,
                                inspection.ElapsedTime.Value);
                    }

                    if (inspection.OpenRepair)
                    {
                        sql =
                            "IF NOT EXISTS (SELECT * FROM Repair WHERE VehicleID = @VehicleID) " +
                            "BEGIN " +
                            "    INSERT INTO Repair (VehicleID, Repair, Comments, FirstUserAccountID, AssignedUserID, DateCreated, DateModified) " +
                            "    VALUES (@VehicleID, @Repair, @Comments, @UserID, @UserID, GETDATE(), GETDATE()) " +
                            "END";

                        using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                        {
                            using (var cmd = new SqlCommand(sql, cn))
                            {
                                cmd.Parameters.AddRange(new[]
                                    {
                                        new SqlParameter("@VehicleID", inspection.VehicleId),
                                        new SqlParameter("@Repair",
                                                         NpaDataUtil.GetNextRepairOrderNumber(inspection.LocationId.Value)),
                                        new SqlParameter("@Comments", inspection.RepairComments.ToDbNullIfNull()),
                                        new SqlParameter("@UserID", inspection.InspectionUserId), 
                                    });

                                await cn.OpenAsync();
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(inspection.NewColor) || !string.IsNullOrWhiteSpace(inspection.NewMileage) || !string.IsNullOrWhiteSpace(inspection.NewVIN))
                    {
                        var @params = new Collection<SqlParameter>();

                        sql = "UPDATE Vehicle SET ";

                        if (!string.IsNullOrWhiteSpace(inspection.NewColor))
                        {
                            sql += "Color = @NewColor ";

                            if (inspection.NewColor.Length > 20) inspection.NewColor = inspection.NewColor.Substring(0, 20);
                            @params.Add(new SqlParameter("@NewColor", inspection.NewColor));
                        }

                        if (!string.IsNullOrWhiteSpace(inspection.NewMileage))
                        {
                            if (@params.Count > 0) { sql += ", "; }
                            sql += "MilesHours = @NewMilesHours ";

                            if (inspection.NewMileage.Length > 20) inspection.NewMileage = inspection.NewMileage.Substring(0, 20);
                            @params.Add(new SqlParameter("@NewMilesHours", inspection.NewMileage.Replace(",","")));
                        }

                        if (!string.IsNullOrWhiteSpace(inspection.NewVIN))
                        {
                            if (@params.Count > 0) { sql += ", "; }
                            sql += "VIN = @NewVIN ";

                            @params.Add(new SqlParameter("@NewVIN", inspection.NewVIN));
                        }

                        sql += "WHERE VehicleID = @VehicleID";
                        @params.Add(new SqlParameter("@VehicleID", inspection.VehicleId));

                        using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                        {
                            using (var cmd = new SqlCommand(sql, cn))
                            {
                                cmd.Parameters.AddRange(@params.ToArray());

                                await cn.OpenAsync();
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                    }

                    txnScope.Complete();
                }

                // check vehicle doc mileshours against CR mileshours and send email if necessary. Continue on error
                try
                {
                    sql = "EXEC spFlagMileage @VehicleID, @CurrentUserAccountID ";
                    using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                    {
                        using (var cmd = new SqlCommand(sql, cn))
                        {
                            cmd.Parameters.AddRange(new[]
                                {
                                new SqlParameter("@VehicleID", inspection.VehicleId),
                                new SqlParameter("@CurrentUserAccountID",inspection.InspectionUserId)
                            });

                            await cn.OpenAsync();
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    IwLogUtil.SendExceptionNotification(ex);
                }

                // check to see if the vehicle was here in the last 180 days and if so, has the CR changed much. Continue on error
                try
                {
                    sql = "EXEC spCheckForCRdiscrepancy @VehicleID ";
                    using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                    {
                        using (var cmd = new SqlCommand(sql, cn))
                        {
                            cmd.Parameters.AddRange(new[]
                                {
                                new SqlParameter("@VehicleID", inspection.VehicleId)
                            });

                            await cn.OpenAsync();
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    IwLogUtil.SendExceptionNotification(ex);
                }

                var updateVinCodeAtCr = NPA.Core.ConfigurationMgr.GetAMSSetting("UpdateVinCodeAtCr");
                if (inspection.InspectionTypeId == 1 && updateVinCodeAtCr == "1")
                {
                    // update VIN code / vehicle model association
                    sql = string.Format("EXEC spUpdateVINCodeVehicleModelAssociation '{0}', @VehicleModelID, @VehicleID ", inspection.NewVIN);

                    using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                    {
                        using (var cmd = new SqlCommand(sql, cn))
                        {
                            cmd.Parameters.AddRange(new[]
                                    {
                                        new SqlParameter("@VehicleID", inspection.VehicleId),
                                        new SqlParameter("@VehicleModelID",inspection.NewVehicleModelId)
                                    });

                            await cn.OpenAsync();
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }

                var newInspection = await FindInspections(new InspectionSearchRequest {InspectionId = inspection.InspectionId}, true);

                if (!string.IsNullOrWhiteSpace(inspection.NewMileage))
                {
                    newInspection[0].NewMileage = inspection.NewMileage;
                }

                if (!string.IsNullOrWhiteSpace(inspection.NewColor))
                {
                    newInspection[0].NewColor = inspection.NewColor;
                }

                return Ok(newInspection);
            }
            catch (Exception ex)
            {
                IwLogUtil.SendExceptionNotification(ex);
                return InternalServerError(ex);
            }
        }

        [NonAction]
        private static async Task BackfillMissingInspectionItems(Guid inspectionId, Guid inspectionMasterId)
        {
            var sql = "SELECT ic.InspectionCategoryID, ic.InspectionCategory " +
                      "FROM InspectionCategoryDetailView ic WITH (NOLOCK) " +
                      "WHERE ic.InspectionMasterID = @InspectionMasterID " +
                      "AND ic.InspectionCategoryID NOT IN ( " +
                      "  SELECT InspectionCategoryID " +
                      "  FROM InspectionItem " +
                      "  WHERE inspectionID = @InspectionID " +
                      ")";

            var missingCategories = new Dictionary<Guid, string>();

            try
            {
                using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@InspectionID", inspectionId);
                        cmd.Parameters.AddWithValue("@InspectionMasterID", inspectionMasterId);

                        await cn.OpenAsync();

                        using (var dr = await cmd.ExecuteReaderAsync())
                        {
                            while (await dr.ReadAsync())
                            {
                                missingCategories.Add((Guid)dr["InspectionCategoryID"], dr["InspectionCategory"].ToString());
                            }
                        }
                    }
                }

                sql = "INSERT INTO InspectionItem (" +
                      "    InspectionItem, " +
                      "    InspectionID, " +
                      "    InspectionCategoryID, " +
                      "    InspectionOptionID, " +
                      "    Score, " +
                      "    ItemComments, " +
                      "    DateCreated, " +
                      "    DateModified " +
                      ") VALUES ( " +
                      "    @InspectionItem, " +
                      "    @InspectionID, " +
                      "    @InspectionCategoryID, " +
                      "    NULL, " +
                      "    NULL, " +
                      "    NULL, " +
                      "    GETDATE(), " +
                      "    GETDATE()" +
                      ")";

                using (var txnScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                    {
                        using (var cmd = new SqlCommand(sql, cn))
                        {
                            await cn.OpenAsync();

                            foreach (var categoryId in missingCategories.Keys)
                            {
                                cmd.Parameters.Clear();

                                cmd.Parameters.AddWithValue("@InspectionID", inspectionId);
                                cmd.Parameters.AddWithValue("@InspectionItem", missingCategories[categoryId]);
                                cmd.Parameters.AddWithValue("@InspectionCategoryID", categoryId);

                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                    }

                    txnScope.Complete();
                }
            }
            catch (Exception ex)
            {
                IwLogUtil.SendExceptionNotification(ex, "Failed during backfill of 'missing' inspection items.");
                throw;
            }
        }

        [NonAction]
        private static async Task LogCrElapsedTime(Guid inspectionId, Guid userId, DateTime inspectionDate, int elapsedTime)
        {
            try
            {
                var @params = new[]
                {
                    new SqlParameter("@Activity", "[NEW INSPECTION - NEW (IPAD)]"),
                    new SqlParameter("@ActivityTypeID",
                        WebConfigurationManager.AppSettings["InspectionActivityTypeId"].ParseTo<Guid>()),
                    new SqlParameter("@ActivityUserID", userId),
                    new SqlParameter("@ActivityDate", inspectionDate),
                    new SqlParameter("@Duration", (double) elapsedTime),
                    new SqlParameter("@LinkEntity", "Inspection"),
                    new SqlParameter("@LinkEntityID", inspectionId),
                    new SqlParameter("@LinkTypeID",
                        WebConfigurationManager.AppSettings["InspectionLinkTypeId"].ParseTo<int>()),
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
                IwLogUtil.SendExceptionNotification(ex, "Failed during call to LogCrElapsedTime");
                throw;
            }
        }

        [NonAction]
        private static async Task UpdateInspectionItemWithCategoryTitle(Guid inspectionItemId)
        {
            var sql =
                "UPDATE InspectionItem " +
                "SET InspectionItem = ic.InspectionCategory " +
                "FROM InspectionCategory ic " +
                "JOIN InspectionItem ii ON ic.InspectionCategoryID = ii.InspectionCategoryID " +
                "WHERE ii.InspectionItemID = @InspectionItemID";

            try
            {
                using (var txnScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                    {
                        using (var cmd = new SqlCommand(sql, cn))
                        {
                            cmd.Parameters.AddWithValue("@InspectionItemID", inspectionItemId);

                            await cn.OpenAsync();

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }

                    txnScope.Complete();
                }
            }
            catch (Exception ex)
            {
                IwLogUtil.SendExceptionNotification(ex, "Failed during call to UpdateInspectionItemWithCategoryTitle");
                throw;
            }
        }

        [NonAction]
        private static async Task<bool> InspectionExists(Guid inspectionId)
        {
            var sql =
                "SELECT CASE WHEN EXISTS (SELECT * FROM Inspection WHERE InspectionID = @InspectionID) THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END";

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@InspectionID", inspectionId);

                    await cn.OpenAsync();

                    var result = await cmd.ExecuteScalarAsync();

                    return (bool)result;
                }
            }
        }

        [NonAction]
        public static async Task<IList<InspectionMaster>> GetMasters(Guid? inspectionId, Guid? vehicleId)
        {
            var sql =
                "IF @InspectionID IS NOT NULL " +
                "BEGIN " +
                "	SELECT InspectionMasterID, InspectionMaster, Hidden, CategoryDefault, MaxScore FROM ( " +
                "        SELECT im.InspectionMasterID, im.InspectionMaster, CAST(ISNULL(im.Hide, 0) AS bit) AS Hidden, CAST(im.CategoryDefault AS bit) AS CategoryDefault, 1 AS Actual, " +
                "        CAST(im.MaxInspectionScore AS int) AS MaxScore " +
                "        FROM InspectionMaster im WITH (NOLOCK) " +
                "        JOIN Inspection i WITH (NOLOCK) ON im.InspectionMasterID = i.InspectionMasterID " +
                "        WHERE i.InspectionID = @InspectionID " +
                "    UNION " +
                "        SELECT im.InspectionMasterID, im.InspectionMaster, CAST(ISNULL(im.Hide, 0) AS bit) AS Hidden, CAST(CategoryDefault AS bit), 2, " +
                "        CAST(im.MaxInspectionScore AS int) AS MaxScore " +
                "        FROM InspectionMaster im WITH (NOLOCK) " +
                "        JOIN Vehicle v WITH (NOLOCK) on v.VehicleCategoryID = im.VehicleCategoryID " +
                "        JOIN Inspection i WITH (NOLOCK) ON i.VehicleID = v.VehicleID AND im.InspectionMasterid <> i.InspectionMasterID " +
                "        WHERE i.InspectionID = @InspectionID " +
                "    ) t " +
                "    ORDER BY Actual, CategoryDefault DESC, Hidden ASC; " +
                "END " +
                "ELSE IF @VehicleID IS NOT NULL " +
                "BEGIN " +
                "   IF dbo.fn_IsFactory(@VehicleID) = 1 " +
                "   BEGIN " +
                "	  SELECT im.InspectionMasterID, im.InspectionMaster, CAST(ISNULL(im.Hide, 0) AS bit) AS Hidden, CAST(im.CategoryDefault AS bit) AS CategoryDefault, " +
                "     CAST(im.MaxInspectionScore AS int) AS MaxScore, 1 AS Ordered " +
                "	  FROM InspectionMaster im WITH (NOLOCK) " +
                "	  JOIN Vehicle v WITH (NOLOCK) ON v.VehicleCategoryID = im.VehicleCategoryID " +
                "	  WHERE v.VehicleID = @VehicleID " +
                "     AND (im.InspectionMaster LIKE '*OEM/%') " +
                "   UNION " +
                "	  SELECT im.InspectionMasterID, im.InspectionMaster, CAST(ISNULL(im.Hide, 0) AS bit) AS Hidden, CAST(im.CategoryDefault AS bit) AS CategoryDefault, " +
                "     CAST(im.MaxInspectionScore AS int) AS MaxScore, 2 AS Ordered " +
                "	  FROM InspectionMaster im WITH (NOLOCK) " +
                "	  JOIN Vehicle v WITH (NOLOCK) ON v.VehicleCategoryID = im.VehicleCategoryID " +
                "	  WHERE v.VehicleID = @VehicleID " +
                "     AND (im.InspectionMaster LIKE '%Factory%') " +
                "	  ORDER BY Ordered ASC, CAST(im.CategoryDefault AS bit) DESC, CAST(ISNULL(im.Hide, 0) AS bit) ASC; " +
                "   END " +
                "  ELSE " +
                "   BEGIN " +
                "	  SELECT im.InspectionMasterID, im.InspectionMaster, CAST(ISNULL(im.Hide, 0) AS bit) AS Hidden, CAST(im.CategoryDefault AS bit) AS CategoryDefault, " +
                "     CAST(im.MaxInspectionScore AS int) AS MaxScore " +
                "	  FROM InspectionMaster im WITH (NOLOCK) " +
                "	  JOIN Vehicle v WITH (NOLOCK) ON v.VehicleCategoryID = im.VehicleCategoryID " +
                "	  WHERE v.VehicleID = @VehicleID " +
                "     AND im.InspectionMaster NOT LIKE '*OEM/%' " +
                "	  ORDER BY im.CategoryDefault DESC, ISNULL(im.Hide, 0) ASC; " +
                "   END " +
                "END";

            var ret = new List<InspectionMaster>();

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@InspectionID", inspectionId.ToDbNullIfNoValue());
                    cmd.Parameters.AddWithValue("@VehicleID", vehicleId.ToDbNullIfNoValue());

                    await cn.OpenAsync();

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            ret.Add(new InspectionMaster
                            {
                                MasterId = (Guid)dr["InspectionMasterID"],
                                DisplayName = dr["InspectionMaster"].ToString().TrimEnd(new[] { '.' }),
                                DefaultForCategory = (bool)dr["CategoryDefault"],
                                Hidden = (bool)dr["Hidden"],
                                MaxInspectionScore = (int)dr["MaxScore"],
                                RequiredCategoryIds = await RequiredCategoryIds((Guid)dr["InspectionMasterID"]),
                            });
                        }
                    }
                }
            }
            return ret;
        }

        [HttpGet]
        [Route("{inspectionId}")]
        public async Task<IHttpActionResult> GetInspection(Guid inspectionId)
        {
            var userAccountId = (Guid)HttpContext.Current.Items["UserAccountGuid"];
            
            var inspections = await FindInspections(new InspectionSearchRequest { InspectionId = inspectionId, UserId = userAccountId }, false);

            if (inspections == null || inspections.Count < 1)
            {
                return NotFound();
            }

            var inspection = inspections.Single();

            return Ok(inspection);
        }

        [NonAction]
        internal static async Task<IDictionary<string, IList<Inspection>>> GetInspectionsForVehicle(Guid vehicleId, string userName)
        {
            try
            {
                var sql =
                "SELECT i.InspectionID, i.InspectionUser, i.InspectionUserID, i.InspectionDate, CAST(i.Score AS int) AS Score, it.InspectionType, sn.StockNumber, i.InspectionMasterID," +
                "im.InspectionMaster, i.Comments, it.InspectionTypeID, v1.MilesHours, v1.Color, i.InspectionLocationID, " +
                "(CASE WHEN InspectionUser = '" + userName + "' THEN 1 ELSE 0 END) AS allowEditing " +
                "FROM Inspection i WITH (NOLOCK) " +
                "JOIN InspectionMaster im WITH (NOLOCK) ON i.InspectionMasterID = im.InspectionMasterID " +
                "JOIN InspectionType it WITH (NOLOCK) ON it.InspectionTypeID = i.InspectionType " +
                "JOIN Vehicle v1 WITH (NOLOCK) ON i.VehicleID = v1.VehicleID " +
                "JOIN Vehicle v2 WITH (NOLOCK) ON v1.VIN = v2.VIN " +
                "JOIN StockNumber sn WITH (NOLOCK) ON sn.StockNumberID = v1.StockNumberID " +
                "WHERE v2.VehicleID = @VehicleID " +
                "ORDER BY i.InspectionDate ASC";

                var inspections = new List<Inspection>();

                using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                {
                    await cn.OpenAsync();

                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@VehicleID", vehicleId));

                        using (var dr = await cmd.ExecuteReaderAsync())
                        {
                            while (await dr.ReadAsync())
                            {
                                inspections.Add(new Inspection
                                {
                                    VehicleId = vehicleId,
                                    InspectionId = (Guid)dr["InspectionID"],
                                    InspectionDate = (DateTime)dr["InspectionDate"],
                                    InspectionUser = dr["InspectionUser"].ToString(),
                                    InspectionUserId = (dr["InspectionUserID"] != null ? (Guid)dr["InspectionUserID"] : Guid.Empty),
                                    InspectionTypeId = (int)dr["InspectionTypeID"],
                                    InspectionType = dr["InspectionType"].ToString(),
                                    Score = (int)dr["Score"],
                                    LocationId = dr["InspectionLocationID"] as Guid?,
                                    StockNumber = dr["StockNumber"].ToString(),
                                    Comments = dr["Comments"].ToString().ToNullIfWhiteSpace(),
                                    MasterId = (Guid)dr["InspectionMasterID"],
                                    MasterDisplayName = dr["InspectionMaster"].ToString().TrimEnd(new[] { ',' }),
                                    InspectionMilesHours = dr["MilesHours"].ToString().ToNullIfWhiteSpace(),
                                    InspectionColor = dr["Color"].ToString().ToNullIfWhiteSpace(),
                                    AllowEditing = Convert.ToBoolean(dr["allowEditing"])
                                });
                            }
                        }
                    }
                }

                var groupedInspections =
                    from inspection in inspections
                    orderby inspection.InspectionDate descending
                    group inspection by inspection.StockNumber into grp
                    select grp;

                var inspectionGroups = groupedInspections as IGrouping<string, Inspection>[] ?? groupedInspections.ToArray();

                var ret = new Dictionary<string, IList<Inspection>>(inspectionGroups.Count());

                foreach (var inspectionGroup in inspectionGroups)
                {
                    ret.Add(inspectionGroup.Key, new List<Inspection>());

                    foreach (var inspection in inspectionGroup)
                    {
                        ret[inspectionGroup.Key].Add(inspection);
                    }
                }

                return ret;
            }
            catch (Exception ex)
            {
                IwLogUtil.SendExceptionNotification(ex);
                return  new Dictionary<string, IList<Inspection>>();
            }
            
        }

        [NonAction]
        internal static async Task<Inspection> GetInspectionForVehicle(Guid inspectionId)
        {
            var sql =
                "SELECT i.VehicleID, i.InspectionUser, i.InspectionUserID, i.InspectionDate, CAST(i.Score AS int) AS Score, it.InspectionType, sn.StockNumber, i.InspectionMasterID," +
                "im.InspectionMaster, i.Comments, it.InspectionTypeID, v.MilesHours, v.Color, i.InspectionLocationID, " +
                "0 AS allowEditing " +
                "FROM Inspection i WITH (NOLOCK) " +
                "JOIN InspectionMaster im WITH (NOLOCK) ON i.InspectionMasterID = im.InspectionMasterID " +
                "JOIN InspectionType it WITH (NOLOCK) ON it.InspectionTypeID = i.InspectionType " +
                "JOIN Vehicle v WITH (NOLOCK) ON i.VehicleID = v.VehicleID " +
                "JOIN StockNumber sn WITH (NOLOCK) ON sn.StockNumberID = v.StockNumberID " +
                "WHERE i.InspectionID = @InspectionID ";

            Inspection inspection = null;

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                await cn.OpenAsync();

                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.Add(new SqlParameter("@InspectionID", inspectionId));

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            inspection = new Inspection()
                            {
                                VehicleId = (Guid)dr["VehicleID"],
                                InspectionId = inspectionId,
                                InspectionDate = (DateTime)dr["InspectionDate"],
                                InspectionUser = dr["InspectionUser"].ToString(),
                                InspectionUserId = (Guid)dr["InspectionUserID"],
                                InspectionTypeId = (int)dr["InspectionTypeID"],
                                InspectionType = dr["InspectionType"].ToString(),
                                Score = (int)dr["Score"],
                                LocationId = dr["InspectionLocationID"] as Guid?,
                                StockNumber = dr["StockNumber"].ToString(),
                                Comments = dr["Comments"].ToString().ToNullIfWhiteSpace(),
                                MasterId = (Guid)dr["InspectionMasterID"],
                                MasterDisplayName = dr["InspectionMaster"].ToString().TrimEnd(new[] { ',' }),
                                InspectionMilesHours = dr["MilesHours"].ToString().ToNullIfWhiteSpace(),
                                InspectionColor = dr["Color"].ToString().ToNullIfWhiteSpace(),
                                AllowEditing = Convert.ToBoolean(dr["allowEditing"])
                            };
                        }
                    }
                }
            }

            return inspection;
        }



        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetInspections([FromUri]InspectionSearchRequest request)
        {
            request.UserId = (Guid) HttpContext.Current.Items["UserAccountGuid"];

            var inspections = await FindInspections(request, false);

            if (inspections == null || inspections.Count < 1)
            {
                return NotFound();
            }
            
            return Ok(inspections);
        }

        [NonAction]
        private static async Task<IList<Guid>> RequiredCategoryIds(Guid masterId)
        {
            var sql =
               "SELECT InspectionCategoryID " +
               "FROM InspectionCategoryDetailView WITH (NOLOCK) " +
               "WHERE InspectionMasterID = @MasterID " +
               "AND ISNULL(Hide, 0) = 0 " +
               "AND ISNULL(IncludeInHeader, 0) = 1 " +
               "ORDER BY DisplayOrder ASC, CategoryWeight DESC, InspectionCategory ASC";

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@MasterID", masterId);

                    await cn.OpenAsync();

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        var ret = new List<Guid>();

                        while (await dr.ReadAsync())
                        {
                            ret.Add((Guid)dr["InspectionCategoryID"]);
                        }

                        return ret;
                    }
                }
            }
        }

        internal static async Task<IList<Inspection>> FindInspections(InspectionSearchRequest request, bool? includeLatestColorAndMileage)
        {
            var @params = new Collection<SqlParameter>();
            var sql =
                "SELECT i.InspectionID, it.InspectionType, CAST(i.Score AS int) AS Score, i.InspectionDate, i.InspectionUser, i.InspectionUserID ";

            if (includeLatestColorAndMileage.GetValueOrDefault(false))
            {
                sql += ", v.MilesHours, v.Color ";
            }

            //sql += ", (CASE WHEN InspectionUser = (SELECT TOP 1 UserAccount FROM UserAccount WHERE UserAccountID = @UserID AND IsNull(Hide,0) = 0) THEN 1 ELSE 0 END) AS allowEditing ";
            //@params.Add(new SqlParameter("@UserID", request.UserId));

            sql += "FROM Inspection i WITH (NOLOCK) " +
                   "JOIN InspectionType it WITH (NOLOCK) ON it.InspectionTypeID = i.InspectionType ";

            if (!string.IsNullOrWhiteSpace(request.Vin) || includeLatestColorAndMileage.GetValueOrDefault(false))
            {
                sql += "JOIN Vehicle v WITH (NOLOCK) ON i.VehicleID = v.VehicleID ";
            }

            sql += "WHERE 1 = 1 ";

            if (request.VehicleId.HasValue)
            {
                sql += "AND i.VehicleID = @VehicleID ";
                @params.Add(new SqlParameter("@VehicleID", request.VehicleId.Value));
            }

            if (request.InspectionId.HasValue)
            {
                sql += "AND InspectionID = @InspectionID ";
                @params.Add(new SqlParameter("@InspectionID", request.InspectionId.Value));
            }

            if (!string.IsNullOrWhiteSpace(request.Vin))
            {
                sql += "AND v.VIN = @VIN ";
                @params.Add(new SqlParameter("@VIN", request.Vin));
            }
            
            sql += "ORDER BY i.InspectionDate DESC";

            if (!@params.Any())
            {
                return null;
            }
           
            var ret = new List<Inspection>();

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                await cn.OpenAsync();

                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddRange(@params.ToArray());

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            var tempInspection = new Inspection
                            {
                                InspectionId = (Guid)dr["InspectionID"],
                                InspectionType = dr["InspectionType"].ToString(),
                                InspectionUser = dr["InspectionUser"].ToString(),
                                Score = dr["Score"] as int?,
                                InspectionDate =
                                    DateTime.SpecifyKind((DateTime)dr["InspectionDate"], DateTimeKind.Local),
                                //AllowEditing = Convert.ToBoolean(dr["allowEditing"])
                            };

                            if (includeLatestColorAndMileage.GetValueOrDefault(false))
                            {
                                tempInspection.NewColor = dr["Color"].ToString().ToNullIfWhiteSpace();
                                tempInspection.NewMileage = dr["MilesHours"].ToString().ToNullIfWhiteSpace();
                            }

                            ret.Add(tempInspection);
                        }
                    }
                }
            }

            if (ret.Count < 1)
            {
                return null;
            }

            return ret;
        }

        [Route("{inspectionId}/items")]
        [HttpGet]
        public async Task<IHttpActionResult> GetInspectionItems(Guid inspectionId)
        {
            var sql =
                "SELECT ii.InspectionItemID, ii.InspectionCategoryID, ii.InspectionOptionID, ii.ItemComments " +
                "FROM InspectionItem ii WITH (NOLOCK) " +
                "WHERE ii.InspectionID = @InspectionID";

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                await cn.OpenAsync();

                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@InspectionID", inspectionId);

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        var ret = new List<InspectionItem>();

                        while (await dr.ReadAsync())
                        {
                            ret.Add(new InspectionItem
                            {
                                ItemId = (Guid)dr["InspectionItemID"],
                                OptionId = dr["InspectionOptionID"] as Guid?,
                                ItemComments = dr["ItemComments"].ToString().ToNullIfWhiteSpace(),
                                CategoryId = (Guid)dr["InspectionCategoryID"],
                            });
                        }

                        return Ok(ret);
                    }
                }
            }
        }

        [NonAction]
        public static IEnumerable<InspectionType> GetInspectionTypes()
        {
            return
                NPA.CodeGen.InspectionType.GetValues()
                    .OrderBy(it => it.InspectionTypeId)
                    .Select(it => new InspectionType(it))
                    .ToList();
        }

        [HttpGet]
        [Route("inspectionType/{inspectionTypeId}")]
        public IHttpActionResult GetInspectionType(int inspectionTypeId)
        {
            var types = GetInspectionTypes();

            return Ok(types.Single(t => t.InspectionTypeId == inspectionTypeId));
        }


        public InspectionType GetInspectionTypeById(int inspectionTypeId)
        {
            var types = GetInspectionTypes();

            return  types.Single(t => t.InspectionTypeId == inspectionTypeId);
        }

        [HttpGet]
        [Route("{inspectionId}/images")]
        public async Task<IHttpActionResult> GetInspectionImages(Guid inspectionId)
        {
            var sql =
                "SELECT ip.PictureID, CAST(COALESCE(p.PublicViewOrder, ROW_NUMBER() OVER(ORDER BY p.DateCreated ASC)) AS int) AS PublicViewOrder, " +
                "CASE WHEN ISNULL(p.ImageW, 0) > ISNULL(p.ImageH, 0) THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS IsLandscape, " +
                "pl.LocationID " +
                "FROM InspectionPicture ip WITH (NOLOCK) " +
                "LEFT OUTER JOIN PictureLocation pl WITH (NOLOCK) ON pl.PictureID = ip.PictureID " +
                "JOIN Picture p WITH (NOLOCK) ON p.PictureID = ip.PictureID " +
                "WHERE ip.InspectionID = @InspectionID";

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                await cn.OpenAsync();

                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@InspectionID", inspectionId);

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        var ret = new List<InspectionImage>();

                        while (await dr.ReadAsync())
                        {
                            ret.Add(new InspectionImage
                            {
                                PictureId = (Guid)dr["PictureID"],
                                DisplayOrder = (int)dr["PublicViewOrder"],
                                IsLandscape = (bool)dr["IsLandscape"],
                            });
                        }
                        return Ok(ret);
                    }
                }
            }
        }


        [HttpGet]
        [Route("getInspectionImagesWithURL/{inspectionId}")]
        public async Task<IHttpActionResult> GetInspectionImagesWithURL(Guid inspectionId)
        {
            try
            {
                var sql =
               "SELECT ip.PictureID, CAST(COALESCE(p.PublicViewOrder, ROW_NUMBER() OVER(ORDER BY p.DateCreated ASC)) AS int) AS PublicViewOrder, " +
               "CASE WHEN ISNULL(p.ImageW, 0) > ISNULL(p.ImageH, 0) THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS IsLandscape, " +
               "pl.LocationID, CAST(imageW AS int) imageW, CAST(imageH AS int) imageH " +
               "FROM InspectionPicture ip WITH (NOLOCK) " +
               "LEFT OUTER JOIN PictureLocation pl WITH (NOLOCK) ON pl.PictureID = ip.PictureID " +
               "JOIN Picture p WITH (NOLOCK) ON p.PictureID = ip.PictureID " +
               "WHERE ip.InspectionID = @InspectionID " +
               "ORDER BY PublicViewOrder ";

                using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                {
                    await cn.OpenAsync();

                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@InspectionID", inspectionId);

                        using (var dr = await cmd.ExecuteReaderAsync())
                        {
                            var ret = new List<InspectionImage>();
                            string imageBaseUrl = null;

                            while (await dr.ReadAsync())
                            {
                                if(imageBaseUrl == null) { imageBaseUrl = GetLocationImageBaseURL(dr["LocationID"].TryToString()); }
                                ret.Add(new InspectionImage
                                {
                                    PictureId = (Guid)dr["PictureID"],
                                    DisplayOrder = (int)dr["PublicViewOrder"],
                                    IsLandscape = (bool)dr["IsLandscape"],
                                    ImageBaseUrl = ((int)dr["PublicViewOrder"] == 1 ? imageBaseUrl : ""),
                                    Width = (int?)dr["imageW"],
                                    Height = (int?)dr["imageH"]
                                });
                            }

                            return Ok(ret);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //public string GetLocationImageBaseURL(string locationId)
        //{
        //    if (locationId.IsEmpty()) { return null; }

        //    string amsBaseUrl = "";
        //    string imageBaseUrl = "/library/Image.aspx?PictureID={0}";

        //    switch (locationId.ToUpper())
        //    {
        //        case Locations.SAN:
        //        case Locations.REMOTE:
        //            amsBaseUrl = "http://amssan.npauctions.com/amsproto";
        //            break;
        //        case Locations.DAL:
        //            amsBaseUrl = "http://amsdal.npauctions.com/amsproto";
        //            break;
        //        case Locations.CIN:
        //            amsBaseUrl = "http://amscin.npauctions.com/amsproto";
        //            break;
        //        case Locations.ATL:
        //            amsBaseUrl = "http://amsatl.npauctions.com/amsproto";
        //            break;
        //        default:
        //            amsBaseUrl = "http://DEV-STAGING-VM/amsproto";
        //            break;
        //    }


        //    return amsBaseUrl + imageBaseUrl;
        //}

        public string GetLocationImageBaseURL(string locationId)
        {
            if (locationId.IsEmpty()) { return null; }

            string amsBaseUrl = "";
            string imageBaseUrl = "/library/Image.aspx?PictureID={0}";

            string sql = "SELECT LocalURL FROM Location WITH (NOLOCK) WHERE LocationID = @LocationID";

            amsBaseUrl = NPA.Core.SqlHelper.ExecuteScalar(ApiSettings.ConnectionString, CommandType.Text, sql, new[] { new SqlParameter("@LocationID", locationId) }).TryToString();

            return amsBaseUrl + imageBaseUrl;
        }

        [Route("uploadSuccess/{inspectionId}/{imageCount}")]
        [HttpGet]
        public async Task<IHttpActionResult> ImageUploadSuccess(Guid inspectionId, int imageCount)
        {
            try
            {
                var sql =
                @"SELECT (CASE WHEN count(p.pictureID) <> @Count THEN 0 ELSE 1 END) AS CountGood " +
                 "FROM Inspection i " +
                 "INNER JOIN Vehicle v ON i.VehicleID = v.VehicleID " +
                 "INNER JOIN InspectionPicture ip ON i.InspectionID = ip.InspectionID " +
                 "INNER JOIN Picture p ON ip.PictureID = p.PictureID " +
                 "WHERE i.InspectionID = @InspectionID";

                using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                {
                    cn.Open();
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@InspectionID", inspectionId);
                        cmd.Parameters.AddWithValue("@Count", imageCount);

                        var ret = cmd.ExecuteScalar().TryToString();

                        if (ret.IsEmpty() || ret == "0")
                        {
                            var images = await GetInspectionImages(inspectionId);
                            List<InspectionImage> pictureIds = ((System.Web.Http.Results.OkNegotiatedContentResult<List<InspectionImage>>)images).Content;
                            string picIds = pictureIds.Aggregate("", (a, b) => a + "|" + b.PictureId.TryToString(""));
                            string[] result = new string[] { "status:false", "PictureIds:" + picIds };
                            return Ok(result);
                        }
                        else
                        {
                            string[] result = new string[] { "status:true", "PictureIds:" };
                            return Ok(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            
        }

        [Route("amsSetting/{setting}")]
        [HttpGet]
        public Task<IHttpActionResult> GetAMSSetting(string setting)
        {
            return GetAMSSetting(setting, null);
        }

        [Route("amsSetting/{setting}/{locationId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAMSSetting(string setting, string locationId)
        {
            var sql = "SELECT Value FROM Setting WITH (NOLOCK) WHERE setting = @Setting ";

            if (!string.IsNullOrEmpty(locationId)) { 
                sql += "AND LocationID = @LocationID"; 
            }

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                await cn.OpenAsync();

                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Setting", setting);
                    if (!string.IsNullOrEmpty(locationId))
                    {
                        cmd.Parameters.AddWithValue("@LocationID", locationId);
                    }

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        string[] ret = new string[1];

                        while (await dr.ReadAsync())
                        {
                            ret[0] = dr["Value"].TryToString();
                        }

                        return Ok(ret);
                    }
                }
            }
        }

        [NonAction]
        private static async Task<IList<InspectionCategory>> CategoriesForMasterWithId(Guid masterId)
        {
            var sql =
                "SELECT InspectionCategoryID, InspectionCategory, CAST(MaxCategoryScore AS int) AS MaxCategoryScore, " +
                "CAST(CategoryWeight AS int) AS CategoryWeight, CAST(IncludeInHeader AS bit) AS Required " +
                "FROM InspectionCategoryDetailView " +
                "WHERE InspectionMasterID = @MasterID " +
                "AND ISNULL(Hide, 0) = 0 " +
                "ORDER BY DisplayOrder ASC, CategoryWeight DESC, InspectionCategory ASC";

            var ret = new List<InspectionCategory>();

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@MasterID", masterId);

                    await cn.OpenAsync();

                    using (var dr = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult))
                    {
                        while (await dr.ReadAsync())
                        {
                            ret.Add(new InspectionCategory
                            {
                                CategoryId = (Guid)dr["InspectionCategoryID"],
                                DisplayName = dr["InspectionCategory"].ToString().TrimEnd(new[] { '.' }),
                                MaxScore = (int)dr["MaxCategoryScore"],
                                Weight = (int)dr["CategoryWeight"],
                                Required = (bool)dr["Required"],
                                Options = await OptionsForCategoryWithId((Guid)dr["InspectionCategoryID"]),
                            });
                        }
                    }
                }
            }

            return ret;
        }

        [NonAction]
        private static async Task<IList<InspectionOption>> OptionsForCategoryWithId(Guid categoryId)
        {
            var sql =
                "SELECT iodv.InspectionOptionID, iodv.InspectionOption, CAST(iodv.ScoreValue AS int) AS ScoreValue " +
                "FROM InspectionOptionDetailView iodv " +
                "WHERE iodv.InspectionCategoryID = @CategoryID " +
                "ORDER BY iodv.ScoreValue DESC, iodv.InspectionOption ASC";

            var ret = new List<InspectionOption>();

            using (var txnScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryID", categoryId);

                        await cn.OpenAsync();

                        using (var dr = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult))
                        {
                            while (await dr.ReadAsync())
                            {
                                ret.Add(new InspectionOption
                                {
                                    OptionId = (Guid)dr["InspectionOptionID"],
                                    DisplayName = dr["InspectionOption"].ToString(),
                                    Value = (int)dr["ScoreValue"],
                                });
                            }
                        }
                    }
                } 

                txnScope.Complete();
            }

            return ret;
        }

        [NonAction]
        internal static async Task<IList<InspectionType>> AvailableInspectionTypes(Guid? vehicleId, Guid? inspectionId)
        {
            var types = GetInspectionTypes().ToList();

            string sql;

            if (inspectionId.HasValue && !vehicleId.HasValue)
            {
                sql = "SELECT VehicleID FROM Inspection WITH (NOLOCK) WHERE InspectionID = @InspectionID";

                using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@InspectionID", inspectionId.Value);

                        await cn.OpenAsync();

                        vehicleId = await cmd.ExecuteScalarAsync() as Guid?;
                    }
                }
            }

            if (!vehicleId.HasValue)
            {
                return types.OrderBy(it => it.InspectionTypeId).ToList();
            }

            sql =
                "SELECT ISNULL(CAST((SELECT ISNULL(dbo.Sale.Redemption,0) FROM dbo.Sale WITH (NOLOCK) WHERE VehicleID = @VehicleID) AS bit), 0) AS IsRedemption, " +
                "   CASE WHEN EXISTS (SELECT * FROM dbo.Inspection WITH (NOLOCK) WHERE VehicleID = @VehicleID AND InspectionType='1') THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS PreInspExists, " +
                "   CASE WHEN EXISTS (SELECT * FROM dbo.Inspection WITH (NOLOCK) WHERE VehicleID = @VehicleID AND InspectionType='3') THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS RedemptionInspExists, " +
                "   CASE WHEN EXISTS (SELECT * FROM dbo.Inspection WITH (NOLOCK) WHERE VehicleID = @VehicleID AND InspectionType='4') THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS TransferInspExists, " +
                "   CASE WHEN EXISTS (SELECT * FROM dbo.Transit WITH (NOLOCK) WHERE VehicleID = @VehicleID AND ISNULL(TransitTypeID, 0) = 2) THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS RedemptionExists, " +
                "   CASE WHEN EXISTS (SELECT * FROM dbo.Transit WITH (NOLOCK) WHERE VehicleID = @VehicleID AND ISNULL(TransitTypeID, 0) = 3) THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS TransferExists";
            
            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@VehicleID", vehicleId.Value);

                    await cn.OpenAsync();

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            // if is a redemption, than only allow this type
                            if (TypeUtil.GetSafeBoolean(dr["IsRedemption"]) && !(bool)dr["RedemptionInspExists"])
                            {
                                types.RemoveAll(
                                    it => it.InspectionTypeId != NPA.CodeGen.InspectionType.Redemption.InspectionTypeId);
                            }
                            else if (!(bool)dr["PreInspExists"])
                            {
                                types.RemoveAll(
                                    it => it.InspectionTypeId != NPA.CodeGen.InspectionType.PreInspection.InspectionTypeId);
                            }
                            else
                            {
                                // Set this to either redemption, transfer or a post. do not let them choose. Per MH 10/4/16
                                if ((bool)dr["TransferExists"] && !(bool)dr["TransferInspExists"]) // no transfer record was found, so remove this type
                                {
                                    types.RemoveAll(
                                        it => it.InspectionTypeId != NPA.CodeGen.InspectionType.TransferInspection.InspectionTypeId);
                                }
                                //else if ((bool)dr["RedemptionExists"] && !(bool)dr["RedemptionInspExists"]) // no redemption transfer record exists, so remove this type
                                //{
                                //    types.RemoveAll(
                                //        it => it.InspectionTypeId != NPA.CodeGen.InspectionType.Redemption.InspectionTypeId);
                                //}
                                else
                                {
                                    types.RemoveAll(
                                        it => it.InspectionTypeId != NPA.CodeGen.InspectionType.PostInspection.InspectionTypeId);
                                }

                            }                           
                        }
                    }

                    return types.OrderBy(it => it.InspectionTypeId).ToList();
                }
            }
        }




    }
}