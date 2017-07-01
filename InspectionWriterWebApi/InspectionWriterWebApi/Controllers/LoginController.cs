using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using InspectionWriterWebApi.Models;
using NPA.CodeGen;
using NPA.Common;
using Location = InspectionWriterWebApi.Models.Location;

namespace InspectionWriterWebApi.Controllers
{
    [Authorize]
    public class LoginController : ApiController
    {
        [Route("login")]
        public async Task<IHttpActionResult> Get([FromUri]LoginRequest loginRequest)
        {
            var user = HttpContext.Current.User;

            var sql =
                "SELECT st.Value AS CurrentAppVersion, st2.Value AS CurrentAppVersionRequiredDate, ua.UserAccountID, ua.UserAccount, ua.Password, ua.UserLevelID, " +
                "l.LocationID, l.Location, c.Email, CAST(ISNULL(ua.CanCreateCR, 0) AS bit) AS CanCreateCR " +
                "FROM UserAccount ua WITH (NOLOCK) " +
                "JOIN Location l WITH (NOLOCK) ON ua.LocationID = l.LocationID " +
                "LEFT OUTER JOIN Contact c WITH (NOLOCK) ON c.ContactID = ua.ContactID " +
                "LEFT OUTER JOIN Setting st WITH (NOLOCK) ON st.Setting = 'CrWriterVersion'" +
                "LEFT OUTER JOIN Setting st2 WITH (NOLOCK) ON st2.Setting = 'CRWriterVersionRequiredDate'" +
                "WHERE ua.UserAccount = @Username";

            var @params = new[]
                {
                    new SqlParameter("@Username", user.Identity.Name),
                };

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddRange(@params);

                    await cn.OpenAsync();

                    using (
                        var dr = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult | CommandBehavior.SingleRow))
                    {
                        if (await dr.ReadAsync())
                        {
                            //var loginReq = new InspectionWriterWebApi.Utilities.LoginQuery() {
                            //    Username = dr["UserAccount"].ToString(),
                            //    Password = dr["Password"].ToString(),
                            //    EntityType = "amsuser",
                            //    IPAddress = HttpContext.Current.Request.UserHostAddress
                            //};

                            var ret = new AmsUser
                            {
                                UserAccountId = (Guid)dr["UserAccountID"],
                                UserName = dr["UserAccount"].ToString(),
                                AmsUserLevel = UserLevel.FromGuid((Guid)dr["UserLevelID"]),
                                LocationEx = new Location
                                {
                                    LocationId = (Guid)dr["LocationID"],
                                    Name = dr["Location"].ToString(),
                                },
                                EmailAddress = dr["Email"].ToString(),
                                CurrentAppVersion = dr["CurrentAppVersion"].ToString(),
                                CurrentAppVersionRequiredDate = TypeUtil.GetSafeDate(dr["CurrentAppVersionRequiredDate"].ToString(), DateTime.Today.AddYears(10)),
                                CanCreateInspections = (bool)dr["CanCreateCR"],
                                //AuthToken = user.Identity.,
                                //SessionToken = InspectionWriterWebApi.Utilities.DBHelpers.GetSessionToken(loginReq)
                            };

                            var ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                            NPA.Common.EntityTracker.LogEvent(ret.CanCreateInspections ? EntityTrackingType.CrWriterLogin : EntityTrackingType.AMSLogin, LinkType.AMSUser,
                                ret.UserAccountId, LinkType.AMSUser, ret.UserAccountId, ip);

                            return Ok(ret);
                        }
                    }
                }
            }

            return Unauthorized();
        }
    }
}