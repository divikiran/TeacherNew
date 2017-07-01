namespace NPAInspectionWriter.Logging
{
    public interface ISysLogLogger
    {
        void Log( string message, Level level );
        void Log( string message, Level level, Facility facility );
    }
}
