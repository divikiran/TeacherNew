using NPAInspectionWriter.AppData;

namespace NPAInspectionWriter.Helpers
{
    public class LocalInspectionNotFoundException : ObjectNotFoundException
    {
        public LocalInspectionNotFoundException() : base( AppMessages.CurrentInspectionNotFoundMessage )
        {
        }
    }
}
