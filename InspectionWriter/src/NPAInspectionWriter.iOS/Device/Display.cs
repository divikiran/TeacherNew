﻿using NPAInspectionWriter.Device;
using UIKit;

namespace NPAInspectionWriter.iOS.Device
{
    /// <summary>
    ///     Apple Display class.
    /// </summary>
    public class Display : IDisplay
    {
        public static IDisplay Current { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Display" /> class.
        /// </summary>
        /// <param name="height">Height in pixels.</param>
        /// <param name="width">Width in pixels.</param>
        /// <param name="xdpi">Pixel density for X.</param>
        /// <param name="ydpi">Pixel density for  Y.</param>
        internal Display( int height, int width, double xdpi, double ydpi )
        {
            Height = height;
            Width = width;
            Xdpi = xdpi;
            Ydpi = ydpi;

            //FontManager = new FontManager(this);

            Current = this;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents the current <see cref="Display" />.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents the current <see cref="Display" />.</returns>
        public override string ToString() =>
            $"[Screen: Height={Height}, Width={Width}, Xdpi={Xdpi}, Ydpi={Ydpi}]";

        #region IScreen implementation

        /// <summary>
        /// Gets the screen height in pixels
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the screen width in pixels
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the screens X pixel density per inch
        /// </summary>
        public double Xdpi { get; private set; }

        /// <summary>
        /// Gets the screens Y pixel density per inch
        /// </summary>
        public double Ydpi { get; private set; }

        /// <summary>
        /// Gets the scale value of the display.
        /// </summary>
        public double Scale
        {
            get
            {
                return UIScreen.MainScreen.Scale;
            }
        }

        /// <summary>
        /// Convert width in inches to runtime pixels
        /// </summary>
        public double WidthRequestInInches( double inches )
        {
            return inches * Xdpi / UIScreen.MainScreen.Scale;
        }

        /// <summary>
        /// Convert height in inches to runtime pixels
        /// </summary>
        public double HeightRequestInInches( double inches )
        {
            return inches * Ydpi / UIScreen.MainScreen.Scale;
        }

        #endregion
    }
}
