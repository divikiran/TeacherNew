
using System;
using System.Collections;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SQLite.Net.Async;

namespace NPAInspectionWriter.Extensions
{
    public static class SQLiteAsyncConnectionExtensions
    {
        public static async Task<int> DeleteWhereAsync<T>( this SQLiteAsyncConnection connection, Expression<Func<T,bool>> predExpr, CancellationToken token = default( CancellationToken ) ) where T : class
        {
            var items = await connection.Table<T>().Where( predExpr ).ToListAsync();

            foreach( var item in items )
            {
                await connection.DeleteAsync( item, token );
            }

            return items.Count;
        }

        public static async Task<bool> ExistsAsync<T>( this SQLiteAsyncConnection connection, Expression<Func<T, bool>> predExpr ) where T : class
        {
            return await connection.Table<T>().Where( predExpr ).FirstOrDefaultAsync() != null;
        }

        public static async Task<int> InsertOrReplaceAsync<T>( this SQLiteAsyncConnection connection, object item, CancellationToken token = default( CancellationToken ) ) where T : class
        {
            return await connection.InsertOrReplaceAsync( ( T )item, token );
        }

        public static async Task<int> InsertOrReplaceAllAsync<T>( this SQLiteAsyncConnection connection, IEnumerable items, CancellationToken token = default( CancellationToken ) ) where T : class
        {
            int count = 0;

            foreach( var item in items )
            {
                count += await connection.InsertOrReplaceAsync<T>( item, token );
            }

            return count;
        }

        public static async Task<int> InsertOrIgnoreAsync<T>( this SQLiteAsyncConnection connection, object item ) where T : class
        {
            return await connection.InsertOrIgnoreAsync( ( T )item );
        }

        public static async Task<int> InsertOrIgnoreAllAsync<T>( this SQLiteAsyncConnection connection, IEnumerable items ) where T : class
        {
            int count = 0;

            foreach( var item in items )
            {
                count += await connection.InsertOrIgnoreAsync( item );
            }

            return count;
        }
    }
}
