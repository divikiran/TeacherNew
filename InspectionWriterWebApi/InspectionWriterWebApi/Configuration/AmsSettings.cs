using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace InspectionWriterWebApi.Configuration
{
    public static class AmsSettings
    {
        public static void LoadSettings()
        {
            var amsSettings = WebConfigurationManager.GetSection("amsSettings");
        }
    }
}