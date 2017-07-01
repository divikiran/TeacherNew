using System;
using System.IO;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.Services;

namespace NPAInspectionWriter.Extensions
{
    /// <summary>
    /// Class MediaExtensions.
    /// </summary>
    public static class MediaExtensions
    {
        /// <summary>
        /// Verifies the options.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <exception cref="System.ArgumentNullException">options</exception>
        /// <exception cref="System.ArgumentException">options.Directory must be a relative folder;options</exception>
        public static void VerifyOptions( this MediaStorageOptions self )
        {
            if( self == null )
            {
                throw new ArgumentNullException( "self" );
            }
            //if (!Enum.IsDefined (typeof(MediaFileStoreLocation), options.Location))
            //    throw new ArgumentException ("options.Location is not a member of MediaFileStoreLocation");
            //if (options.Location == MediaFileStoreLocation.Local)
            //{
            //if (String.IsNullOrWhiteSpace (options.Directory))
            //  throw new ArgumentNullException ("options", "For local storage, options.Directory must be set");
            if( Path.IsPathRooted( self.Directory ) )
            {
                throw new ArgumentException( "options.Directory must be a relative folder", "self" );
            }
            //}
        }

        /// <summary>
        /// Gets the output file name with folder.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="rootPath">The root folder.</param>
        /// <returns>System.String.</returns>
        public static string GetMediaFileWithPath( this MediaStorageOptions self, string rootPath )
        {
            var isPhoto = !( self is VideoMediaStorageOptions );
            var name = ( self != null ) ? self.Name : null;
            var directory = ( self != null ) ? self.Directory : null;

            return MediaFileHelpers.GetMediaFileWithPath( isPhoto, rootPath, directory, name );
        }

        /// <summary>
        /// Gets the unique filepath.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="rootPath">The root folder.</param>
        /// <param name="checkExists">The check exists.</param>
        /// <returns>System.String.</returns>
        public static string GetUniqueMediaFileWithPath( this MediaStorageOptions self, string rootPath,
            Func<string, bool> checkExists )
        {
            var isPhoto = !( self is VideoMediaStorageOptions );
            var path = self.GetMediaFileWithPath( rootPath );

            var folder = Path.GetDirectoryName( path );
            var name = Path.GetFileNameWithoutExtension( path );

            return MediaFileHelpers.GetUniqueMediaFileWithPath( isPhoto, folder, name, checkExists );
        }
    }
}
