using System;

namespace NPAInspectionWriter.Logging
{
    public static class StaticLogger
    {
        public static Action<object> AlertLogger= (e) => System.Diagnostics.Debug.WriteLine("Alert logged: " + e.ToString());
        public static Action<Exception> ExceptionLogger = ( e ) => System.Diagnostics.Debug.WriteLine("Exception Logged: " + e);
        public static Action<object> DebugLogger= (e) => System.Diagnostics.Debug.WriteLine("Debug logged: " + e.ToString());
        public static Action<object> InfoLogger= (e) => System.Diagnostics.Debug.WriteLine("Info logged: " + e.ToString());
        public static Action<object> WarningLogger= (e) => System.Diagnostics.Debug.WriteLine("Warning logged: " + e.ToString());
        public static Action<object> NoticeLogger= (e) => System.Diagnostics.Debug.WriteLine("Notice logged: " + e.ToString());

        public static void Log( bool condition, object message, Level level = Level.Debug )
        {
            if( condition )
            {
                Log( message, level );
            }
        }

        public static void Log( object message, Level level = Level.Debug )
        {
            switch( level )
            {
                case Level.Alert:
                    AlertLogger?.Invoke( message );
                    break;
                case Level.Error:
                    if( message.GetType() == typeof( Exception ) )
                    {
                        ExceptionLogger?.Invoke( message as Exception );
                    }
                    break;
                case Level.Information:
                    InfoLogger?.Invoke( message );
                    break;
                case Level.Warning:
                    WarningLogger?.Invoke( message );
                    break;
                case Level.Notice:
                    NoticeLogger?.Invoke( message );
                    break;
                default:
                    DebugLogger?.Invoke( message );
                    break;
            }
        }
    }
}
