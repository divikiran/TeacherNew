using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.Logging;

namespace NPA.XF.Http
{
    public interface ICommonApiClientOptions
    {
        RuntimeEnvironment GetRuntimeEnvironment();

        string GetToken();

        ILog Logger { get; }
    }
}
