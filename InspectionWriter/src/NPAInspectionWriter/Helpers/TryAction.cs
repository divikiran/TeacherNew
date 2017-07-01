﻿using System;
using System.Threading.Tasks;
using NPAInspectionWriter.Logging;

namespace NPAInspectionWriter.Helpers
{
    public static class TryAction
    {
        public static bool TryActionWithLogging( Action action )
        {
            try
            {
                action?.Invoke();
                return true;
            }
            catch( Exception e )
            {
                StaticLogger.ExceptionLogger?.Invoke( e );
                return false;
            }
        }

        public static async Task<bool> TryActionWithLoggingAsync( Func<Task> asyncAction )
        {
            try
            {
                await asyncAction?.Invoke();
                return true;
            }
            catch( Exception e )
            {
                StaticLogger.ExceptionLogger?.Invoke( e );
                return false;
            }
        }
    }
}
