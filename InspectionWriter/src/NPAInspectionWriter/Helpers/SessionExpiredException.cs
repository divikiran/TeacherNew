using NPAInspectionWriter.AppData;
using System;

namespace NPAInspectionWriter.Helpers
{
    public class SessionExpiredException : Exception
    {
        public SessionExpiredException() : base( AppMessages.ExpiredSessionMessage )
        {
        }
    }
}
