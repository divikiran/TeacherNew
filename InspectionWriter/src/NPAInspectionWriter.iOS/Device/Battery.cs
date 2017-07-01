using Foundation;
using UIKit;
using NPAInspectionWriter.Device;
using System;

namespace NPAInspectionWriter.iOS.Device
{
    /// <summary>
    ///     Battery portion for Apple devices.
    /// </summary>
    public partial class Battery : IBattery
    {
        /// <summary>
        ///     Gets the battery level.
        /// </summary>
        /// <returns>Battery level in percentage, 0-100</returns>
        public int Level
        {
            get
            {
                UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;
                return ( int )( UIDevice.CurrentDevice.BatteryLevel * 100 );
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this <see cref="Battery" /> is charging.
        /// </summary>
        /// <value><c>true</c> if charging; otherwise, <c>false</c>.</value>
        public bool Charging
        {
            get { return UIDevice.CurrentDevice.BatteryState != UIDeviceBatteryState.Unplugged; }
        }

        public event EventHandler<EventArgs<int>> OnLevelChange;
        public event EventHandler<EventArgs<bool>> OnChargerStatusChanged;

        /// <summary>
        ///     Starts the level monitor.
        /// </summary>
        void StartLevelMonitoring()
        {
            UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;
            NSNotificationCenter.DefaultCenter.AddObserver(
                UIDevice.BatteryLevelDidChangeNotification,
                ( NSNotification n ) =>
                {
                    if( OnLevelChange != null )
                    {
                        OnLevelChange( OnLevelChange, new EventArgs<int>( Level ) );
                    }
                } );
        }

        /// <summary>
        ///     Stops the level monitoring.
        /// </summary>
        void StopLevelMonitoring()
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver( UIDevice.BatteryLevelDidChangeNotification );

            // if charger monitor does not have subscribers then lets disable battery monitoring
            UIDevice.CurrentDevice.BatteryMonitoringEnabled = ( OnChargerStatusChanged != null );
        }

        /// <summary>
        ///     Stops the charger monitoring.
        /// </summary>
        void StopChargerMonitoring()
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver( UIDevice.BatteryStateDidChangeNotification );

            // if level monitor does not have subscribers then lets disable battery monitoring
            UIDevice.CurrentDevice.BatteryMonitoringEnabled = ( OnLevelChange != null );
        }

        /// <summary>
        ///     Starts the charger monitoring.
        /// </summary>
        void StartChargerMonitoring()
        {
            UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;
            NSNotificationCenter.DefaultCenter.AddObserver(
                UIDevice.BatteryStateDidChangeNotification,
                ( NSNotification n ) =>
                {
                    if( OnChargerStatusChanged != null )
                    {
                        OnChargerStatusChanged(
                            OnChargerStatusChanged,
                            new EventArgs<bool>( Charging ) );
                    }
                } );
        }
    }
}
