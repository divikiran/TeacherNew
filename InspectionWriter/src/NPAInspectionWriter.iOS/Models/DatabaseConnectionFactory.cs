using System;
using System.IO;
using NPAInspectionWriter.iOS.Models;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinIOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(DatabaseConnectionFactory))]
namespace NPAInspectionWriter.iOS.Models
{
    public class DatabaseConnectionFactory : DatabaseConnectionFactoryBase
    {
        public DatabaseConnectionFactory(){}
        public override ISQLitePlatform GetPlatform()
        {
            return new SQLitePlatformIOS();
        }

        public override string PlatformDatabasePath( string dbName )
        {
            string docFolder = Environment.GetFolderPath( Environment.SpecialFolder.Personal );
            string libFolder = Path.Combine( docFolder, "..", "Library", "Databases" );

            if( !Directory.Exists( libFolder ) )
            {
                Directory.CreateDirectory( libFolder );
            }

            return Path.Combine( libFolder, dbName );
        }
    }
}
