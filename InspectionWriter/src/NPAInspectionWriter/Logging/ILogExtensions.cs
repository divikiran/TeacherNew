namespace NPAInspectionWriter.Logging
{
    public static class ILogExtensions
    {
        public static void Alert( this ILog logger, object obj ) => logger.Alert( $"{obj}" );

        public static void Debug( this ILog logger, object obj ) => logger.Debug( $"{obj}" );

        public static void Info( this ILog logger, object obj ) => logger.Info( $"{obj}" );

        public static void Notice( this ILog logger, object obj ) => logger.Notice( $"{obj}" );

        public static void Warn( this ILog logger, object obj ) => logger.Warn( $"{obj}" );
    }
}
