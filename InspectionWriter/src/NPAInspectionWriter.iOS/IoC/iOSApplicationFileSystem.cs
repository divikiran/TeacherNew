﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.iOS.Extensions;
using Xamarin.Forms;
using NPAInspectionWriter.iOS.IOC;
using NPAInspectionWriter.Logging;

[assembly: Dependency( typeof( iOSApplicationFileSystem ) )]
namespace NPAInspectionWriter.iOS.IOC
{
    public class iOSApplicationFileSystem : IApplicationFileSystem
    {
        public iOSApplicationFileSystem()
        {
            DocumentsDirectory = Environment.GetFolderPath( Environment.SpecialFolder.Personal );
            ApplicationRoot = Path.GetDirectoryName( DocumentsDirectory );
            DatabaseDirectory = Path.Combine( ApplicationRoot, "Library", "Databases" );
        }

        public string ApplicationRoot { get; }

        public string DatabaseDirectory { get; }

        public string DocumentsDirectory { get; }

        public void DeleteDirectory( string path )
        {
            var sanitizedPath = Path.Combine( ApplicationRoot, path );
            if( Directory.Exists( sanitizedPath ) )
                Directory.Delete( Path.Combine( ApplicationRoot, path ), true );

            StaticLogger.DebugLogger( $"Unable to delete directory for path: {path}" );
        }

        public byte[] GetFileData( string path )
        {
            try
            {
                return File.ReadAllBytes( Path.Combine( ApplicationRoot, path ) ); //File.ReadAllBytes( GetValidFilePath( path ) );
            }
            catch( Exception e )
            {
                StaticLogger.WarningLogger( $"Unable to retrieve File Data for '{path}'" );
                StaticLogger.DebugLogger( e );
                return null;
            }
        }

        public Stream GetFileStream(string path)
        {
            try
            {
                return File.OpenRead( Path.Combine( ApplicationRoot, path ) ); //File.OpenRead( GetValidFilePath( path ) );
            }
            catch( Exception e )
            {
                StaticLogger.WarningLogger( $"Unable to retrieve File Stream for '{path}'" );
                StaticLogger.DebugLogger( e );
                return null;
            }
        }

        public string GetValidFilePath( string path )
        {
            var sanitized = path.EnsureProperApplicationRoot();

            sanitized = Regex.Replace( sanitized, @"^\/", "" );

            //if( !sanitized.StartsWith( "file://" ) )
            //    sanitized = $"file://{sanitized}";

            return sanitized;
        }

    }
}
