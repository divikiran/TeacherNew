using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using InspectionWriterWebApi.Models;
using InspectionWriterWebApi.Utilities;
using NPA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using InspectionWriterWebApi.Extensions;

namespace InspectionWriterWebApi.Controllers
{
    [Authorize]
    [RoutePrefix("print")]
    public class PrintController : ApiController
    {

        [HttpGet]
        [Route("{locationId}/printersAtLocation")]
        public async Task<IHttpActionResult> GetPrinters(Guid locationId)
        {
            var sql = "SELECT PrinterID, PrinterName AS Printer FROM dbo.fn_GetLabelPrintersByLocation(@LocationID)";

            var ret = new List<Printer>();

            var @params = new List<SqlParameter>() { new SqlParameter("@LocationID", locationId) };

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddRange(@params.ToArray());

                    await cn.OpenAsync();

                    try
                    {
                        using (var dr = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult))
                        {
                            while (await dr.ReadAsync())
                            {
                                ret.Add(new Printer
                                {
                                    PrinterId = (Guid)dr["PrinterID"],
                                    PrinterName = dr["Printer"].ToString()
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == -2)
                        {
                            return
                                InternalServerError(
                                    new Exception(
                                        "Unable to get the list of printers for this location."));
                        }
                    }
                    catch (Exception ex2)
                    {
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

        [Route("printLabel")]
        [HttpPost]
        public async Task<IHttpActionResult> PrintLabel([FromBody]ReportRequest request)
        {
            try
            {
                string amsBaseUrl = "";
                string paperOrientation = string.Empty;
                var sql = "SELECT LocalURL FROM Location WHERE LocationID = @LocationID";

                if (!string.IsNullOrEmpty(request.LocationId.TryToString()))
                {
                    using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                    {
                        using (var cmd = new SqlCommand(sql, cn))
                        {
                            cmd.Parameters.AddWithValue("@LocationID", request.LocationId);

                            await cn.OpenAsync();

                            amsBaseUrl = await cmd.ExecuteScalarAsync() as string;
                        }
                    }
                }

                if (string.IsNullOrEmpty(amsBaseUrl))
                {
                    throw new Exception("ERROR printing label: Could not find URL for location (" + request.LocationId.ToString() + ")");
                }

                string reportFile = request.ReportFile.GetReportFileName();
                if (string.IsNullOrEmpty(reportFile))
                {
                    throw new Exception("ERROR printing label: Report not supported (" + request.ReportFile + ")");
                }

                // Build the url to the AMS label printing page with url params.
                string amsPrintUrl = string.Format("{0}/ipad-PrintReport.aspx?VehicleID={1}&PrinterID={2}&ReportFile={3}", amsBaseUrl, request.VehicleId, request.PrinterId, reportFile);

                // Call the AMS page to print the label report, this page returns only text (text/plain), either "OK" or some error text.
                WebClient client = new WebClient();
                string responseString = client.DownloadString(amsPrintUrl);
                if (responseString != "OK")
                {
                    throw new Exception("ERROR printing label: " + responseString 
                        + Environment.NewLine + string.Format("AMSUrl:{0}, PrinterId:{1}, ReportFile:{2}", amsBaseUrl, request.PrinterId, reportFile) );
                }

                return Ok();

            }
            catch (Exception ex)
            {
                IwLogUtil.SendExceptionNotification(ex);
                return InternalServerError(ex);
            }
        }



        // **********************************************************************
        // Everything below is for the non-AMS version of "printlabel"
        // **********************************************************************

        #region "PrintLabel2"

        [Route("printLabel2")]
        [HttpPost]
        public async Task<IHttpActionResult> PrintLabel2([FromBody]ReportRequest request)
        {
            try
            {
                string localURL = "";
                string paperOrientation = string.Empty;
                var sql = "SELECT LocalURL FROM Location WHERE LocationID = @LocationID";

                if (!string.IsNullOrEmpty(request.LocationId.TryToString()))
                {
                    using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                    {
                        using (var cmd = new SqlCommand(sql, cn))
                        {
                            cmd.Parameters.AddWithValue("@LocationID", request.LocationId);

                            await cn.OpenAsync();

                            localURL = await cmd.ExecuteScalarAsync() as string;
                        }
                    }
                }

                sql = string.Format("SELECT TOP 1 Orientation FROM dbo.Report WITH (NOLOCK) WHERE ReportFile = '{0}'", request.ReportFile);
                using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        await cn.OpenAsync();

                        paperOrientation = (await cmd.ExecuteScalarAsync() as string).TryToString("2");
                    }
                }

                sql = "";

                switch (request.ReportFile)
                {
                    case ReportFile.AuctionLabel:
                        sql = string.Format("{{AuctionItemDetailView.VehicleID}} = '{0}'", request.VehicleId);
                        break;
                    case ReportFile.CompressionTestLabel:
                        sql = string.Format("{{VehicleDetailView.VehicleID}} = '{0}'", request.VehicleId);
                        break;
                    case ReportFile.TransferLabel:
                    case ReportFile.VehicleTransferLabelWithComp:
                        string isFactory = "0";
                        sql = string.Format("SELECT dbo.fn_IsFactory('{0}')", request.VehicleId);
                        using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                        {
                            using (var cmd = new SqlCommand(sql, cn))
                            {
                                await cn.OpenAsync();
                                isFactory = cmd.ExecuteScalar().TryToString();
                            }
                        }
                        if (isFactory == "1")
                        {
                            sql = string.Format("{{VehicleTransferDetailView.VehicleID}} = '{0}'", request.VehicleId);
                        }
                        else
                        {
                            sql = string.Format("{{VehicleTransferDetailView.VehicleID}} = '{0}' AND {{VehicleTransferDetailView.PickupLinkID}} = '{1}'",
                            request.VehicleId, request.LocationId);
                        }

                        break;
                    case ReportFile.VehicleLabel:
                        sql = string.Format("{{VehicleDetailView.VehicleID}} = '{0}'", request.VehicleId);
                        break;
                    case ReportFile.VehicleLabelWithComp:
                        sql = string.Format("{{VehicleDetailView.VehicleID}} = '{0}'", request.VehicleId);
                        break;
                }

                string sFilePath = localURL + "/reports/" + request.ReportFile.GetReportFileName();
                string sReportsPath = AppDomain.CurrentDomain.BaseDirectory + "reports\\"; // + request.ReportFile;

                if (!System.IO.Directory.Exists(sReportsPath)) { System.IO.Directory.CreateDirectory(sReportsPath); }
                sReportsPath += request.ReportFile;

                //Copy local if it doesn't exist already. Might want to overrite the file if it's old once this is working.
                if (!System.IO.File.Exists(sReportsPath))
                {
                    // Create an instance of WebClient
                    var client = new System.Net.WebClient();

                    // Start the download and copy the file to the local reports folder
                    client.DownloadFile(new Uri(sFilePath), sReportsPath);
                }
                else
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(sReportsPath);
                    if (DateTime.Compare(DateTime.Now.AddDays(-1), fi.CreationTime) > 0)
                    {
                        System.IO.File.Delete(sReportsPath);
                        // Create an instance of WebClient
                        var client = new System.Net.WebClient();

                        // Start the download and copy the file to the local reports folder
                        client.DownloadFile(new Uri(sFilePath), sReportsPath);
                    }
                }

                // might change to extract the following "Setting"s: "CRDataSource", "CRDatabaseName", "CRUserPassword", "CRUserPassword"
                string dbSource = "AMSProto", dbName = "AMSProto", dbUser = "sa", dbPass = "1WWV20B";

                var crRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                crRpt.Load(sReportsPath, OpenReportMethod.OpenReportByTempCopy);  // .LogOnServerEx("crdb_ado.dll", dbSource, dbName, "", "", null, ApiSettings.ConnectionString);
                crRpt.SetDatabaseLogon(dbUser, dbPass, dbSource, dbName);

                crRpt.RecordSelectionFormula = sql;
                crRpt.Refresh();

                string printerName = null;
                if (request.PrinterId.HasValue)
                {
                    printerName = GetPrinterName(request.PrinterId.Value);
                }

                if (string.IsNullOrEmpty(printerName))
                {
                    printerName = GetPrinterName(request.ReportFile.GetReportFileName(), request.LocationId);
                }

                crRpt.PrintOptions.DissociatePageSizeAndPrinterPaperSize = true;
                crRpt.PrintOptions.PaperSize = PaperSize.PaperLetter;
                crRpt.PrintOptions.PaperOrientation = (NPA.Common.TypeUtil.GetInt32(paperOrientation, 1) == 2 ? PaperOrientation.Portrait : PaperOrientation.Landscape);

                // ** Export report. Uncomment to enable. **
                string sExportPath = AppDomain.CurrentDomain.BaseDirectory + "export\\";
                if (!System.IO.Directory.Exists(sExportPath)) { System.IO.Directory.CreateDirectory(sExportPath); }

                string sExportFileName = Guid.NewGuid().TryToString();
                if (ExportReport(crRpt, sExportPath, sExportFileName))
                {
                    //return Ok();
                    if (!string.IsNullOrEmpty(printerName))
                    {
                        // ** Print report **

                        if (PrintPDFs(sExportPath + sExportFileName + ".pdf", printerName))
                        {
                            return Ok();
                        }

                    }
                    else
                    {
                        throw new Exception("Unable to find printer");
                    }
                }
                else
                {
                    return InternalServerError();
                }

                //return Ok();
            }
            catch (Exception ex)
            {
                IwLogUtil.SendExceptionNotification(ex);
                return InternalServerError(ex);
            }
            return InternalServerError();
        }

        private string GetPrinterName(string reportName, Guid locationId)
        {
            var sql = @"SELECT TOP 1 PrinterName FROM Report r WITH (NOLOCK)
                        INNER JOIN ReportPrinterLink rpl WITH (NOLOCK) ON r.ReportID = rpl.ReportID
                        INNER JOIN Printer p WITH (NOLOCK) ON rpl.PrinterID = p.PrinterID
                        WHERE ReportFile = @Report AND p.LocationID = @LocationID AND ISNULL(p.Hide,0) = 0 ".TrimExtraWhiteSpace();
            var @params = new List<SqlParameter>() { new SqlParameter("@Report", reportName) };
            @params.Add(new SqlParameter("@LocationID", locationId));
            string printerName = string.Empty;

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddRange(@params.ToArray());
                    cn.Open();

                    try
                    {
                        printerName = cmd.ExecuteScalar().TryToString();
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }

            return printerName;
        }

        private string GetPrinterName(Guid printerId)
        {
            var sql = "SELECT PrinterName FROM Printer WHERE PrinterID = @PrinterID";
            var @params = new List<SqlParameter>() { new SqlParameter("@PrinterID", printerId) };
            string printerName = string.Empty;

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddRange(@params.ToArray());
                    cn.Open();

                    try
                    {
                        printerName = cmd.ExecuteScalar().TryToString();
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }

            return printerName;
        }

        /// <summary>
        /// Export Report to pdf
        /// </summary>
        /// <param name="crRpt">CrystalDecisions.CrystalReports.Engine.ReportDocument</param>
        /// <param name="exportPath">localURL + "\export\"</param>
        /// <param name="fileName">new filename as string (Guid)</param>
        /// <returns></returns>
        private static bool ExportReport(ReportDocument crRpt, string exportPath, string fileName)
        {
            try
            {
                crRpt.ExportToDisk(ExportFormatType.PortableDocFormat, exportPath + fileName + ".pdf");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                IwLogUtil.SendExceptionNotification(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Return a stream of Bytes
        /// </summary>
        /// <param name="crRpt">CrystalDecisions.CrystalReports.Engine.ReportDocument</param>
        /// <param name="exptype">CrystalDecisions.Shared.ExportFormatType</param>
        /// <returns>Byte array</returns>
        public static byte[] ExportReportToStream(ReportDocument crRpt, CrystalDecisions.Shared.ExportFormatType exptype)
        {
            System.IO.Stream st;
            st = crRpt.ExportToStream(exptype);

            byte[] arr = new byte[st.Length];
            st.Read(arr, 0, (int)st.Length);

            return arr;
        }

        private static Boolean PrintPDFs(string pdfFileName, string printerName)
        {
            try
            {
                var proc = new System.Diagnostics.Process();
                proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                proc.StartInfo.Verb = "print";

                //Define location of adobe reader/command line switches to launch adobe in "print" mode
                string foxitReaderPath = WebConfigurationManager.AppSettings["FoxitReaderPath"];
                proc.StartInfo.FileName = foxitReaderPath;
                proc.StartInfo.Arguments = string.Format("/t \"{0}\" \"{1}\"", pdfFileName, printerName);
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;
                using (System.Security.Principal.WindowsImpersonationContext wic = System.Security.Principal.WindowsIdentity.Impersonate(IntPtr.Zero))
                {
                    proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    proc.Start();

                    if (proc.HasExited == false)
                    {
                        proc.WaitForExit(5000);
                    }
                    proc.EnableRaisingEvents = true;

                    proc.Close();
                    wic.Undo();
                }

                return true;
            }
            catch (Exception ex)
            {
                IwLogUtil.SendExceptionNotification(ex);
                return false;
            }
        }

        #endregion // PrintLabel2

    }
}