using System;

namespace NPAInspectionWriter.Helpers
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException( string message ) : base( message ) { }
    }
}
