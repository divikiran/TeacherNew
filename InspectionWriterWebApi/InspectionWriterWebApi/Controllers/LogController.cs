using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using InspectionWriterWebApi.Models;
using NPA.Common;

namespace InspectionWriterWebApi.Controllers
{
    [Authorize]
    [RoutePrefix("log")]
    public class LogController : ApiController
    {
        [HttpPost]
        [Route("error")]
        public async Task<IHttpActionResult> LogError(Error err)
        {
            try
            {
                await Task.Run(() => LogErrorFromApp(err));

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [NonAction]
        private static void LogErrorFromApp(Error err)
        {
            string errorText = "InspectionWriter App Error: " + Environment.NewLine +
                ((string.IsNullOrEmpty(err.Description)) ? err.Message + Environment.NewLine + err.StackTrace : err.Description);
            //string toEmail = NPA.Core.ConfigurationMgr.GetSetting("DevTeam", "wsteed@npauctions.com");
            string toEmail = "wsteed@npauctions.com";

            Email.SendBasicEmail(toEmail, "InspectionWriter App Error", errorText, "info@npauctions.com", 1, 0);

            LogUtil.WriteLogEntry("InspectionWriterWebApi", "LogErrorFromApp: " + err.ToString(), System.Diagnostics.EventLogEntryType.Error);
        }

        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage prtg()
        {
            string result = string.Empty;

            var pmHelper = new InspectionWriterWebApi.Utilities.PerformanceMonitorHelper();

            // add cpu
            result += "CPU: " + Convert.ToInt32(pmHelper.GetCPUUsage()) + "%" + Environment.NewLine;

            // add ram
            result += "RAM: " + Convert.ToInt32(pmHelper.GetMemoryUsage()) + "%" + Environment.NewLine;

            // get network cards
            var networkCardNames = pmHelper.GetAllNetworkCardNames();

            // add networks
            foreach (string name in networkCardNames)
            {
                result += "NET: " + Convert.ToInt32(pmHelper.GetNetworkUsage(name)) + "%" + Environment.NewLine;
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(result, System.Text.Encoding.UTF8, "text/plain")
            };
        }
    }
}