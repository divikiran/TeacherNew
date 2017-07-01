using NPAInspectionWriter.Models;
using System.Threading.Tasks;

namespace NPAInspectionWriter.Services
{
    public interface ILogService
    {
        Task<bool> LogErrorAsync( Error err );
    }
}
