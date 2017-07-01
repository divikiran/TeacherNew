using SQLite.Net;
using SQLite.Net.Interop;

namespace NPAInspectionWriter.iOS.Models
{
    public abstract class DatabaseConnectionFactoryBase : IDatabaseConnectionFactory
    {
        public DatabaseConnectionFactoryBase(){}
        public SQLiteConnectionWithLock GetConnectionWithLock( string dbName )
        {
            return new SQLiteConnectionWithLock( GetPlatform(), new SQLiteConnectionString( PlatformDatabasePath( dbName ), true ) );
        }

        public SQLiteConnection GetConnection( string dbName )
        {
            return new SQLiteConnection( GetPlatform(), PlatformDatabasePath( dbName ) );
        }

        public SQLiteConnectionString GetConnectionString( string dbName, bool storeDateTimeAsTicks = true )
        {
            return new SQLiteConnectionString( PlatformDatabasePath( dbName ), storeDateTimeAsTicks );
        }

        public abstract ISQLitePlatform GetPlatform();
        public abstract string PlatformDatabasePath( string dbName );
    }
}
