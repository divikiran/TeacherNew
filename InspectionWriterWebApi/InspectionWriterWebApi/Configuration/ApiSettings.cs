using System.Web.Configuration;

namespace InspectionWriterWebApi
{
    public static class ApiSettings
    {
        public static string ConnectionString
        {
            get
            {
                return WebConfigurationManager.ConnectionStrings["AMSProto"].ConnectionString;      
            }
        }

        public const int SessionLifetimeInMinutes = 600;

        public const int SessionTimeoutMinutes = 90;
    }
}