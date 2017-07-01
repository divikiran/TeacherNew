using System;
using System.Web.Configuration;
using NPA.Common;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Web;
using System.Net.Http;

namespace InspectionWriterWebApi.Utilities
{
    public static class IwLogUtil
    {
        public static void SendExceptionNotification(Exception ex, string contextMessage = null)
        {
            LogUtil.WriteLogEntry("InspectionWriterWebApi", "Exception: " + ex, EventLogEntryType.Error);

            string machineName = string.Empty;
            try
            {
                machineName = Environment.MachineName;
            }
            catch
            {
                machineName = "Error trying to read Machine Name";
            }

            string hostName = string.Empty;
            try
            {
                // this should be web api 2.x safe and self host safe
                var httpRequestMessage = HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                var httpContext = httpRequestMessage.Properties["MS_HttpContext"] as HttpContextBase;

                // read from the url
                if (httpContext.Request.Url != null) { hostName = httpContext.Request.Url.Host; }

                // read from the server variables if the url is null
                if (string.IsNullOrEmpty(hostName)) { hostName = httpContext.Request.ServerVariables["SERVER_NAME"]; }
                if (string.IsNullOrEmpty(hostName)) { hostName = httpContext.Request.ServerVariables["HTTP_HOST"]; }
            }
            catch
            {
                hostName = "Error trying to read Host Name";
            }

            try
            {
                string errorText =
                    "InspectionWriterWebApi Error: " + Environment.NewLine +
                    "Machine Name: " + machineName + Environment.NewLine +
                    "Host Name: " + hostName + Environment.NewLine +
                    ((!string.IsNullOrEmpty(contextMessage)) ? contextMessage + Environment.NewLine : "") + ex.ToString();

                //string toEmail = NPA.Core.ConfigurationMgr.GetSetting("DevTeam", "wsteed@npauctions.com");
                string toEmail = "wsteed@npauctions.com";

                Email.SendBasicEmail(toEmail, "InspectionWriterWebApi Error", errorText, "info@npauctions.com", 1, 0);
            }
            catch (Exception ex1)
            {
                LogUtil.WriteLogEntry("InspectionWriterWebApi", "Error in SendExceptionNotification: " + ex1, EventLogEntryType.Error);
            }
        }

        public static void LogContext(string appVersion, string iOSVersion, string iPadModel, string username, string location, Guid entityLinkID, int entityLinkTypeID, Guid entityTrackingTypeID)
        {
            try
            {
                string logText = string.Format(@"<context><appVersion>{0}</appVersion><iOSVersion>{1}</iOSVersion><iPadModel>{2}</iPadModel><username>{3}</username><location>{4}</location></context>",
                    appVersion, iOSVersion, iPadModel, username, location);
                Guid entityTrackingID = Guid.NewGuid();
                var sql =
                    "INSERT INTO EntityTracking (EntityTrackingID,LinkID,LinkTypeID,EntityLinkID,EntityLinkTypeID,EntityTrackingTypeID,DatePosted" +
                    ") VALUES (" +
                    "   @EntityTrackingID, @EntityLinkID, @EntityLinkTypeID, @EntityLinkID, @EntityLinkTypeID, @EntityTrackingTypeID, GETDATE()" +
                    ")";

                using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                {
                    int newRowCount = 0;

                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cn.Open();

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddRange(new[]
                                    {
                                        new SqlParameter("@EntityTrackingID", entityTrackingID),
                                        new SqlParameter("@EntityLinkID", entityLinkID),
                                        new SqlParameter("@EntityLinkTypeID", entityLinkTypeID),
                                        new SqlParameter("@EntityTrackingTypeID", entityTrackingTypeID),
                                    });

                        newRowCount = cmd.ExecuteNonQuery();
                    }

                    if (newRowCount > 0)
                    {
                        sql = "INSERT INTO EntityTrackingInfo (EntityTrackingID,EntityTrackingInfo" +
                            ") VALUES (" +
                            "   @EntityTrackingID, @EntityTrackingInfo" +
                            ")";

                        using (var cmd = new SqlCommand(sql, cn))
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(new[]
                                    {
                                        new SqlParameter("@EntityTrackingID", entityTrackingID),
                                        new SqlParameter("@EntityTrackingInfo", logText),
                                    });

                            newRowCount = cmd.ExecuteNonQuery();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLogEntry("InspectionWriterWebApi", "Error in LogContext: " + ex, EventLogEntryType.Error);
            }
        }
    }
}