using System;

namespace NPAInspectionWriter.Services
{
    /// <summary>
    /// Class VideoMediaStorageOptions.
    /// </summary>
    public class VideoMediaStorageOptions : MediaStorageOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoMediaStorageOptions" /> class.
        /// </summary>
        public VideoMediaStorageOptions()
        {
            Quality = VideoQuality.High;
            DesiredLength = TimeSpan.FromMinutes( 10 );
            SaveMediaOnCapture = true;
        }

        /// <summary>
        /// Gets or sets the default camera.
        /// </summary>
        /// <value>The default camera.</value>
        public CameraDevice DefaultCamera { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [save media on capture].
        /// </summary>
        /// <value><c>true</c> if [save media on capture]; otherwise, <c>false</c>.</value>
        public bool SaveMediaOnCapture { get; set; }

        /// <summary>
        /// Gets or sets the length of the desired.
        /// </summary>
        /// <value>The length of the desired.</value>
        public TimeSpan DesiredLength { get; set; }

        /// <summary>
        /// Gets or sets the quality.
        /// </summary>
        /// <value>The quality.</value>
        public VideoQuality Quality { get; set; }
    }
}
