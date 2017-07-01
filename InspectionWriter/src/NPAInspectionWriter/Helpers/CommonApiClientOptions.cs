using System;
using NPAInspectionWriter.Helpers;
using NPA.XF.Http;
using NPAInspectionWriter.Logging;

namespace NPAInspectionWriter.Helpers
{
    public class CommonApiClientOptions : ICommonApiClientOptions
    {
        public CommonApiClientOptions( ILog logger )
        {
            Logger = logger;
        }

        public ILog Logger { get; }

        public RuntimeEnvironment GetRuntimeEnvironment() =>
            Settings.Current.Environment;

        public string GetToken() =>
            Settings.Current.AuthToken;
    }
}
