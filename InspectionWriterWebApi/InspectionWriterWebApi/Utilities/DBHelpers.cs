using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using NPA.Common;

namespace InspectionWriterWebApi.Utilities
{
    public class LoginQuery
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string EntityType { get; set; }
        public string IPAddress { get; set; }
        public string QueryString { get; set; }
    }

    public class DBHelpers
    {

        public static string OptionIdToDescription(Guid optionId)
        {
            string val = string.Empty;
            var sql =
                "SELECT InspectionOption " +
                "FROM InspectionOption WITH (NOLOCK) " +
                "WHERE InspectionOptionID = @InspectionOptionID";

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                cn.Open();

                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@InspectionOptionID", optionId);
                    val = cmd.ExecuteScalar().TryToString();
                }
            }
            return val;
        }

        public static string GetUserAccountID(string username)
        {
            string val = string.Empty;
            var sql =
                "SELECT UserAccountID " +
                "FROM UserAccount WITH (NOLOCK) " +
                "WHERE UserAccount = @username";

            using (var cn = new SqlConnection(ApiSettings.ConnectionString))
            {
                cn.Open();

                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    val = cmd.ExecuteScalar().TryToString();
                }
            }
            return val;
        }

        public static string GetAuthToken(LoginQuery query)
        {
            return Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(query.Username + ":" + query.Password));
        }

        public static string GetSessionToken(LoginQuery query)
        {
            return NPA.Core.SqlHelper.ExecuteScalar(ApiSettings.ConnectionString, CommandType.Text, 
                        string.Format(";EXEC spCreateSessionToken '{0}', '{1}', '{2}', '{3}', '{4}'", 
                        query.Username, query.Password, query.EntityType, query.IPAddress, string.Empty)).TryToString();
        } 

    }
}