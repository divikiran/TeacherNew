﻿using System;
using System.Diagnostics;
using NPAInspectionWriter.Models;
using NPAInspectionWriter.iOS.Extensions;
using NPAInspectionWriter.Logging;
using NPAInspectionWriter.Services;
using NPAInspectionWriter.Services;

namespace NPAInspectionWriter.iOS.IoC
{
    public class InspectionWriterUdpLogger : UdpLoggerBase, ISysLogLogger, ILog
    {
        const string messageFormat = "{0}: Priority {1} -\n {2}\n\n";
        ILogService _logService { get; }

        public InspectionWriterUdpLogger( IUdpLoggerConfiguration config, IUdpMessenger messenger, ILogService logService ) : base( config, messenger )
        {
            _logService = logService;
        }

        public void Log( string format, Level level, params object[] args )
        {
            if( args?.Length > 0 )
                Log( string.Format( format, args ), level );
            else
                Log( format, level );
        }

        #region ISysLogLogger

        public void Log( string message, Level level )
        {
            Log( message, level, Facility.Local0 );
        }

        public void Log( string message, Level level, Facility facility )
        {
#if !DEBUG
            if( level == Level.Error )
            {
                _logService.LogErrorAsync( new Error()
                {
                    Description = $"{level}",
                    Message = message,
                    StackTrace = new StackTrace().ToString()
                } );
            }
#endif

            if( PlatformSettings.Current.LoggingLevel < 0
                || string.IsNullOrWhiteSpace( message ) ) return;

            var loggingLevel = PlatformSettings.Current.LoggingLevel;
            if( loggingLevel == 0 || loggingLevel >= ( int )level )
                SendMessage( new SysLogMessage( message, level, facility ) );
        }

        #endregion

        #region ILogger Facade

        public void Log( string message)
        {
            Log( message );
        }

        #endregion

        #region ILog

        /// <summary>
        /// Logs the message as an alert.
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        public override void Alert( string format, params object[] args )
        {
            Log( format, Level.Alert, args );
        }

        /// <summary>
        /// Logs the message as a debug message.
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        public override void Debug( string format, params object[] args )
        {
            Log( format, Level.Debug, args );
        }

        /// <summary>
        /// Logs the message as info.
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        public override void Info( string format, params object[] args )
        {
            Log( format, Level.Information, args );
        }

        /// <summary>
        /// Logs the message as a notice.
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        public override void Notice( string format, params object[] args )
        {
            Log( format, Level.Notice, args );
        }

        /// <summary>
        /// Logs the message as a warning.
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        public override void Warn( string format, params object[] args )
        {
            Log( format, Level.Warning, args );
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public override void Error( Exception exception )
        {
            Log( exception.ToString(), Level.Error );
        }

        #endregion
    }
}
