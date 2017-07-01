using NPAInspectionWriter.Logging;

namespace NPAInspectionWriter.iOS.IoC
{
    public class UdpLoggerConfiguration : IUdpLoggerConfiguration
    {
        public string HostOrIp { get { return PlatformSettings.Current.HostNameOrIp; } }

        public int Port { get { return PlatformSettings.Current.Port; } }
    }
}
