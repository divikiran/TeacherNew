using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using InspectionWriterWebApi.Models;

namespace InspectionWriterWebApi.Controllers
{
    [RoutePrefix("devices")]
    public class DeviceRegistrationController : ApiController
    {
        [Route("register")]
        [HttpPost]   
        public async Task<IHttpActionResult> RegisterDevice([FromBody]DeviceRegistration registration)
        {
            try
            {
                var sql = "MERGE DeviceRegistration AS target " +
                          "USING (SELECT @DeviceToken, @UserAccountID) AS source (DeviceToken, UserAccountID) " +
                          "ON (target.UserAccountID = source.UserAccountID) " +
                          "WHEN MATCHED THEN UPDATE " +
                          "    SET target.DeviceToken = source.DeviceToken, target.DateModified = GETDATE() " +
                          "WHEN NOT MATCHED THEN " +
                          "    INSERT (DeviceToken, UserAccountID) " +
                          "    VALUES (source.DeviceToken, source.UserAccountID);";

                using (var cn = new SqlConnection(ApiSettings.ConnectionString))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@DeviceToken", registration.DeviceToken);
                        cmd.Parameters.AddWithValue("@UserAccountID", registration.UserAccountId);

                        await cn.OpenAsync();

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
