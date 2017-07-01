using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace NPAInspectionWriter.iOS.Extensions
{
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Returns the assembly version of the Application as specified by the fieldCount.
        /// </summary>
        /// <param name="app">The Application.</param>
        /// <param name="fieldCount">Version fields to return as 1.0.0</param>
        /// <returns></returns>
        public static string GetAppVersion<T>( this T app, int fieldCount = 3 )
            where T : Application =>
                new AssemblyName( typeof( T ).GetTypeInfo().Assembly.FullName ).Version.ToString( fieldCount );


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TApplication"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="app"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static async Task<T> LoadJsonObjectFromResourceAsync<TApplication, T>( this TApplication app, string resourceName )
            where TApplication : Application
        {
            using( var reader = new StreamReader( typeof( TApplication ).GetTypeInfo().Assembly.GetManifestResourceStream( resourceName ) ) )
            {
                return JsonConvert.DeserializeObject<T>( await reader.ReadToEndAsync() );
            }
        }

        public static async Task<IEnumerable<T>> LoadJsonObjectCollectionFromResourcePathAsync<TApplication, T>( this TApplication app, string resourcePath )
        {
            var collection = new List<T>();
            var assembly = typeof( TApplication ).GetTypeInfo().Assembly;
            foreach( string resourceName in assembly.GetManifestResourceNames().Where( r => r.StartsWith( resourcePath ) ) )
            {
                using( var reader = new StreamReader( assembly.GetManifestResourceStream( resourceName ) ) )
                {
                    collection.Add( JsonConvert.DeserializeObject<T>( await reader.ReadToEndAsync() ) );                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
                }
            }

            return collection;
        }
    }
}
