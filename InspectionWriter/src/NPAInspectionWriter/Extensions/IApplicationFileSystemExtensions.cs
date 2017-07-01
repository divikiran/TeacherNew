using NPAInspectionWriter.Helpers;

namespace NPAInspectionWriter.Extensions
{
    public static class IApplicationFileSystemExtensions
    {
        public static string GetValidFilePath( this IApplicationFileSystem app, object path ) =>
            app.GetValidFilePath( path.ToString() );
    }
}
