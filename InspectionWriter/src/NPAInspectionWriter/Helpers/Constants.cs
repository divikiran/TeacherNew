﻿namespace NPAInspectionWriter.Helpers
{
    public static class Constants
    {
        #region Build Agent Variables

        // The following constants should never be changed in source control. These are meant to act as a placeholder to be changed
        // an automated build system such as Visual Studio Team Services, Jenkins, or Team City. These should only be changed for
        // releases in which you wish to override the default use DebugWebApi.

        public const string HockeyAppiOS = "f1c26f639bac48f8b41a763603bd66e9";

        /// <summary>
        /// The base Uri of the Web Api to use if overriding the DebugWebApiBase value.
        /// </summary>
        public const string InspectionWriterWebApiBase = "__InspectionWriterWebApiBase__";

        /// <summary>
        /// This should be the Web Api Version i.e. 'A' for WebApi-A, 'B' for WebApi-B.
        /// </summary>
        public const string InspectionWriterWebApiVersion = "__InspectionWriterWebApiVersion__";

        #endregion

        #region API Settings


        /// <summary>
        /// The base path for the Stage Web Api
        /// </summary>
        public const string StageWebApiBase = "https://dev-staging-vm.powersportauctions.com/";

		/// <summary>
		/// The base path for the Remote Access Debug Web Api
		/// </summary>
		//public const string DebugWebApiBase = "https://208.33.179.105/";
		public const string DebugWebApiBase = "https://65.153.138.111/";
		//public const string DebugWebApiBase = "https://stgsrv.npauctions.com/"; //Info : not working for Div

        /// <summary>
        /// The root hostname for the Stage Server (used to evaluate whether we are running on Stage).
        /// </summary>
        public const string StageHostName = "dev-staging-vm";

        #endregion

        /// <summary>
        /// The number of hours until a login should be considered expired.
        /// </summary>
        public const int LoginExpiresHours = 10;

        /// <summary>
        /// Value used for the CacheControl header in the InspectionWriter API Client
        /// </summary>
        public const int CacheControlSeconds = 60;

        /// <summary>
        /// The file name containing the NPA Definitions
        /// </summary>
        public const string DefinitionsFileName = "NPADefinitions.json";

        /// <summary>
        /// The resource path for the NPA Logo
        /// </summary>
        public const string NPALogoResourcePath = "NPAInspectionWriter.Assets.Images.NPALogo.png";

        /// <summary>
        /// The resource path when we have no image for a Vehicle
        /// </summary>
        public const string VehicleNoImageSource = "NPAInspectionWriter.Assets.Images.no-image.jpg";

        /// <summary>
        /// This is the name of the local SQLite database
        /// </summary>
        public const string DatabaseName = "InspectionWriter.sqlite3";

        /// <summary>
        /// The max pixel size for an image
        /// </summary>
        public const int MaxPixelSize = 1600;

        /// <summary>
        /// The inspection source
        /// </summary>
        public const string InspectionSource = "00001";

        #region Current Object Key's

        /// <summary>
        /// Database Key for the Current User
        /// </summary>
        public const string CurrentUserKey = "currentUser";

        /// <summary>
        /// Database Key for the Current User And Location (SAN/SomeUser)
        /// </summary>
        public const string CurrentUserAndLocationKey = "currentUserAndLocation";

        /// <summary>
        /// Database Key for the Current Vehicle
        /// </summary>
        public const string CurrentVehicleKey = "currentVehicle";

        /// <summary>
        /// Database Key for when the Session Expires
        /// </summary>
        public const string SessionExpiresKey = "sessionExpires";

        /// <summary>
        /// Database Key for the Last User Error Message
        /// </summary>
        public const string LastErrorMessageKey = "lastErrorMessage";

        /// <summary>
        /// Database Key for the Current Inspection
        /// </summary>
        public const string CurrentInspectionKey = "currentInspection";

        /// <summary>
        /// Database Key for the Current Inspection State (New, Editing, Locked);
        /// </summary>
        public const string CurrentInspectionState = "currentInspectionState";

        /// <summary>
        /// Database Key for the Selected Printer
        /// </summary>
        public const string SelectedPrinterKey = "selectedPrinter";

        /// <summary>
        /// Database Key for the Current Vehicle Image
        /// </summary>
        public const string CurrentVehicleImageKey = "currentVehicleImage";

        /// <summary>
        /// Database Key for the Requried App Version
        /// </summary>
        public const string RequiredAppVersionKey = "requiredAppVersion";

        #endregion
    }
}
