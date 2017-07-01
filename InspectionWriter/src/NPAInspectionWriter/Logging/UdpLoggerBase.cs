using System;
using NPAInspectionWriter.Services;

namespace NPAInspectionWriter.Logging
{
    public class UdpLoggerBase : ILog
    {
        protected IUdpLoggerConfiguration _configuration { get; }
        protected IUdpMessenger _messenger { get; }

        [Obsolete( "This constructor is Obsolete. Please use the IUdpLoggerConfiguration." )]
        public UdpLoggerBase( string hostOrIp, int port, IUdpMessenger messenger ) :
            this( new LoggerConfiguration() { HostOrIp = hostOrIp, Port = port }, messenger )
        {
        }

        public UdpLoggerBase( IUdpLoggerConfiguration configuration, IUdpMessenger messenger )
        {
            _messenger = messenger;
            _configuration = configuration;
        }

        public virtual void Alert( string format, params object[] args )
        {
            SendMessage( "Alert:\n{0}\n\n", GetFormattedMessage( format, args ) );
        }

        public virtual void Debug( string format, params object[] args )
        {
            SendMessage( "Debug:\n{0}\n\n", GetFormattedMessage( format, args ) );
        }

        public virtual void Error( Exception exception )
        {
            SendMessage( "Error:\n{0}\n\n", exception );
        }

        public virtual void Info( string format, params object[] args )
        {
            SendMessage( "Info:\n{0}\n\n", GetFormattedMessage( format, args ) );
        }

        public virtual void Notice( string format, params object[] args )
        {
            SendMessage( "Notice:\n{0}\n\n", GetFormattedMessage( format, args ) );
        }

        public virtual void Warn( string format, params object[] args )
        {
            SendMessage( "Warn:\n{0}\n\n", GetFormattedMessage( format, args ) );
        }

        protected async void SendMessage( string formattedMessage )
        {
            await _messenger.SendMessage( formattedMessage, _configuration.HostOrIp, _configuration.Port );
        }

        protected void SendMessage( string format, params object[] args )
        {
            SendMessage( string.Format( format, args ) );
        }

        protected void SendMessage( object message )
        {
            SendMessage( message.ToString() );
        }

        /// <summary>
        /// Safely returns formatted message
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected string GetFormattedMessage( string format, params object[] args )
        {
            return args?.Length > 0 ? string.Format( format, args ) : format;
        }

        private class LoggerConfiguration : IUdpLoggerConfiguration
        {
            public string HostOrIp { get; set; }
            public int Port { get; set; }
        }
    }
}
