// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace gw.proto.http
{
    ////////////////////////////////////////////////////////////////////////////////

    public class HttpResponse
    {
        Stream          mStream         = null;
        bool            mHeadersWritten = false;

        public bool     IsClosed        { get; private set; }
        public Encoding Encoding        = Encoding.UTF8;
        public int      Code            = 200;
        public string   Reason          = "OK";

        public Dictionary<string,string> Headers = new Dictionary<string, string>();


        //------------------------------------------------------------------------------

        public HttpResponse( Stream stream )
        {
            IsClosed = false;
            mStream  = stream;
        }


        //------------------------------------------------------------------------------

        public void Response( ResponseCode code, string reason = null )
        {
            Code = (int) code;
            Reason = reason != null ? reason : HttpUtils.CodeToString( code );
        }
            
        public void Close()
        {
            if( mHeadersWritten == false )
            {
                WriteHeaders();
            }

            if( IsClosed == false )
            {
                mStream.Flush();
                mStream.Close();
                mStream.Dispose();
            }

            IsClosed = true;
        }

        public void Abort()
        {
            mStream.Close();
            mStream.Dispose();
            IsClosed = true;
        }


        //------------------------------------------------------------------------------

        public void Send( string str )
        {
            Send( Encoding.GetBytes( str ) );
        }

        public void Send( byte[] bytes )
        {
            Headers[ "Connection" ] = "close";
            WriteHeaders();

            mStream.Write( bytes, 0, bytes.Length );

            Close();
        }

        public void SendAsync( byte[] bytes )
        {
            Headers[ "Connection" ] = "close";
            WriteHeaders();

            mStream.BeginRead( bytes, 0, bytes.Length, OnSent, this );
        }

        void OnSent( IAsyncResult res )
        {
            mStream.EndWrite( res );
            Close();
        }


        //------------------------------------------------------------------------------

        public void WriteHeaders()
        {
            if( mHeadersWritten || IsClosed )
            {
                throw new HttpResponseException( ResponseCode.InternalServerError );
            }

            mHeadersWritten = true;

            var str = new StringBuilder();

            str.AppendFormat( "HTTP/1.1 {0} {1}\r\n", Code, Reason );

            foreach( var item in Headers )
            {
                str.AppendFormat( "{0}: {1}\r\n", item.Key, item.Value );
            }

            str.Append( "\r\n" );

            var bytes = Encoding.ASCII.GetBytes( str.ToString() );
            mStream.Write( bytes, 0, bytes.Length );
        }


        //------------------------------------------------------------------------------

        public void Redirect( string url )
        {
            Code   = (int) ResponseCode.MovedPermanently;
            Reason = HttpUtils.CodeToString( ResponseCode.MovedPermanently );

            Headers[ "Location" ] = url;
            Headers[ "Connection" ] = "close";

            WriteHeaders();
            Close();
        }


        public void Reject( ResponseCode code )
        {
            Code   = (int) code;
            Reason = HttpUtils.CodeToString( code );

            Headers[ "Connection" ] = "close";

            WriteHeaders();
            Close();
        }
    }
}

#endif
