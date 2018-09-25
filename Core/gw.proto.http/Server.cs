// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using gw.proto.utils;
using System.Collections.Generic;

namespace gw.proto.http
{
    ////////////////////////////////////////////////////////////////////////////////
    // NB: not thread safe!

    public class Dispatcher
    {
        public delegate void WebRequestHandler( HttpRequest req );
        public delegate void WebSocketHandler( WebSocket sender );

        public WebRequestHandler        OnWebRequest    = null;
        public WebSocketHandler         OnSocketRequest = null;
        public WebSocketHandler         OnSocketOpen    = null;
        public WebSocketHandler         OnSocketClose   = null;

        public void SocketOpen( WebSocket ws )
        {
            if( OnSocketOpen != null )
            {
                OnSocketOpen( ws );
            }
        }

        public void SocketClose( WebSocket ws )
        {
            if( OnSocketClose != null )
            {
                OnSocketClose( ws );
            }
        }
    }

    public class Settings
    {
        public IPAddress    Address     = IPAddress.Any;
        public int          Port        = 8342;
        public int          Timeout     = 3 * 1000; // milliseconds
        public int          PostLimit   = 1 * 1024 * 1024; // 1 Mb
    }


    ////////////////////////////////////////////////////////////////////////////////
    //

    public class Server
    {
        public Settings     Settings    { get; private set; }
        public Dispatcher   Dispatcher  { get; private set; }

        public string       Address     { get { return mListener != null ? mListener.LocalEndpoint.ToString() : null; } }
        public bool         IsListening { get { return mListener != null; } }

        protected TcpListener mListener = null;
        protected List<WebSocket> mWebSockets = new List<WebSocket>();

        public Server( Settings settings = null, Dispatcher dispatcher = null )
        {
            Settings    = settings   != null ? settings   : new Settings();
            Dispatcher  = dispatcher != null ? dispatcher : new Dispatcher();

            Dispatcher.OnSocketOpen     = OnSocketOpen;
            Dispatcher.OnSocketClose    = OnSocketClose;

            HttpRequest.LimitPostSize = Settings.PostLimit;
        }

        public Server( int port, IPAddress address = null, Dispatcher dispatch = null )
            : this( null, dispatch )
        {
            Settings.Port = port;

            if( address != null )
            {
                Settings.Address = address;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////

        public bool Start()
        {
            try
            {
                if( mListener != null )
                {
                    Util.Error( "Already listening on {0}", mListener.LocalEndpoint.ToString() );
                    return false;
                }

                mListener = new TcpListener( Settings.Address, Settings.Port );
                mListener.Start();
                mListener.BeginAcceptTcpClient( new AsyncCallback( OnAcceptConnection ), this );

                Util.Print( "Listening on {0}", mListener.LocalEndpoint.ToString() );

                return true;
            }
            catch( Exception e )
            {
                Util.Error( e.ToString() );
                mListener = null;
            }

            return false;
        }


        ////////////////////////////////////////////////////////////////////////////////
        ///

        public void Stop()
        {
            if( mListener == null )
            {
                return;
            }

            mListener.Stop();
            mListener = null;

            Util.Print( "Server closing down" );

            foreach( var ws in mWebSockets )
            {
                ws.Close( WebSocketStatus.GoingAway );
            }

            mWebSockets.Clear();
        }


        ////////////////////////////////////////////////////////////////////////////////
        ///

        public void Tick()
        {
            lock( mWebSockets )
            {
                foreach( var ws in mWebSockets )
                {
                    ws.Tick();
                }
            }
        }

        void OnSocketOpen( WebSocket ws )
        {
            lock( mWebSockets )
            {
                mWebSockets.Add( ws );
            }
        }

        void OnSocketClose( WebSocket ws )
        {
            lock( mWebSockets )
            {
                mWebSockets.Remove( ws );
            }
        }

        ////////////////////////////////////////////////////////////////////////////////
        // asynchronous accept - happens on ThreadPool

        protected static void OnAcceptConnection( IAsyncResult result )
        {
            var server = result.AsyncState as Server;

            try
            {
                var tcpClient = server.mListener.EndAcceptTcpClient( result );

                if( tcpClient != null )
                {
                    ThreadPool.QueueUserWorkItem( (object c) => (c as Client).OnConnect(), new Client( server.Dispatcher, tcpClient ) );
                }
                else
                {
                    Util.Warn( "Failed to accept client connection" );
                }

                server.mListener.BeginAcceptTcpClient( new AsyncCallback( OnAcceptConnection ), server );
            }
            catch( Exception e )
            {
                Util.Error( e.ToString() );
            }
        }
    }
}

#endif
