using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NPAInspectionWriter.Extensions
{
    public static class HttpRequestHeadersExtensions
    {
        /// <summary>
        /// Adds or Updates a specified header/value
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="name">Header Key Name to Add or Update</param>
        /// <param name="value">Value to Add or Update</param>
        public static void AddOrUpdate( this HttpRequestHeaders headers, string name, string value )
        {
            if( !headers.Update( name, value ) )
            {
                headers.Add( name, value );
            }
        }

        /// <summary>
        /// Indicates whether or not the specified Header name exists
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="name">Header Key Name</param>
        /// <returns>Boolean indicating the presence of the specified name</returns>
        public static bool HasKey( this HttpRequestHeaders headers, string name )
        {
            return headers.Any( h => h.Key == name );
        }

        /// <summary>
        /// This will update the first element of the header array if it exists
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="name">Header Key Name to update</param>
        /// <param name="value">Value to update</param>
        /// <returns>Boolean indicating whether the element was updated</returns>
        public static bool Update( this HttpRequestHeaders headers, string name, string value )
        {
            if( headers.HasKey( name ) )
            {
                var header = headers.FirstOrDefault( h => h.Key == name );

                if( header.Value.Count() > 0 )
                {
                    header.Value.Insert( 0, value );
                    return true;
                }
            }

            return false;
        }
    }
}
