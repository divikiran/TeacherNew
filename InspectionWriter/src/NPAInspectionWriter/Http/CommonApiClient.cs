using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NPAInspectionWriter.iOS.Extensions;
using NPAInspectionWriter.Helpers;
using NPA.XF.Http.CommonApiApi;
using NPAInspectionWriter.Extensions;
using System.Diagnostics;

namespace NPA.XF.Http
{
    public class CommonApiClient : HttpClient
    {
        private const string StageApiBase = "https://stgsrv.npauctions.com/";
        private const string ProductionApiBase = "https://capi.npauctions.com";
        //private const string StageApiBase = "http://DEV-STAGING-VM:8888";

        internal ICommonApiClientOptions _options { get; }

        internal HttpResponseMessage lastResponse;

        public string Token
        {
            get {
                string token = _options.GetToken();
                return (token == null || token == "") ? NPAInspectionWriter.Caching.CachingAgent.Provider.Get<string>("AuthCred") : token;
            }
        }

        public RuntimeEnvironment RuntimeEnvironment
        {
            get { return _options.GetRuntimeEnvironment(); }
        }

        public CommonApiClient( NPAInspectionWriter.iOS.Models.IDatabaseConnectionFactory database, ICommonApiClientOptions options ) : base()
        {
            _options = options;
            SetBaseAddress();
            DefaultRequestHeaders.AddOrUpdate( "SessionToken", Token );
            database.GetPlatform();
            database.GetConnectionString("InspectionWriter.sqlite3");
        }

        #region Controllers

        private Auction _auctionController;
        public Auction AuctionController
        {
            get
            {
                if( _auctionController == null )
                    _auctionController = new Auction( this );

                return _auctionController;
            }
        }

        private AuctionLane _auctionLaneController;
        public AuctionLane AuctionLaneController
        {
            get
            {
                if( _auctionLaneController == null )
                    _auctionLaneController = new AuctionLane( this );

                return _auctionLaneController;
            }
        }

        private AuctionQcCheck _auctionQcCheckController;
        public AuctionQcCheck AuctionQcCheckController
        {
            get
            {
                if( _auctionQcCheckController == null )
                    _auctionQcCheckController = new AuctionQcCheck( this );

                return _auctionQcCheckController;
            }
        }

        private CheckConnectionController _checkConnectionController;
        public CheckConnectionController CheckConnectionController
        {
            get
            {
                if( _checkConnectionController == null )
                    _checkConnectionController = new CheckConnectionController( this );

                return _checkConnectionController;
            }
        }

        private LoginController _loginController;
        public LoginController LoginController
        {
            get
            {
                if( _loginController == null )
                    _loginController = new LoginController( this );

                return _loginController;
            }
        }

        private VinController _vinController;
        public VinController VinController
        {
            get
            {
                if( _vinController == null )
                    _vinController = new VinController( this );

                return _vinController;
            }
        }

        #endregion

        public override async Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken )
        {
            // TODO: Remove this block once we have both a stage and production endpoint
            if( _options.GetRuntimeEnvironment() == RuntimeEnvironment.Production )
            {
                Debug.WriteLine( "Warning the Common Api Client is only configured to work on Stage." );
            }

            SetBaseAddress();
            //request.Headers.Add( "SessionToken", _options.GetToken() );
            DefaultRequestHeaders.AddOrUpdate( "SessionToken", _options.GetToken() );
            return lastResponse = await base.SendAsync( request, cancellationToken );
        }

        protected virtual void SetBaseAddress()
        {
            BaseAddress = _options.GetRuntimeEnvironment() == RuntimeEnvironment.Production ?
                new Uri( ProductionApiBase ) :
                new Uri( StageApiBase );
        }
    }
}
