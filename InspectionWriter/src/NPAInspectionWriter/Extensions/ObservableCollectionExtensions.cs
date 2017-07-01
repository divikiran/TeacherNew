using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NPAInspectionWriter.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>( this ObservableCollection<T> collection, IEnumerable<T> range )
        {
            foreach( T item in range )
            {
                collection.Add( item );
            }
        }

        public static void AddRange<T>( this ObservableCollection<T> collection, params T[] range )
        {
            foreach( T item in range )
            {
                collection.Add( item );
            }
        }

        public static void ReplaceRange<T>( this ObservableCollection<T> collection, IEnumerable<T> range )
        {
            collection.Clear();
            collection.AddRange( range );
        }

        public static void ReplaceRange<T>( this ObservableCollection<T> collection, params T[] range )
        {
            collection.Clear();
            collection.AddRange( range );
        }
    }
}
