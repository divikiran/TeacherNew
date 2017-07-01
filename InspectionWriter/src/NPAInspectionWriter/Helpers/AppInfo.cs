using Xamarin.Forms;

namespace NPAInspectionWriter.Helpers
{
    public class AppInfo
    {
        public string AppName { get; set; }
        public string AppVersion { get; set; }
        public string UserName { get; set; }
        public string Location { get; set; }
        public string LogoImageResourceLocation { get; set; } = "NPALogo.png";
        public ImageSource LogoSource { get; set; } = null;
    }
}
