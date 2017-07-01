namespace NPAInspectionWriter.Device
{
    /// <summary>
    /// The accelerometer interval.
    /// </summary>
    public enum AccelerometerInterval
    {
        /// <summary>
        /// The fastest interval.
        /// </summary>
        Fastest = 1,

        /// <summary>
        /// The game interval, approximately 20ms.
        /// </summary>
        Game = 20,

        /// <summary>
        /// The UI interval, approximately 70ms.
        /// </summary>
        Ui = 70,

        /// <summary>
        /// The normal interval, approximately 200ms.
        /// </summary>
        Normal = 200
    }
}
