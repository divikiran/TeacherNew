using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using NPAInspectionWriter.Caching;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.Models;

namespace NPAInspectionWriter.Helpers
{
    /// <summary>
    /// This is the Settings class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters.
    /// </summary>
    public class Settings : INotifyPropertyChanged
    {
        //static ISettings AppSettings
        //{
        //    get
        //    {
        //        return CrossSettings.Current;
        //    }
        //}

        static Settings settings;

        public static Settings Current
        {
            get { return settings ?? ( settings = new Settings() ); }
        }

        #region Setting Constants

        private static readonly string UserNameDefault = string.Empty;

        private const UserLevel UserLevelDefault = UserLevel.Disabled;

        private const bool IgnoreCellularConnectivityDefault = false;

        private const bool HideCancelButtonDefault = true;

        private static readonly char ApiVersionDefault = 'C';

        private const int InspectionLookbackDefault = 90;

        private static Guid LocationIdDefault = new Guid( "c4b1d148-57e2-4fd2-9dae-0121d40854a0" );

        private static readonly string AuthTokenDefault = string.Empty;

        #endregion

        #region Persistent Settings

        /// <summary>
        /// Api Version to use 'A'/'B'/etc. 'Defaults to 'A'
        /// </summary>
        public char ApiVersion
        {
            get { return GetProperty( ApiVersionDefault ); }
            set { SetProperty( value ); }
        }

        public string AuthToken
        {
            get { return GetProperty( AuthTokenDefault ); }
            set { SetProperty( value ); }
        }

        public string UserName
        {
            get { return GetProperty( UserNameDefault ); }
            set { SetProperty( value ); }
        }

        public string Authorization
        {
            get { return GetProperty(AuthTokenDefault); }
            set { SetProperty(value); }
        }

        public UserLevel UserLevel
        {
            get { return GetProperty( UserLevelDefault ); }
            set { SetProperty( value ); }
        }

        public bool IgnoreCellularConnectivity
        {
            get { return GetProperty( IgnoreCellularConnectivityDefault ); }
            set { SetProperty( value ); }
        }

        public bool HideCancelButton
        {
            get { return GetProperty( HideCancelButtonDefault ); }
            set { SetProperty( value ); }
        }

        /// <summary>
        /// The number of days to lookback at previous inspections on a vehicle
        /// </summary>
        public int InspectionLookback
        {
            get { return GetProperty( InspectionLookbackDefault ); }
            set { SetProperty( value ); }
        }

        /// <summary>
        /// This is a calculated field set by the InspectionLookback
        /// </summary>
        public DateTime InspectionLookbackDate
        {
            // Subtract InspectionLookback from Current Date.
            get { return DateTime.Now.Date.AddDays( -1 * InspectionLookback ); }
        }

        /// <summary>
        /// This is the current location id
        /// </summary>
        public Guid LocationId
        {
            get { return GetProperty( LocationIdDefault ); }
            set { SetProperty( value ); }
        }

        #endregion

        #region Session Settings

        private Guid _currentInspectionId;
        public Guid CurrentInspectionId
        {
            get { return _currentInspectionId; }
            set { SetProperty( ref _currentInspectionId, value ); }
        }

        private Guid _currentVehicleId;
        public Guid CurrentVehicleId
        {
            get { return _currentVehicleId; }
            set { SetProperty( ref _currentVehicleId, value ); }
        }

        private bool isConnected;
        public bool IsConnected
        {
            get { return isConnected; }
            set { SetProperty( ref isConnected, value ); }
        }

        private bool _disableWebApiLocationOverride;
        public bool DisableWebApiLocationOverride
        {
            get { return _disableWebApiLocationOverride; }
            set { SetProperty( ref _disableWebApiLocationOverride, value ); }
        }

        private bool _disableWebApiVersionOverride;
        public bool DisableWebApiVersionOverride
        {
            get { return _disableWebApiVersionOverride; }
            set { SetProperty( ref _disableWebApiVersionOverride, value ); }
        }

        private string inspectionWriterWebApiBase = string.Empty;
        public string InspectionWriterWebApiBase
        {
            get
            {
                if( !string.IsNullOrWhiteSpace( inspectionWriterWebApiBase ) )
                    return inspectionWriterWebApiBase;

                Uri test;
                // This should only execute the first time when inspectionWriterWebApiBase is still an Empty string.
                // 1) Disable Production API use if Debugger Is Attached
                // 2) Verify Constant Value for the Production API is:
                //   - not equal to the placeholder value
                //   - not an empty string
                //   - is a valid URL
                if( !System.Diagnostics.Debugger.IsAttached &&
                    Constants.InspectionWriterWebApiBase != $"__{nameof( Constants.InspectionWriterWebApiBase )}__" &&
                    !string.IsNullOrWhiteSpace( Constants.InspectionWriterWebApiBase ) &&
                    Uri.TryCreate( Constants.InspectionWriterWebApiBase, UriKind.Absolute, out test ) )
                {
                    inspectionWriterWebApiBase = Constants.InspectionWriterWebApiBase;
                }
                else
                {
                    inspectionWriterWebApiBase = Constants.DebugWebApiBase;
                }

                OnPropertyChanged( nameof( InspectionWriterWebApiBase ) );

                return inspectionWriterWebApiBase;
            }
            set
            {
                if( Constants.InspectionWriterWebApiBase == $"__{nameof( Constants.InspectionWriterWebApiBase )}__" )
                {
                    SetProperty( ref inspectionWriterWebApiBase, value );
                }
            }
        }

        #endregion

        /// <summary>
        /// Runs as Debug if we are using the DebugWebApiBase
        /// </summary>
        public RuntimeEnvironment Environment
        {
            get
            {
                if( InspectionWriterWebApiBase.Contains( Constants.StageHostName ) )
                    return RuntimeEnvironment.Stage;
                else if( InspectionWriterWebApiBase == Constants.DebugWebApiBase )
                    return RuntimeEnvironment.Debug;
                else if( DisableWebApiLocationOverride )
                    return RuntimeEnvironment.Other;
                else
                    return RuntimeEnvironment.Production;
            }
        }

        public AmsUser AppUser { get; internal set; }

        public const string VehicleCachePath = "Vehicles";

        public static string CurrentInspectionImageSavePath() =>
            CurrentInspectionImageSavePath( Current.CurrentVehicleId, Current.CurrentInspectionId );

        public static string ImageSavePath( Guid imageId ) =>
            System.IO.Path.Combine( CurrentInspectionImageSavePath(), $"{imageId}.png" );

        public static string CurrentInspectionImageSavePath( Guid vehicleId, Guid inspectionId ) =>
            CurrentInspectionImageSavePath( vehicleId.ToString(), inspectionId.ToString() );

        public static string CurrentInspectionImageSavePath( string vehicleId, string inspectionId )
        {
            if( string.IsNullOrWhiteSpace( vehicleId ) ) throw new ArgumentNullException( nameof( vehicleId ) );
            if( string.IsNullOrWhiteSpace( inspectionId ) ) throw new ArgumentNullException( nameof( inspectionId ) );

            return $"{VehicleCachePath}/{vehicleId}/Inspections/{inspectionId}/Images/";

        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private bool SetProperty<T>( T value, [CallerMemberName]string name = "" )
        {
            if( CachingAgent.Provider.Set( name, value ) )
            {
                OnPropertyChanged( name );
                return true;
            }

            return false;
        }

        private bool SetProperty<T>( ref T storage, T value, [CallerMemberName]string name = "" )
        {
            if( object.Equals( storage, value ) ) return false;
            storage = value;
            OnPropertyChanged( name );
            return true;
        }

        void OnPropertyChanged( [CallerMemberName]string name = "" ) =>
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );

        #endregion

        private T GetProperty<T>( T defaultValue, [CallerMemberName]string name = "" )
        {
            try
            {
                return CachingAgent.Provider.Get( name, defaultValue );
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
