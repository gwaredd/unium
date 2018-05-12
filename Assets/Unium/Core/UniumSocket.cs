// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;

using TinyJson;

using gw.proto.http;
using gw.proto.utils;

namespace gw.unium
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// A class that handles communication over a websocket
    /// 
    /// Parses json messages from the client and dispatches them on the game thread
    /// Also handles "watching variables" and listening to game events
    /// </summary>

    public partial class UniumSocket
    {
        class EventListener
        {
            public Message      Message;
            public EventInfo    EventInfo;
            public Delegate     Delegate;
            public object       Target;

            public EventListener( Message msg, object target, EventInfo eventInfo )
            {
                Message     = msg;
                EventInfo   = eventInfo;
                Target      = target;
                Delegate    = Delegate.CreateDelegate( eventInfo.EventHandlerType, Message, "Reply" );

                // eventInfo.AddEventHandler( Target, Delegate );
                eventInfo.GetAddMethod().Invoke( Target, new [] { Delegate } );

                msg.Info( "bound" );
            }

            public void Unbind()
            {
                if( Message == null )
                {
                    return;
                }

                EventInfo.RemoveEventHandler( Target, Delegate );
                Message.Info( "unbound" );
            }
        }


        //----------------------------------------------------------------------------------------------------
        // internal state

        WebSocket           mSocket         = null;                         // underlying websocket connection
        List<Message>       mMessageQueue   = new List<Message>();          // messages waiting for dispatch
        List<Repeater>      mRepeaters      = new List<Repeater>();         // messages that are repeated periodically (temporal queries)
        List<EventListener> mListeners      = new List<EventListener>();    // event bindings
        Message             mSocketMessage  = new Message();                // special message used to broadcast "socket level" information


        //----------------------------------------------------------------------------------------------------
        // lifetime managment

        public UniumSocket( WebSocket ws )
        {
            mSocket             = ws;
            mSocket.User        = this;
            mSocket.OnMessage   = OnMessage;

            mSocketMessage.id = "socket";
            mSocketMessage.Socket = this;
        }

        public void OnClose()
        {
            mListeners.ForEach( i => i.Unbind() );
            mListeners.Clear();
        }


        //----------------------------------------------------------------------------------------------------
        // parse message and queue for dispatch on the game thread

        void OnMessage( WebSocket ws, byte[] data, bool isText )
        {
            try
            {
                if( isText == false )
                {
                    throw new Exception( "Unable to process binary messages" );
                }

                var json    = Encoding.UTF8.GetString( data );
                var message = json.FromJson< Message >();

                if( message == null || message.q == null )
                {
                    throw new Exception( "Failed to parse message" );
                }

                UniumComponent.Log( string.Format( "[sock:{0}] {1}", mSocket.ID, json ) );

                lock( mMessageQueue )
                {
                    mMessageQueue.Add( message );
                }
            }
            catch( Exception e )
            {
                mSocketMessage.Error( ResponseCode.BadRequest );
                UniumComponent.Warn( string.Format( "[sock:{0}] {1}", mSocket.ID, e.Message ) );
            }
        }


        //----------------------------------------------------------------------------------------------------
        // process any message queue on the game thread

        public void Tick()
        {
            mSocket.Tick();

            if( mRepeaters.Count > 0 )
            {
                foreach( var repeater in mRepeaters )
                {
                    repeater.Tick();
                }

                mRepeaters.RemoveAll( r => r.IsFinished );
            }

            lock( mMessageQueue )
            {
                if( mMessageQueue.Count == 0 )
                {
                    return;
                }

                // dispatch one message per frame

                var msg = mMessageQueue[ 0 ];
                mMessageQueue.RemoveAt( 0 );

                // bind socket

                msg.Socket = this;


                // find route

                var route = Unium.RoutesSocket.Find( msg.q );

                if( route == null || route.Handler == null )
                {
                    msg.Error( ResponseCode.NotFound );
                    return;
                }


                // create request

                var req = new RequestAdapterSocket( msg );


                // queue for repetition?

                if( msg.repeat != null )
                {
                    route.SetCacheContext( req, msg.repeat.cache ? 1 : 0 );
                    mRepeaters.Add( new Repeater( route, req ) );
                }

                // otherwise just dispatch

                else
                {
                    route.Dispatch( req );
                }
            }
        }


        //----------------------------------------------------------------------------------------------------
        // handler interface

        // stop a repeater

        public void Stop( string id )
        {
            var num = mRepeaters.Count;

            if( id == "*" )
            {
                mRepeaters.ForEach( r => r.Cancel() );
                mRepeaters.Clear();
            }
            else
            {
                mRepeaters.ForEach( s => { if( s.ID == id ) { s.Cancel(); } } );
                mRepeaters.RemoveAll( s => s.IsFinished );
            }

            if( mRepeaters.Count == num )
            {
                Send( id, "error", "not found" );
            }
        }


        // bind to an event

        public void Bind( Message msg, object target, EventInfo eventInfo )
        {
            var listener = new EventListener( msg, target, eventInfo );
            mListeners.Add( listener );
        }


        // unbind an event

        public void Unbind( string id )
        {
            var num = mListeners.Count;

            if( id == "*" )
            {
                mListeners.ForEach( i => i.Unbind() );
                mListeners.Clear();
            }
            else
            {
                mListeners.ForEach( i => { if( i.Message.id == id ) { i.Unbind(); } } );
                mListeners.RemoveAll( i => i.Message.id == id );
            }

            if( mListeners.Count == num )
            {
                Send( id, "error", "not found" );
            }
        }


        // ping-pong

        public void Pong()
        {
            mSocketMessage.Reply( @"""pong""" );
        }


        //----------------------------------------------------------------------------------------------------
        // return message format

        void Send( string id, string msg, string data )
        {
            var sb = new StringBuilder();

            sb.Append( "{" );

            if( String.IsNullOrEmpty( id ) == false )
            {
                sb.AppendFormat( @"""id"":{0},", JsonTypeConverters.EscapedString( id ) );
            }

            sb.AppendFormat( @"""{0}"":{1}", msg, data );

            sb.Append( "}" );

            mSocket.SendAsync( sb.ToString() );
        }
    }
}

#endif
