﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using NPAInspectionWriter.Services;

namespace NPAInspectionWriter.Device
{
    /// <summary>
    /// Abstracted device interface.
    /// </summary>
    public interface IDevice
    {
        /// <summary>
        /// Gets Unique Id for the device.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the display information for the device.
        /// </summary>
        IDisplay Display { get; }

        /// <summary>
        /// Gets the battery for the device.
        /// </summary>
        IBattery Battery { get; }

        /// <summary>
        /// Gets the accelerometer.
        /// </summary>
        /// <value>The accelerometer instance if available, otherwise null.</value>
        IAccelerometer Accelerometer { get; }

        /// <summary>
        /// Gets the gyroscope.
        /// </summary>
        /// <value>The gyroscope instance if available, otherwise null.</value>
        IGyroscope Gyroscope { get; }

        /// <summary>
        /// Gets the picture chooser.
        /// </summary>
        /// <value>The picture chooser.</value>
        IMediaPicker MediaPicker { get; }

        /// <summary>
        /// Gets the network service.
        /// </summary>
        /// <value>The network service.</value>
        INetwork Network { get; }

        /// <summary>
        /// Gets the bluetooth hub service.
        /// </summary>
        /// <value>The bluetooth hub service if available, otherwise null.</value>
        IBluetoothHub BluetoothHub { get; }

        /// <summary>
        /// Gets the file manager for the device.
        /// </summary>
        /// <value>Device file manager.</value>
        IFileManager FileManager { get; }

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        /// <value>The name of the device.</value>
        string Name { get; }

        /// <summary>
        /// Gets the firmware version.
        /// </summary>
        string FirmwareVersion { get; }

        /// <summary>
        /// Gets the hardware version.
        /// </summary>
        string HardwareVersion { get; }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        string Manufacturer { get; }

        /// <summary>
        /// Gets the total memory in bytes.
        /// </summary>
        long TotalMemory { get; }

        /// <summary>
        /// Gets the ISO Language Code
        /// </summary>
        string LanguageCode { get; }

        /// <summary>
        /// Gets the UTC offset
        /// </summary>
        double TimeZoneOffset { get; }

        /// <summary>
        /// Gets the timezone name
        /// </summary>
        string TimeZone { get; }

        /// <summary>
        /// Gets the <see cref="Orientation"/> of the device.
        /// </summary>
        Orientation Orientation { get; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        Assembly[] GetAssemblies();

        string GetMD5Hash( string input );

        /// <summary>
        /// Starts the default app associated with the URI for the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>The launch operation.</returns>
        Task<bool> LaunchUriAsync( Uri uri );

        /// <summary>
        /// Starts the default app associated with the URI for the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>The launch operation.</returns>
        Task<bool> LaunchUriAsync( string uri );

        Task DownloadFileToPathAsync( Uri uri, string savePath );

        Task DownloadFileToPathAsync( string uri, string savePath );
    }
}
