using System.Threading.Tasks;

namespace NPAInspectionWriter.Services
{
    public interface IAAAService
    {
        Task<bool> LoginAsync( string userName, string password );

        Task<bool> LogoutAsync();
    }
}
