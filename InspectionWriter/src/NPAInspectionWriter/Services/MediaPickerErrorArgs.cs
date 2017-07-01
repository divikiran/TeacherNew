using System;

namespace NPAInspectionWriter.Services
{
    /// <summary>
    /// Class MediaPickerErrorArgs.
    /// </summary>
    public class MediaPickerErrorArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPickerErrorArgs" /> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public MediaPickerErrorArgs( Exception ex )
        {
            Error = ex;
        }

        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <value>The error.</value>
        public Exception Error { get; private set; }
    }
}
