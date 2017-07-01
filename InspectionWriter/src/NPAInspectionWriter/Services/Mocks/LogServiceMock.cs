#if USE_MOCKS
using NPAInspectionWriter.Models;
using System.Threading.Tasks;

namespace NPAInspectionWriter.Services
{
    public class LogServiceMock : ILogService
    {
        public Task<bool> LogErrorAsync( Error err ) => new Task<bool>( () => true );
    }
}
#endif