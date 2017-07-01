using System.IO;

namespace NPAInspectionWriter.Helpers
{
    public interface IApplicationFileSystem
    {
        string ApplicationRoot { get; }

        string DatabaseDirectory { get; }

        string DocumentsDirectory { get; }

        string GetValidFilePath( string path );

        Stream GetFileStream( string path );

        byte[] GetFileData( string path );

        void DeleteDirectory( string path );
    }
}
