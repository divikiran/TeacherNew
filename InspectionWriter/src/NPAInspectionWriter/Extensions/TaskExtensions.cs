using System.Threading.Tasks;

namespace NPAInspectionWriter.Extensions
{
    public static class TaskExtensions
    {
        public static TResult GetResult<TResult>( this Task<TResult> task )
        {
            var awaiter = task.GetAwaiter();
            return awaiter.GetResult();
        }
    }
}
