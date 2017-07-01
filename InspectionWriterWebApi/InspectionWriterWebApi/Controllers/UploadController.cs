using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IdentityModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Hosting;
using System.Web.Configuration;
using System.Web.Http;
using InspectionWriterWebApi.Models;
using NPA.Common;
using InspectionWriterWebApi.Utilities;

namespace InspectionWriterWebApi.Controllers
{
    [RoutePrefix("upload")]
    public class UploadController : ApiController
    {
        [HttpPost]
        [Route("inspectionImage")]
        public async Task<IHttpActionResult> UploadImage()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                return InternalServerError(new HttpResponseException(HttpStatusCode.UnsupportedMediaType));
            }

            var root = HttpContext.Current.Server.MapPath("~/App_Data");

            var provider = new MultipartFormDataStreamProvider(root);

            var ret = new Dictionary<int, string>();

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);
                
                var inspectionId = provider.FormData["InspectionId"].ParseTo<Guid>();

                //string values = "InspectionId: " + inspectionId.ToString();
                //LogUtil.WriteLogEntry("InspectionWriterWebApi", values, System.Diagnostics.EventLogEntryType.Information);

                var existingPictureIds = await GetExistingPictureIds(inspectionId);

                var serverLocationId = WebConfigurationManager.AppSettings["ServerLocationId"].ParseTo<Guid>();
                
                var loggedFileErrorOnce = false;

                // We're uploading images 1 at a time now so there should only be 1 "file"
                foreach (var file in provider.FileData)
                {
                    var displayOrder = 0;

                    try
                    {
                        string tempParamValue;
                        var fileParams = file.Headers.ContentDisposition.Parameters.AsEnumerable();

                        var tmp = fileParams.FirstOrDefault( p => p.Name.Equals("pictureId", StringComparison.OrdinalIgnoreCase) );
                        if (tmp == null) { continue; }
                        else { tempParamValue = tmp.Value; }

                        var pictureId = Regex.Replace(tempParamValue, @"[^a-fA-F0-9-]", string.Empty).ParseTo<Guid>();

                        if (existingPictureIds.Contains(pictureId))
                        {
                            continue;
                        }

                        tempParamValue = fileParams.FirstOrDefault( p => p.Name.Equals("displayOrder", StringComparison.OrdinalIgnoreCase) ).Value;
                        displayOrder = Regex.Replace(tempParamValue, @"\D", string.Empty).ParseTo<int>();

                        tempParamValue = fileParams.FirstOrDefault(p => p.Name.Equals("width", StringComparison.OrdinalIgnoreCase)).Value;
                        var width = Regex.Replace(tempParamValue, @"\D", string.Empty).ParseTo<int>();

                        tempParamValue = fileParams.FirstOrDefault(p => p.Name.Equals("height", StringComparison.OrdinalIgnoreCase)).Value;
                        var height = Regex.Replace(tempParamValue, @"\D", string.Empty).ParseTo<int>();

                        var targetImagePath = await GetImageSavePath(serverLocationId);

                        var sql =
                            "INSERT INTO Picture (PictureID, URL, ImageW, ImageH, PublicViewOrder, DateCreated, DateModified) " +
                            "VALUES (@PictureID, @URL, @ImageW, @ImageH, @PublicViewOrder, GETDATE(), GETDATE());";

                        var @params = new[]
                        {
                            new SqlParameter("@PictureID", pictureId),
                            new SqlParameter("@URL", Path.GetFileName(targetImagePath)),
                            new SqlParameter("@ImageW", width),
                            new SqlParameter("@ImageH", height),
                            new SqlParameter("@PublicViewOrder", displayOrder),
                        };

                        using (var txnScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                        {
                            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                            {
                                using (var cmd = new SqlCommand(sql, cn))
                                {
                                    cmd.Parameters.AddRange(@params);

                                    await cn.OpenAsync();

                                    await cmd.ExecuteNonQueryAsync();
                                }
                            }

                            await CreatePictureLocationRecord(pictureId);
                            await CreateInspectionPictureRecord(pictureId, inspectionId);

                            string sourceHash;

                            using (var source = File.Open(file.LocalFileName, FileMode.Open))
                            {
                                sourceHash = source.ComputeMd5Hash();
                            }
                            // this will both save the file to a local temp cache for viewing and stage the file for the move to picdata
                            //Create the gallery thumbnail
                            string cacheImageName = GetCacheImageFileBaseName(pictureId.ToString(), 160, 120);
                            cacheImageName = Path.Combine(WebConfigurationManager.AppSettings["AMSCacheImagePath"].TryToString(), cacheImageName);
                            //File.Copy(file.LocalFileName, cacheImageName);
                            ImageUtils.ShrinkAndSaveImage(file.LocalFileName, cacheImageName, 160, 120);

                            //save the default image if necessary
                            if (displayOrder == 1)
                            {
                                cacheImageName = GetCacheImageFileBaseName(pictureId.ToString(), 320, 240);
                                cacheImageName = Path.Combine(WebConfigurationManager.AppSettings["AMSCacheImagePath"].TryToString(), cacheImageName);
                                //File.Copy(file.LocalFileName, cacheImageName);
                                ImageUtils.ShrinkAndSaveImage(file.LocalFileName, cacheImageName, 320, 240);
                            }

                            //======================================================================
                            // copy files directly to PicData

                            File.Move(file.LocalFileName, targetImagePath);

                            ret.Add(displayOrder, sourceHash);

                            txnScope.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!loggedFileErrorOnce)
                        {
                            LogUtil.WriteLogEntry("InspectionWriterWebApi", "Exception during image processing upload: " + ex, EventLogEntryType.Error);
                            loggedFileErrorOnce = true;
                            try
                            {
                                IwLogUtil.SendExceptionNotification(ex, "Exception during image processing upload: ");
                            }
                            catch (Exception exInner)
                            {
                                LogUtil.WriteLogEntry("InspectionWriterWebApi", "Error during SendExceptionNotification: " + ex + Environment.NewLine + exInner, EventLogEntryType.Error);
                                throw;
                            }
                        }

                        // TODO: Make this more robust
                        if (ret.ContainsKey(displayOrder))
                        {
                            ret.Remove(displayOrder);
                        }
                    }
                }

                // return the inspection object created above
                return Ok(new {hashes = ret, inspection = await InspectionsController.FindInspections(new InspectionSearchRequest{InspectionId = inspectionId}, true)});
            }
            catch (Exception ex)
            {
                LogUtil.WriteLogEntry("InspectionWriterWebApi", "Exception during image upload " + ex, EventLogEntryType.Error);
                IwLogUtil.SendExceptionNotification(ex, "Exception during image upload: ");
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("startImageMove")]
        public async Task StartImageMove()
        {
            ////Start the background process to move the files from temp storage to picdata
            //HostingEnvironment.QueueBackgroundWorkItem(async cancellationToken =>
            //{
            //    var result = await FileUtils.MoveFiles();

            //    //Create a log
            //    LogUtil.WriteLogEntry("InspectionWriterWebApi", "Successfully moved " + result.ToString() + " images ", EventLogEntryType.Information);
            //});
        }

        [HttpPost]
        [Route("inspectionImage/{inspectionImageId}")]
        public async Task<IHttpActionResult> ReplaceInspectionImage(Guid inspectionImageId)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return InternalServerError(new HttpResponseException(HttpStatusCode.UnsupportedMediaType));
            }

            var root = HttpContext.Current.Server.MapPath("~/App_Data");

            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                var imageExists = await DoesInspectionImageExist(inspectionImageId);

                if (!imageExists)
                {
                    return BadRequest("No inspection image with ID " + inspectionImageId + " found to replace.");
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLogEntry("InspectionWriterWebApi", "Exception during replace inspection image: " + ex, EventLogEntryType.Error);
                IwLogUtil.SendExceptionNotification(ex, "Exception during replace inspection image: ");
                throw;
            }

            return Ok();
        }

        private static async Task<bool> DoesInspectionImageExist(Guid inspectionImageId)
        {
            var sql = "SELECT CASE WHEN EXISTS ( " +
                      "  SELECT * FROM InspectionImage WITH (NOLOCK) WHERE InspectionImageID = @InspectionImageID" +
                      ") THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END";

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@InspectionImageID", inspectionImageId);

                    await cn.OpenAsync();

                    var result = await cmd.ExecuteScalarAsync();

                    return (bool)result;
                }
            }
        }

        private static async Task CreatePictureLocationRecord(Guid pictureId)
        {
            var sql = "INSERT INTO PictureLocation (PictureID, LocationID) " +
                      "VALUES (@PictureID, @LocationID)";

            var @params = new[]
                    {
                        new SqlParameter("@PictureID", pictureId),
                        new SqlParameter("@LocationID", WebConfigurationManager.AppSettings["ServerLocationId"].ParseTo<Guid>())
                    };

            using (var txnScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddRange(@params);

                        await cn.OpenAsync();

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                txnScope.Complete();
            }
        }

        private static async Task CreateInspectionPictureRecord(Guid pictureId, Guid inspectionId)
        {
            var sql = "INSERT INTO InspectionPicture (PictureID, InspectionID) " +
                      "VALUES (@PictureID, @InspectionID)";

            var @params = new[]
                    {
                        new SqlParameter("@PictureID", pictureId),
                        new SqlParameter("@InspectionID", inspectionId),
                    };

            using (var txnScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddRange(@params);

                        await cn.OpenAsync();

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                txnScope.Complete();
            }
        }

        private string GetCacheImageFileBaseName(string pictureId, int width = 0, int height = 0, string imageType = ".JPG")
        {
            if (!imageType.StartsWith(".")) { imageType = "." + imageType; }
            return "I" + pictureId + "_" + width.ToString() + "_" + height.ToString() + imageType;
        }

        private static async Task<string> GetImageSavePath(Guid locationId)
        {
            // we are no longer going to stage images in "tempImages". they are going directly to picdata
            //var basePath = WebConfigurationManager.AppSettings["ImageFileBasePath"];
            var basePath = WebConfigurationManager.AppSettings["BaseImagePath"];
            var nextFilename = await GetNextImageFilenameAsync(locationId, true);
            var imageFilename = string.Format("{0}.jpg", nextFilename);

            return Path.Combine(basePath, imageFilename);
        }

        public static async Task<string> GetNextImageFilenameAsync(Guid locationId, bool isRemote)
        {
            var sql = "DECLARE @TempCounterTable table (Prefix varchar(10), Value varchar(30), MaxValue varchar(15)); " +
                      "UPDATE AMSCounter " +
                      "SET Value = Value + 1 " +
                      "OUTPUT inserted.Prefix, CAST(inserted.Value AS varchar(20)), CAST(inserted.MaxValue AS varchar(15)) INTO @TempCounterTable " +
                      "WHERE AMSCounter = @CounterName " +
                      "AND LocationID = @LocationID; " +
                      "SELECT * FROM @TempCounterTable;";

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@CounterName", isRemote ? "RemoteImageFilename" : "ImageFilename");
                    cmd.Parameters.AddWithValue("@LocationID", locationId);

                    await cn.OpenAsync();

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            var prefix = dr["Prefix"].ToString();
                            var value = dr["Value"].ToString();
                            var maxWidth = dr["MaxValue"].ToString().Length;

                            return prefix + value.PadLeft(maxWidth, '0');
                        }
                    }
                }
            }

            return null;
        }

        private static async Task<IList<Guid>> GetExistingPictureIds(Guid inspectionId)
        {
            var sql = "SELECT PictureID " +
                      "FROM InspectionPicture WITH (NOLOCK) " +
                      "WHERE InspectionID = @InspectionID";

            var ret = new List<Guid>();

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@InspectionID", inspectionId);

                    await cn.OpenAsync();

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            ret.Add((Guid)dr["PictureID"]);
                        }
                    }
                }
            }

            return ret;
        }
    }
}