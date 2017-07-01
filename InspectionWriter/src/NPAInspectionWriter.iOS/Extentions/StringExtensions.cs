using System;
using System.IO;
using System.Text.RegularExpressions;
using Foundation;

namespace NPAInspectionWriter.iOS.Extensions
{
    public static class StringExtensions
    {
        const string applicationRootPathPattern = @"^\/var\/mobile\/Containers\/Data\/Application\/(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})\/";

        public static NSString ToNSString( this string val )
        {
            return new NSString( val );
        }

        public static bool StartsWithApplicationRoot( this string path )
        {
            return Regex.IsMatch( path, applicationRootPathPattern );
        }

        public static string EnsureProperApplicationRoot( this string path )
        {
            var appRoot = Path.GetDirectoryName( Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ) );
            if( path.StartsWithApplicationRoot() )
            {
                return Regex.Replace( path, applicationRootPathPattern, appRoot );
            }

            return Path.Combine( appRoot, path );
        }

        public static string RemoveApplicationRoot( this string path )
        {
            return Regex.Replace( path, applicationRootPathPattern, string.Empty );
        }
    }
}
