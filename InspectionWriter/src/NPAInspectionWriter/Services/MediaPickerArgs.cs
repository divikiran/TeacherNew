using System;

namespace NPAInspectionWriter.Services
{
    /// <summary>
    /// Class MediaPickerArgs.
    /// </summary>
    public class MediaPickerArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPickerArgs" /> class.
        /// </summary>
        /// <param name="mf">The mf.</param>
        public MediaPickerArgs( MediaFile mf )
        {
            MediaFile = mf;
        }

        /// <summary>
        /// Gets the media file.
        /// </summary>
        /// <value>The media file.</value>
        public MediaFile MediaFile { get; private set; }
    }
}
