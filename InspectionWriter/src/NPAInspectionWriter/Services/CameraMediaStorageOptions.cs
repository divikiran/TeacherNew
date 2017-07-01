namespace NPAInspectionWriter.Services
{
    /// <summary>
    /// Class CameraMediaStorageOptions.
    /// </summary>
    public class CameraMediaStorageOptions : MediaStorageOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CameraMediaStorageOptions" /> class.
        /// </summary>
        public CameraMediaStorageOptions()
        {
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
    }
}
