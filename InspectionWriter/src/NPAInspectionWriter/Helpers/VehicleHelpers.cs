using System;
using System.Threading.Tasks;
using NPAInspectionWriter.Logging;
using NPA.XF.Http;

namespace NPAInspectionWriter.Helpers
{
    public class VehicleHelpers
    {
        private static Func<CommonApiClient> _createApiClient;
        public static Func<CommonApiClient> CreateApiClient
        {
            get
            {
                if( _createApiClient == null )
                {
                    StaticLogger.WarningLogger( "The CreateApiClient function has not been properly initialized." );
                    StaticLogger.DebugLogger( "This must be properly initialized: 'VehicleHelpers.CreateApiClient = SomeCreationFunction;'" );
                    _createApiClient = () => new CommonApiClient( null, new DefaultCommonApiClientOptions());
                }

                return _createApiClient;
            }
            set { _createApiClient = value; }
        }

        public static async Task<bool> IsVinValidAsync( string vin )
        {
            try
            {
                if( string.IsNullOrWhiteSpace( vin ) ) { return false; }

                //if( vin.Length != 17 ) { return false; }

                //if( !Regex.IsMatch( vin, NPAConstants.SimpleVinValidationPattern ) ) { return false; }

                CommonApiClient client = CreateApiClient.Invoke();
                return await client.VinController.IsVinValid( vin );
            }
            catch( Exception e )
            {
                StaticLogger.ExceptionLogger( e );
                return false;
            }

        }

        private class DefaultCommonApiClientOptions : ICommonApiClientOptions
        {
            public RuntimeEnvironment GetRuntimeEnvironment()
            {
                StaticLogger.WarningLogger( "Using Default CommonApiClient Options." );
                return RuntimeEnvironment.Other;
            }

            public string GetToken()
            {
                StaticLogger.WarningLogger( "There is no token availabe. Only Api calls not requiring authorization have a chance of working." );
                return string.Empty;
            }

            public ILog Logger
            {
                get { return new DebugLog( typeof( DefaultCommonApiClientOptions ) ); }
            }
        }
    }
}
