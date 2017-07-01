using System;
using System.IO;

namespace NPAInspectionWriter.Services
{
    /// <summary>
    /// Interface IFileManager provides access to files located in Isolated Storage.
    /// </summary>
    public interface IFileManager
    {
        /// <summary>
        /// Directories the exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DirectoryExists( string path );

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        void CreateDirectory( string path );

        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="access">The access.</param>
        /// <returns>Stream.</returns>
        Stream OpenFile( string path, FileMode mode, FileAccess access );

        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="access">The access.</param>
        /// <param name="share">The share.</param>
        /// <returns>Stream.</returns>
        Stream OpenFile( string path, FileMode mode, FileAccess access, FileShare share );

        /// <summary>
        /// Saves a file from a given stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="appRelativePath"></param>
        /// <param name="overwriteIfExists"></param>
        void SaveFile( Stream stream, string appRelativePath, bool overwriteIfExists = true );

        /// <summary>
        /// Checks if file exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if file exists, <c>false</c> otherwise.</returns>
        bool FileExists( string path );

        /// <summary>
        /// Gets the last write time.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>DateTimeOffset.</returns>
        DateTimeOffset GetLastWriteTime( string path );

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        void DeleteFile( string path );

        /// <summary>
        /// Deletes a directory.
        /// </summary>
        /// <param name="path">Path to the directory.</param>
        void DeleteDirectory( string path );
    }
}
