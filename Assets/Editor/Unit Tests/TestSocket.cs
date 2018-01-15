// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using NUnit.Framework;
using gw.proto.http;
using gw.unium;


////////////////////////////////////////////////////////////////////////////////////////////////////

public class TestSocket
{
    class MockWebSocket : WebSocket
    {
        public MockWebSocket() : base( new HttpRequest() ) { }

        public string Expect = null;

        public override void SendAsync( string json )
        {
            Assert.AreEqual( Expect, json );
        }
    }

    //----------------------------------------------------------------------------------------------------

    [Test]
    public void SendDataRaw()
    {
        var rcv = new MockWebSocket();
        var msg = new UniumSocket.Message();

        msg.Socket = new UniumSocket( rcv );

        // no id

        rcv.Expect = @"{""data"":{""x"":1,""y"":2,""z"":3}}";
        msg.Reply( @"{""x"":1,""y"":2,""z"":3}" );

        // id

        msg.id = "id";

        rcv.Expect = @"{""id"":""id"",""data"":{""x"":1,""y"":2,""z"":3}}";
        msg.Reply( @"{""x"":1,""y"":2,""z"":3}" );

        // null

        rcv.Expect = @"{""id"":""id"",""data"":null}";
        msg.Reply( null as string );

        rcv.Expect = @"{""id"":""id"",""data"":null}";
        msg.Reply( "" );
    }

    //----------------------------------------------------------------------------------------------------

    [Test]
    public void SendDataObject()
    {
        var rcv = new MockWebSocket();
        var msg = new UniumSocket.Message();

        msg.Socket = new UniumSocket( rcv );

        int n = 5;

        // no id

        rcv.Expect = @"{""data"":5}";
        msg.Reply( n );

        // id

        msg.id = "id";

        rcv.Expect = @"{""id"":""id"",""data"":5}";
        msg.Reply( n );

        // objects

        msg.id = null;

        rcv.Expect = @"{""data"":{""name"":""name"",""value"":7}}";
        msg.Reply( new { name = "name", value = 7 } );

        var o = new { name = "name", child = new { c = "a" } };

        rcv.Expect = @"{""data"":{""name"":""name"",""child"":{""c"":""a""}}}";
        msg.Reply( o );

        //        rcv.Expect = @"{""data"":{""name"":""name"",""child"":{""c"":""a""}}}";
        //        msg.Reply( o, true );

        rcv.Expect = @"{""data"":null}";
        msg.Reply( null as object );

        // value types

        float f = 1.0f;
        rcv.Expect = @"{""data"":1}";
        msg.Reply( f );
    }

    //----------------------------------------------------------------------------------------------------

    [Test]
    public void SendErrors()
    {
        var rcv = new MockWebSocket();
        var msg = new UniumSocket.Message();

        msg.Socket = new UniumSocket( rcv );

        // no id

        rcv.Expect = @"{""error"":""Bad Request""}";
        msg.Error( ResponseCode.BadRequest );

        // general errors

        rcv.Expect = @"{""id"":""err"",""error"":""Bad Request""}";
        msg.id = "err";
        msg.Error( ResponseCode.BadRequest );

        rcv.Expect = @"{""id"":""err"",""error"":""Not Found""}";
        msg.id = "err";
        msg.Error( ResponseCode.NotFound );

        rcv.Expect = @"{""id"":""err"",""error"":""Internal Server Error""}";
        msg.id = "err";
        msg.Error( ResponseCode.InternalServerError );

        // json in id

        rcv.Expect = @"{""id"":"":{}'"",""error"":""Bad Request""}";
        msg.id = ":{}'";
        msg.Error( ResponseCode.BadRequest );

        rcv.Expect = @"{""id"":""\""name\"":\""ok\"""",""error"":""Bad Request""}";
        msg.id = @"""name"":""ok""";
        msg.Error( ResponseCode.BadRequest );
    }

    //----------------------------------------------------------------------------------------------------

    [Test]
    public void SendMessage()
    {
        var rcv = new MockWebSocket();
        var msg = new UniumSocket.Message();

        msg.Socket = new UniumSocket( rcv );

        // no id

        rcv.Expect = @"{""info"":""hello""}";
        msg.id = null;
        msg.Info( "hello" );

        // with id

        rcv.Expect = @"{""id"":""msg"",""info"":""hello""}";
        msg.id = "msg";
        msg.Info( "hello" );

        // JSON unfriendly

        rcv.Expect = @"{""id"":""\"",x:{}\"""",""info"":""\"",x:{}\""""}";
        msg.id = @""",x:{}""";
        msg.Info( @""",x:{}""" );

        // no message

        rcv.Expect = @"{""info"":null}";
        msg.id = null;
        msg.Info( null );
    }
}

#endif
