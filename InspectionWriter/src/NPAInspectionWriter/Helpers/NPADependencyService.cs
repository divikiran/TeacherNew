using System;
using System.Collections.Generic;
using NPAInspectionWriter.Extensions;

namespace NPAInspectionWriter.Helpers
{
    public static class NPADependencyService
    {
        static Dictionary<Type, object> s_registrations = new Dictionary<Type, object>();

        public static void Register<T>( T instance ) where T : class
        {
            s_registrations.AddOrUpdate( typeof( T ), instance );
        }

        public static void Register<TInterface, TImplementation>( TImplementation instance ) where TImplementation : class
        {
            s_registrations.AddOrUpdate( typeof( TInterface ), instance );
        }

        public static T Resolve<T>() where T : class
        {
            return ( T )s_registrations[ typeof( T ) ];
        }
    }
}
