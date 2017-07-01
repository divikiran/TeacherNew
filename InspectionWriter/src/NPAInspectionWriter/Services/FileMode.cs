namespace NPAInspectionWriter.Services
{
    /// <summary>
    /// Enum FileMode
    /// </summary>
    public enum FileMode
    {
        /// <summary>
        /// The create new
        /// </summary>
        CreateNew = 1,
        /// <summary>
        /// The create
        /// </summary>
        Create = 2,
        /// <summary>
        /// The open
        /// </summary>
        Open = 3,
        /// <summary>
        /// The open or create
        /// </summary>
        OpenOrCreate = 4,
        /// <summary>
        /// The truncate
        /// </summary>
        Truncate = 5,
        /// <summary>
        /// The append
        /// </summary>
        Append = 6,
    }
}
