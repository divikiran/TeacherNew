using System;

namespace NPAInspectionWriter.Helpers
{
    [AttributeUsage( AttributeTargets.Field, AllowMultiple = false )]
    public class PlaceholderAttribute : Attribute
    {
        public string Placeholder { get; }

        public PlaceholderAttribute( string placeholder )
        {
            Placeholder = placeholder;
        }
    }
}