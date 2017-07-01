using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;

namespace NPAInspectionWriter.iOS.Models
{
    public class ExtendedSQLiteConnectionWithLock : SQLiteConnectionWithLock
    {
        private SQLiteAsyncConnection _asyncConnection { get; }

        public ExtendedSQLiteConnectionWithLock( ISQLitePlatform sqlitePlatform, SQLiteConnectionString connectionString, IDictionary<string, TableMapping> tableMappings = null, IDictionary<Type, string> extraTypeMappings = null ) : base( sqlitePlatform, connectionString, tableMappings, extraTypeMappings )
        {
            _asyncConnection = new SQLiteAsyncConnection( () => this );
        }

        #region AsyncConnection

        public Task<CreateTablesResult> CreateTableAsync<T>( CancellationToken cancellationToken = default( CancellationToken ) )
            where T : class =>
            _asyncConnection.CreateTableAsync<T>( cancellationToken );

        public Task<CreateTablesResult> CreateTablesAsync<T, T2>( CancellationToken cancellationToken = default( CancellationToken ) )
            where T : class
            where T2 : class =>
            _asyncConnection.CreateTablesAsync( cancellationToken, typeof( T ), typeof( T2 ) );

        public Task<CreateTablesResult> CreateTablesAsync<T, T2, T3>( CancellationToken cancellationToken = default( CancellationToken ) )
            where T : class
            where T2 : class
            where T3 : class =>
            _asyncConnection.CreateTablesAsync( cancellationToken, typeof( T ), typeof( T2 ), typeof( T3 ) );

        public Task<CreateTablesResult> CreateTablesAsync<T, T2, T3, T4>( CancellationToken cancellationToken = default( CancellationToken ) )
            where T : class
            where T2 : class
            where T3 : class
            where T4 : class =>
            _asyncConnection.CreateTablesAsync( cancellationToken, typeof( T ), typeof( T2 ), typeof( T3 ), typeof( T4 ) );

        public Task<CreateTablesResult> CreateTablesAsync( params Type[] types )
        {
            if( types == null )
            {
                throw new ArgumentNullException( "types" );
            }
            return _asyncConnection.CreateTablesAsync( CancellationToken.None, types );
        }

        public Task<CreateTablesResult> CreateTablesAsync( CancellationToken cancellationToken = default( CancellationToken ), params Type[] types ) =>
            _asyncConnection.CreateTablesAsync( cancellationToken, types );

        public Task<int> DropTableAsync<T>( CancellationToken cancellationToken = default( CancellationToken ) )
            where T : class =>
            DropTableAsync( typeof( T ), cancellationToken );

        public Task<int> DropTableAsync( Type t, CancellationToken cancellationToken = default( CancellationToken ) ) =>
            _asyncConnection.DropTableAsync( t, cancellationToken );

        public Task<int> InsertAsync( object item, CancellationToken cancellationToken = default( CancellationToken ) ) =>
            _asyncConnection.InsertAsync( item, cancellationToken );

        public Task<int> UpdateAsync( object item, CancellationToken cancellationToken = default( CancellationToken ) ) =>
            _asyncConnection.UpdateAsync( item, cancellationToken );

        public Task<int> InsertOrIgnoreAsync( object item ) =>
            _asyncConnection.InsertOrIgnoreAsync( item );

        public Task<int> InsertOrIgnoreAllAsync( IEnumerable objects, CancellationToken cancellationToken = default( CancellationToken ) ) =>
            _asyncConnection.InsertOrIgnoreAllAsync( objects, cancellationToken );

        public Task<int> InsertOrReplaceAsync( object item, CancellationToken cancellationToken = default( CancellationToken ) ) =>
            _asyncConnection.InsertOrReplaceAsync( item, cancellationToken );

        public Task<int> DeleteAsync( object item, CancellationToken cancellationToken = default( CancellationToken ) ) =>
            _asyncConnection.DeleteAsync( item );

        public Task<int> DeleteAllAsync<T>( CancellationToken cancellationToken = default( CancellationToken ) ) =>
            DeleteAllAsync( typeof( T ), cancellationToken );

        public Task<int> DeleteAllAsync( Type t, CancellationToken cancellationToken = default( CancellationToken ) ) =>
            _asyncConnection.DeleteAllAsync( t, cancellationToken );

        public Task<int> DeleteAsync<T>( object pk, CancellationToken cancellationToken = default( CancellationToken ) ) =>
            _asyncConnection.DeleteAsync<T>( pk, cancellationToken );

        public Task<T> GetAsync<T>( object pk, CancellationToken cancellationToken = default( CancellationToken ) )
            where T : class =>
            _asyncConnection.GetAsync<T>( pk, cancellationToken );

        public Task<T> FindAsync<T>( object pk, CancellationToken cancellationToken = default( CancellationToken ) )
            where T : class =>
            _asyncConnection.FindAsync<T>( pk, cancellationToken );

        public Task<T> GetAsync<T>( Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default( CancellationToken ) )
            where T : class =>
            _asyncConnection.GetAsync<T>( predicate, cancellationToken );

        public Task<T> FindAsync<T>( Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default( CancellationToken ) )
            where T : class =>
            _asyncConnection.FindAsync<T>( predicate, cancellationToken );

        public Task<int> ExecuteAsync( string query, params object[] args ) =>
            _asyncConnection.ExecuteAsync( query, args );

        public Task<int> ExecuteAsync( CancellationToken cancellationToken, string query, params object[] args ) =>
            _asyncConnection.ExecuteAsync( cancellationToken, query, args );

        public Task<int> InsertAllAsync( IEnumerable items, CancellationToken cancellationToken = default( CancellationToken ) ) =>
            _asyncConnection.InsertAllAsync( items, cancellationToken );

        public Task<int> InsertOrReplaceAllAsync( IEnumerable items, CancellationToken cancellationToken = default( CancellationToken ) ) =>
            _asyncConnection.InsertOrReplaceAllAsync( items, cancellationToken );

        public Task<int> UpdateAllAsync( IEnumerable items, CancellationToken cancellationToken = default( CancellationToken ) ) =>
            _asyncConnection.UpdateAllAsync( items, cancellationToken );

        public Task RunInTransactionAsync( Action<SQLiteConnection> action, CancellationToken cancellationToken = default( CancellationToken ) ) =>
            _asyncConnection.RunInTransactionAsync( action, cancellationToken );

        public AsyncTableQuery<T> Table<T>()
            where T : class =>
            _asyncConnection.Table<T>();

        public TableQuery<T> TableSynchronous<T>()
            where T : class
            => base.Table<T>();

        public Task<T> ExecuteScalarAsync<T>( string sql, params object[] args ) =>
            _asyncConnection.ExecuteScalarAsync<T>( sql, args );

        public Task<T> ExecuteScalarAsync<T>( CancellationToken cancellationToken, string sql, params object[] args ) =>
            _asyncConnection.ExecuteScalarAsync<T>( cancellationToken, sql, args );

        //public Task ExecuteNonQueryAsync( string sql, params object[] args ) =>
        //    _asyncConnection.ExecuteNonQueryAsync( sql, args );

        //public Task ExecuteNonQueryAsync( CancellationToken cancellationToken, string sql, params object[] args ) =>
        //    _asyncConnection.ExecuteNonQueryAsync( cancellationToken, sql, args );

        public Task<List<T>> QueryAsync<T>( string sql, params object[] args )
            where T : class
        {
            return QueryAsync<T>( CancellationToken.None, sql, args );
        }

        public async Task<T> QuerySingleAsync<T>( string sql, params object[] args )
            where T : class
        {
            return ( await QueryAsync<T>( sql, args ) ).FirstOrDefault();
        }

        public Task<List<T>> QueryAsync<T>( CancellationToken cancellationToken, string sql, params object[] args )
            where T : class =>
            _asyncConnection.QueryAsync<T>( cancellationToken, sql, args );

        public Task<TableMapping> GetMappingAsync<T>() =>
            _asyncConnection.GetMappingAsync<T>();

        #endregion

        #region Async Extensions

        public async Task<int> DeleteWhereAsync<T>( Expression<Func<T, bool>> predExpr, CancellationToken token = default( CancellationToken ) ) where T : class
        {
            var items = await Table<T>().Where( predExpr ).ToListAsync();

            foreach( var item in items )
            {
                await DeleteAsync( item, token );
            }

            return items.Count;
        }

        public async Task<bool> ExistsAsync<T>( Expression<Func<T, bool>> predExpr ) where T : class =>
            await Table<T>().Where( predExpr ).FirstOrDefaultAsync() != null;

        public async Task<int> InsertOrReplaceAsync<T>( object item, CancellationToken token = default( CancellationToken ) ) where T : class =>
            await InsertOrReplaceAsync( ( T )item, token );

        public async Task<int> InsertOrReplaceAllAsync<T>( IEnumerable items, CancellationToken token = default( CancellationToken ) ) where T : class
        {
            int count = 0;

            foreach( var item in items )
            {
                count += await InsertOrReplaceAsync<T>( item, token );
            }

            return count;
        }

        public async Task<int> InsertOrIgnoreAsync<T>( object item ) where T : class =>
            await InsertOrIgnoreAsync( ( T )item );

        public async Task<int> InsertOrIgnoreAllAsync<T>( IEnumerable items ) where T : class
        {
            int count = 0;

            foreach( var item in items )
            {
                count += await InsertOrIgnoreAsync( item );
            }

            return count;
        }

        #endregion
    }
}
