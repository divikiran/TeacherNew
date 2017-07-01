using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using static System.Net.WebUtility;

namespace NPAInspectionWriter.Extensions
{
    public static class StringExtensions
    {
        const string validStockNumberPattern = @"\d{8}";

        public static string AddQueryStringParameters(this string uri, object queryObject)
        {
            if( queryObject == null ) return uri;

            Regex.Replace(uri, @"\/$", "");

            var prop = queryObject.GetType().GetRuntimeProperties()
                       // Only check Properties with the JsonPropertyAttribute
                       .Where(p => p.GetCustomAttributes().Any(attr => attr.GetType() == typeof(JsonPropertyAttribute)))
                       // Only get Properties that aren't Null. And IF they are a string make sure it isn't an empty string or white space.
                       .Where(p => p.GetValue(queryObject, null) != null || (p.GetType() == typeof(string) && !string.IsNullOrWhiteSpace(p.GetValue(queryObject, null).ToString())))
                       .Select(p => $"{p.GetCustomAttribute<JsonPropertyAttribute>().PropertyName}={UrlEncode(p.GetValue(queryObject, null).ToString())}");

            return $"{uri}/?{string.Join("&", prop)}";
        }

        public static string VersionFormatter( this string rawVersionId ) => Regex.Replace( rawVersionId, @"[^\d\.]", "" );

        public static bool VersionIsEqualOrGreater( this string thisAppVersion, string minimumAppVersion )
        {
            var currentVersion = new Version( VersionFormatter( thisAppVersion ) );
            var minimumVersion = new Version( VersionFormatter( minimumAppVersion ) );

            if( currentVersion.CompareTo( minimumVersion ) < 0 ) return false;

            return true;
        }

        public static string GetUriHostname( this string uriString )
        {
            return new Uri( uriString ).Host;
        }

        public static int GetUriPort( this string uriString )
        {
            return new Uri( uriString ).Port;
        }

        public static char CharAt( this string str, int i )
        {
            return str.ToCharArray()[ i ];
        }

        public static string TryToString(this object o, string defaultValue = null)
        {
            try
            {
                return o.ToString();
            }
            catch
            {
                return !string.IsNullOrWhiteSpace(defaultValue) ? defaultValue : String.Empty;
            }
        }

        public static string FormatStockNumber( this string fullyQualifiedStockNumber )
        {
            if( Regex.IsMatch( fullyQualifiedStockNumber, validStockNumberPattern ) ) return fullyQualifiedStockNumber;

            var parts = fullyQualifiedStockNumber.Split( '-' );

            foreach( var part in parts )
            {
                if( Regex.IsMatch( part, validStockNumberPattern ) )
                    return part;
            }

            return null;
        }

        public static bool IsValidEmailString( this string emailString ) =>
            Regex.IsMatch( emailString, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase );
    }
}
