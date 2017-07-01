using UIKit;

namespace NPAInspectionWriter.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main( string[] args )
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            try
            {
                ObjCRuntime.Class.ThrowOnInitFailure = false;
                UIApplication.Main( args, null, "AppDelegate" );
            }
            catch( System.Exception e )
            {
                NPAInspectionWriter.Logging.StaticLogger.ExceptionLogger( e );
                throw;
            }
        }
    }
}
