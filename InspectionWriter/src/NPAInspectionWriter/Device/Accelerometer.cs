﻿using System;
using NPAInspectionWriter.Helpers;

namespace NPAInspectionWriter.Device
{
    /// <summary>
    /// Class Accelerometer.
    /// </summary>
    public partial class Accelerometer : IAccelerometer
    {
        /// <summary>
        /// Gravitational force is 9.81 m/s^2
        /// </summary>
        public const double Gravitation = 9.81;

        /// <summary>
        /// Initializes a new instance of the <see cref="Accelerometer" /> class.
        /// </summary>
        public Accelerometer()
        {
            this.Interval = AccelerometerInterval.Ui;
        }

        /// <summary>
        /// The reading available event handler.
        /// </summary>
        public event EventHandler<EventArgs<Vector3>> ReadingAvailable
        {
            add
            {
                if( this.readingAvailable == null )
                {
                    this.Start();
                }

                this.readingAvailable += value;
            }

            remove
            {
                this.readingAvailable -= value;
                if( this.readingAvailable == null )
                {
                    this.Stop();
                }
            }
        }

        /// <summary>
        /// Occurs when [reading available].
        /// </summary>
        private event EventHandler<EventArgs<Vector3>> readingAvailable;

        /// <summary>
        /// Gets the latest reading.
        /// </summary>
        /// <value>The latest reading.</value>
        public Vector3 LatestReading
        {
            get;
            private set;
        }
        public AccelerometerInterval Interval { get; set; }

        partial void Start();

        partial void Stop();
    }
}
