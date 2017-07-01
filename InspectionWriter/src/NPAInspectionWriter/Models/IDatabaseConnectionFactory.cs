using SQLite.Net;
using SQLite.Net.Interop;

namespace NPAInspectionWriter.iOS.Models
{
    public interface IDatabaseConnectionFactory
    {
        
        string PlatformDatabasePath( string dbName );

        ISQLitePlatform GetPlatform();

        SQLiteConnectionWithLock GetConnectionWithLock( string dbName );

        SQLiteConnection GetConnection( string dbName );

        SQLiteConnectionString GetConnectionString( string dbName, bool storeDateTimeAsTicks = true );
    }
}
