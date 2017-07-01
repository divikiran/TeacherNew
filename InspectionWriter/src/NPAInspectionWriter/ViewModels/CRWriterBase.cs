using System;
using System.Threading.Tasks;
using NPAInspectionWriter.iOS.Extensions;
using NPAInspectionWriter.Helpers;
using NPAInspectionWriter.AppData;
using NPAInspectionWriter.Extensions;
using NPAInspectionWriter.Models;
using Xamarin.Forms;

namespace NPAInspectionWriter.ViewModels
{
    public abstract class CRWriterBase : BaseViewModel
    {
        protected readonly AppRepository _db = AppRepository.Instance;

        private bool _isWaiting;
        public bool IsWaiting
        {
            get { return _isWaiting; }
            set { SetProperty(ref _isWaiting, value); }
        }

        public CRWriterBase()
        {
            UserLocation = AsyncHelpers.RunSync( async () => await _db.GetCurrentSettingAsync( Constants.CurrentUserAndLocationKey ) );
        }

        protected async Task ValidateSession()
        {
            var sessionExpires = await _db.GetCurrentSettingAsync<DateTime>( Constants.SessionExpiresKey );
            if( DateTime.Now.CompareTo( sessionExpires ) >= 0 )
            {
                // Session is expired....
                await _db.ClearAllCurrentObjectsAsync();
                App.HandleExpiredSessionEvent();
            }
        }

        public abstract string Title { get; }

        public abstract ImageSource Icon { get; }

        public string AppVersion
        {
            get { return App.AppVersion; }
        }

        public string UserLocation { get; set; }
    }
}
