using System;
using System.Collections.Generic;

namespace NPAInspectionWriter.Extensions
{
    public static class IDictionaryExtensions
    {
        public static bool AddOrUpdate<TKey, TValue>( this IDictionary<TKey, TValue> dict, TKey key, TValue value )
        {
            bool updated = false;

            if( key == null || ( typeof(TKey) == typeof(string) && string.IsNullOrWhiteSpace( key as string ) ) ) return false;

            if( updated = ( ( dict.ContainsKey( key ) && !dict[ key ].Equals( value ) ) || !dict.ContainsKey( key ) ) )
            {
                dict[ key ] = value;
            }

            return updated;
        }

        public static T GetValueOrDefault<TKey, TValue, T>( this IDictionary<TKey, TValue> dict, TKey key, T defaultValue )
        {
            if( dict.ContainsKey( key ) && dict[ key ] != null )
                return ( T )Convert.ChangeType( dict[ key ], typeof( T ) );

            return defaultValue;
        }
    }
}
