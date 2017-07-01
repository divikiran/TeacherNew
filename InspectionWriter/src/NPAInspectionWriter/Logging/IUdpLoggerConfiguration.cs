namespace NPAInspectionWriter.Logging
{
    public interface IUdpLoggerConfiguration
    {
        string HostOrIp { get; }
        int Port { get; }
    }
}
