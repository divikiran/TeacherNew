using NPAInspectionWriter.Models;
using System.Threading.Tasks;

namespace NPAInspectionWriter.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync( Message message );
    }
}
