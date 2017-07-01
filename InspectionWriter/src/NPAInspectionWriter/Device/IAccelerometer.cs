using System;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter;

namespace NPAInspectionWriter.Device
{
    /// <summary>
    /// The Accelerometer interface.
    /// </summary>
    public interface IAccelerometer
    {
        /// <summary>
        /// The reading available event handler.
        /// </summary>
        event EventHandler<EventArgs<Vector3>> ReadingAvailable;

        /// <summary>
        /// Gets the latest reading.
        /// </summary>
        /// <value>The latest reading.</value>
        Vector3 LatestReading { get; }

        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        /// <value>The interval.</value>
        AccelerometerInterval Interval { get; set; }
    }
}
