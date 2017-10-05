// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace gw.proto.utils
{
    // TODO: add stack traces as output format

    public class LogEvent : EventArgs
    {
        [Flags]
        public enum Severity
        {
            None = 0x00,
            Info = 0x01,
            Warning = 0x02,
            Error = 0x04,
            All = 0x07,
        }

        public Severity Type { get; private set; }
        public String Message { get; private set; }

        public LogEvent( Severity type, string text )
        {
            Type = type;
            Message = text;
        }
    }


    public class Logger
    {
        public delegate void LogMessage( object sender, LogEvent e );

        public event LogMessage     OnMessage;
        public LogEvent.Severity    Show        = LogEvent.Severity.All;
        public string               Format      = "[%s] %c - %m";

        [Conditional( "GW_LOGGER" )]
        public void Message( object sender, LogEvent.Severity severity, string msg )
        {
            if( OnMessage != null )
            {
                OnMessage( sender, new LogEvent( severity, msg ) );
            }
        }

        [Conditional( "GW_LOGGER" )]
        public void Print( string msg, params object[] args )
        {
            if( msg != null && OnMessage != null && ( Show & LogEvent.Severity.Info ) != 0 )
            {
                OnMessage( null, new LogEvent( LogEvent.Severity.Info, FormatMsg( LogEvent.Severity.Info, msg, args ) ) );
            }
        }

        [Conditional( "GW_LOGGER" )]
        public void Warn( string msg, params object[] args )
        {
            if( msg != null && OnMessage != null && ( Show & LogEvent.Severity.Warning ) != 0 )
            {
                OnMessage( null, new LogEvent( LogEvent.Severity.Info, FormatMsg( LogEvent.Severity.Warning, msg, args ) ) );
            }
        }

        [Conditional( "GW_LOGGER" )]
        public void Error( string msg, params object[] args )
        {
            if( msg != null && OnMessage != null && ( Show & LogEvent.Severity.Error ) != 0 )
            {
                OnMessage( null, new LogEvent( LogEvent.Severity.Info, FormatMsg( LogEvent.Severity.Error, msg, args ) ) );
            }
        }


        Regex _formatRegEx = new Regex( "%(.)" );

        private string FormatMsg( LogEvent.Severity severity, string msg, params object[] args )
        {
            if( Format != null )
            {
                var stackTrace = new StackTrace();

                msg = _formatRegEx.Replace( Format, m =>
                {
                    switch( m.Groups[ 1 ].Value )
                    {
                        case "%": return "%";
                        case "s": return severity.ToString().ToLower();
                        case "S": return severity.ToString().ToUpper();
                        case "m": return msg;

                        case "c":
                            var frame   = stackTrace.GetFrame( 2 );
                            var method  = frame.GetMethod();
                            var type    = method.ReflectedType;

                            return type.Namespace + "." + type.Name + "." + method.Name; // + ":" + frame.GetFileLineNumber();
                    }

                    return m.Value;
                } );
            }

            return String.Format( msg, args );
        }
    }
}
