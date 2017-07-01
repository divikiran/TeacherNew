namespace NPAInspectionWriter.Services
{
    /// <summary>
    /// Class MediaStorageOptions.
    /// </summary>
    public class MediaStorageOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaStorageOptions" /> class.
        /// </summary>
        protected MediaStorageOptions()
        {
        }

        /// <summary>
        /// Gets or sets the directory.
        /// </summary>
        /// <value>The directory.</value>
        public string Directory { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the maximum pixel dimension.
        /// </summary>
        /// <value>The maximum pixel dimension.</value>
        public int? MaxPixelDimension { get; set; }

        /// <summary>
        /// Gets or sets the percent quality.
        /// </summary>
        /// <value>The percent quality.</value>
        public int? PercentQuality { get; set; }
    }
}
