using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NPAInspectionWriter.Caching;
using NPAInspectionWriter.Logging;
using System.Runtime.InteropServices.WindowsRuntime;
using NPAInspectionWriter.Models;
using NPAInspectionWriter.Views;

namespace NPAInspectionWriter.Helpers
{
    public class InspectionWriterClient : HttpClient
    {
        private const string AuthCredentialStorageName = "AuthCred";

        internal static string BasicAuthCredential
        {
            get
            {
                return CachingAgent.Provider.Get<string>( AuthCredentialStorageName );
            }
        }

        bool useBasicAuth = true;

        public InspectionWriterClient() : base()
        {
            InitInspectionWriterClient();
        }

        public InspectionWriterClient(HttpMessageHandler handler) : base(handler)
        {
            InitInspectionWriterClient();
        }

        public InspectionWriterClient(HttpMessageHandler handler, bool disposeHandler) : base(handler, disposeHandler)
        {
            InitInspectionWriterClient();
        }

        public InspectionWriterClient(string userName, string password) : base()
        {
            SetCredential(userName, password);
            InitInspectionWriterClient();
        }

        public InspectionWriterClient(HttpClientHandler handler, string userName, string password) : base(handler)
        {
            SetCredential(userName, password);
            InitInspectionWriterClient();
        }

        public HttpResponseMessage LastResponse { get; private set; }

        private void InitInspectionWriterClient()
        {
            try
            {
                BaseAddress = new Uri(new Uri(Settings.Current.InspectionWriterWebApiBase), $"WebApi-{Settings.Current.ApiVersion}/");
                DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue()
                {
                    MaxAge = TimeSpan.FromSeconds(Constants.CacheControlSeconds)
                };
                DefaultRequestHeaders.UserAgent.ParseAdd($"InspectionWriterApp/{App.AppVersion}");
                DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                DefaultRequestHeaders.Connection.Add("keep-alive");
                if (!useBasicAuth && !string.IsNullOrWhiteSpace(Settings.Current.AuthToken))
                {
                    DefaultRequestHeaders.Add("SessionToken", Settings.Current.AuthToken);
                }
                else if (useBasicAuth && !string.IsNullOrWhiteSpace(BasicAuthCredential))
                {
                    DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", BasicAuthCredential);
                }
            }
            catch (Exception e)
            {
                Log($"----- InspectionWriter Web API Base: {Settings.Current.InspectionWriterWebApiBase}");
                Log($"----- WebApi-{Settings.Current.ApiVersion}/");
                Log(e);
            }
        }

        private void SetCredential(string userName, string password)
        {
            var credential = Convert.ToBase64String(Encoding.GetEncoding("iso-8859-1").GetBytes($"{userName}:{password}"));
            AsyncHelpers.RunSync(async () => await AppRepository.Instance.AddCurrentObjectAsync(AuthCredentialStorageName, credential));
            CachingAgent.Provider.Set(AuthCredentialStorageName, credential);
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", BasicAuthCredential);
            useBasicAuth = true;
        }

        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default(CancellationToken))
        {
            Log($"{request.RequestUri}:\n\r{request.Headers}");
            LastResponse = await base.SendAsync(request, cancellationToken);

            if (LastResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                        App.HandleExpiredSessionEvent();
                });
            }

            return LastResponse;
        }

        public async Task<bool> LogoutAsync()
        {
            var response = await PostAsync("logout", new StringContent(string.Empty));
            await CachingAgent.Provider.RemoveAsync(AuthCredentialStorageName);

            return response.IsSuccessStatusCode;
        }

        private void Log(object message)
        {
            StaticLogger.DebugLogger?.Invoke(message);
        }

        private void Log(Exception e)
        {
            StaticLogger.ExceptionLogger?.Invoke(e);
        }
    }
}
