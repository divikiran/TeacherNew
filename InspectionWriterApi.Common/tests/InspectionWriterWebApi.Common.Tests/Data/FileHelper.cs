using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace InspectionWriterWebApi.Common.Tests.Data
{
    public class FileHelper
    {
        ITestOutputHelper output { get; }

        public FileHelper( ITestOutputHelper output )
        {
            this.output = output;
        }

        public string GetResourceFile( string name )
        {
            try
            {
                var assembly = typeof( FileHelper ).Assembly;

                var resourceName = assembly.GetManifestResourceNames().FirstOrDefault( x => x.EndsWith( name ) );

                if( !string.IsNullOrWhiteSpace( resourceName ) )
                {
                    using( var stream = assembly.GetManifestResourceStream( resourceName ) )
                    using( var reader = new StreamReader( stream ) )
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch( Exception e )
            {
                output?.WriteLine( e.ToString() );
            }

            return string.Empty;
        }

        public T LoadEmbeddedJsonFile<T>( string name )
        {
            try
            {
                var json = GetResourceFile( name );
                output?.WriteLine( json );

                if( string.IsNullOrWhiteSpace( json ) )
                {
                    return default( T );
                }

                return JsonConvert.DeserializeObject<T>( json );
            }
            catch( Exception e )
            {
                output?.WriteLine( e.ToString() );
                return default( T );
            }
        }
    }
}
