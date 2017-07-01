using System;
using Newtonsoft.Json.Linq;
using NPAInspectionWriter.AppData;

namespace NPAInspectionWriter.Helpers
{
    public class InspectionWriterApiException : Exception
    {
        public InspectionWriterApiException( string message = "" ) : base ( ParseMessage( message ) ) { }

        private static string ParseMessage( string message )
        {
            try
            {
                var jObject = JObject.Parse( message );
                if ( !string.IsNullOrWhiteSpace( jObject?[ "message" ].ToString() ) ) return jObject?[ "message" ].ToString();
                throw new Exception();
            }
            catch
            {
                return AppMessages.UnknownErrorMessage;
            }
        }
    }
}
