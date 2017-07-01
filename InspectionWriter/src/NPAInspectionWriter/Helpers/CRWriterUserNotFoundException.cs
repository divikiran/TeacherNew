using NPAInspectionWriter.AppData;

namespace NPAInspectionWriter.Helpers
{
    public class CRWriterUserNotFoundException : ObjectNotFoundException
    {
        public CRWriterUserNotFoundException() : base( AppMessages.UserNotFoundMessage )
        {
        }
    }
}
