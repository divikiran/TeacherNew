using System.Threading.Tasks;

namespace NPAInspectionWriter.Services
{
    public interface IUdpMessenger
    {
        Task SendMessage( string message, string hostOrIp, int port );
    }
}
