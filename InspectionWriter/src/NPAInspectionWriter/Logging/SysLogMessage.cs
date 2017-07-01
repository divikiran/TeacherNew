using System;
using NPAInspectionWriter.Services;
using Xamarin.Forms;

namespace NPAInspectionWriter.Logging
{
    public class SysLogMessage
    {
        public SysLogMessage()
        {
            Level = Level.Debug;
            Facility = Facility.Local0;
        }

        public SysLogMessage( string message, Level level, Facility facility = Facility.Daemon )
        {
            Message = message;
            Facility = facility;
            Level = level;
        }

        public Facility Facility { get; set; }

        public Level Level { get; set; }

        public string Message { get; set; }

        public DateTime TimeStamp { get; } = DateTime.Now;

        public string HostName { get { return _networkServices.LocalHostName(); } }

        public string DeviceIP { get { return _networkServices.GetDeviceIP(); } }

        private INetwork _networkServices { get; } = DependencyService.Get<INetwork>();

        public override string ToString()
        {
            int priority = ( int )Facility * 8 + ( int )Level;

            return $"<{priority}>{DateTime.Now.ToString( "MMM dd HH:mm:ss" )} {HostName} {DeviceIP} NPA: {Message}";
        }
    }
}
