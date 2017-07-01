using System;
using NPAInspectionWriter.Helpers;

namespace NPAInspectionWriter.Device
{
    /// <summary>
    /// Class Gyroscope.
    /// </summary>
    public partial class Gyroscope : IGyroscope
    {
        /// <summary>
        /// Occurs when [reading available].
        /// </summary>
        private event EventHandler<EventArgs<Vector3>> readingAvailable;

        /// <summary>
        /// Initializes a new instance of the <see cref="Gyroscope"/> class.
        /// </summary>
        public Gyroscope()
        {
            this.Interval = AccelerometerInterval.Ui;
        }

        /// <summary>
        /// Occurs when [reading available].
        /// </summary>
        public event EventHandler<EventArgs<Vector3>> ReadingAvailable
        {
            add
            {
                if( readingAvailable == null )
                {
                    Start();
                }
                readingAvailable += value;
            }
            remove
            {
                readingAvailable -= value;
                if( readingAvailable == null )
                {
                    Stop();
                }
            }
        }

        /// <summary>
        /// Gets the latest reading vector
        /// </summary>
        /// <value>Rotation values in radians per second</value>
        public Vector3 LatestReading
        {
            get;
            private set;
        }
        public AccelerometerInterval Interval { get; set ; }

        partial void Start();

        partial void Stop();
    }
}
