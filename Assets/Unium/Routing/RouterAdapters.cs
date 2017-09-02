// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

using System;
using gw.proto.http;
using UnityEngine;

namespace gw.unium
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public abstract class RequestAdapter
    {
        public virtual  string  Path                                { get { return null; } }
        public virtual  byte[]  Body                                { get { return null; } }

        public abstract void Reject( ResponseCode code );
        public abstract void Redirect( string url );
        public abstract void Respond( string data );
        public abstract void Respond( byte[] data );

        public abstract void SetContentType( string mimetype );

        public object CachedContext = null;
    }

#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class RequestAdapterHTTP : RequestAdapter
    {
        HttpRequest mRequest;
        string      mPath;

        public RequestAdapterHTTP( HttpRequest req )                { mRequest = req; mPath = WWW.UnEscapeURL( req.URL ); }

        public override String  Path                                { get { return mPath; } }
        public override byte[]  Body                                { get { return mRequest.Method == "POST" ? mRequest.Body : null; } }

        public override void Reject( ResponseCode code )            { mRequest.Reject( code ); }
        public override void Redirect( string url )                 { mRequest.Redirect( url ); }
        public override void Respond( string data )                 { mRequest.Send( data ); }
        public override void Respond( byte[] data )                 { mRequest.Send( data ); }

        public override void SetContentType( string mimetype )      { mRequest.Response.Headers[ "Content-Type" ] = mimetype; }
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // adapter for router dispatch

    public class RequestAdapterSocket : RequestAdapter
    {
        UniumSocket.Message mMessage;

        public RequestAdapterSocket( UniumSocket.Message msg )
        {
            mMessage = msg;
        }

        public string               ID                          { get { return mMessage.id; } }
        public UniumSocket.Message  Message                     { get { return mMessage; } }
        public UniumSocket          Socket                      { get { return mMessage.Socket; } }
        public bool                 Rejected                    { get; private set; }


        public override String Path                             { get { return mMessage.q; } }
        public override byte[] Body                             { get { return null; } }

        public override void SetContentType( string mimetype )  {}

        public override void Reject( ResponseCode code )        { mMessage.Error( code ); Rejected = true; }
        public override void Redirect( string url )             { mMessage.Error( ResponseCode.MovedPermanently ); Rejected = true; }
        public override void Respond( string data )             { mMessage.Reply( data ); }
        public override void Respond( byte[] data )             { throw new NotImplementedException(); } // binary data

        public void Info( string msg )                          { mMessage.Info( msg ); }
    }

#endif
}

