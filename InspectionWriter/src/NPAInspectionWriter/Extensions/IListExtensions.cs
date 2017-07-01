using System.Collections.Generic;

namespace NPAInspectionWriter.Extensions
{
    public static class IListExtensions
    {
        public static void ReplaceRange<T>( this IList<T> list, IEnumerable<T> enumerable )
        {
            list.Clear();
            list.AddRange( enumerable );
        }

        public static void AddRange<T>( this IList<T> list, IEnumerable<T> enumerable )
        {
            foreach( var item in enumerable )
                list.Add( item );
        }
    }
}
