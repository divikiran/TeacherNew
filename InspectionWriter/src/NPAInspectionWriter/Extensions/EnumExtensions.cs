using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NPAInspectionWriter.iOS.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets an attribute on an Enum value.
        /// </summary>
        /// <typeparam name="T">The type of attribute to return.</typeparam>
        /// <param name="enumVal">The Enum to evaluate.</param>
        /// <returns>The specified attribute.</returns>
        public static T GetAttribute<T>( this Enum enumVal ) where T : Attribute
        {
            return enumVal.GetMemberInfo().GetCustomAttribute<T>();
        }

        public static IEnumerable<Attribute> GetAttributes( this Enum enumVal )
        {
            return enumVal.GetMemberInfo().GetCustomAttributes();
        }

        public static MemberInfo GetMemberInfo( this Enum enumVal )
        {
            var typeInfo = enumVal.GetType().GetTypeInfo();
            return typeInfo.DeclaredMembers.First( x => x.Name == enumVal.ToString() );
        }

        public static IEnumerable<Enum> GetFlags( this Enum input )
        {
            foreach( Enum value in Enum.GetValues( input.GetType() ) )
                if( input.HasFlag( value ) )
                    yield return value;
        }
    }
}
