// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using NUnit.Framework;
using TinyJson;
using gw.unium;

////////////////////////////////////////////////////////////////////////////////////////////////////

public class TestSocketMessages
{
    [Test]
    public void ParsingMessages()
    {
        UniumSocket.Message msg;
        var repeatDefault = new UniumSocket.Message.Repeat();


        // basic

        msg = @"{""id"":""someid"",""q"":""/q/scene/etc""}".FromJson<UniumSocket.Message>();
        Assert.AreEqual( msg.q, "/q/scene/etc" );
        Assert.AreEqual( msg.id, "someid" );
        Assert.IsNull( msg.repeat );


        // no id

        msg = @"{""q"":""/q/scene/etc""}".FromJson<UniumSocket.Message>();
        Assert.AreEqual( msg.q, "/q/scene/etc" );
        Assert.IsNull( msg.id );
        Assert.IsNull( msg.repeat );

        // repeater

        msg = @"{""q"":""/q/"",""repeat"":{}}".FromJson<UniumSocket.Message>();
        Assert.AreEqual( msg.q, "/q/" );
        Assert.IsNull( msg.id );
        Assert.IsNotNull( msg.repeat );

        Assert.AreEqual( repeatDefault.skip, msg.repeat.skip );
        Assert.AreEqual( repeatDefault.samples, msg.repeat.samples );
        Assert.AreEqual( repeatDefault.freq, msg.repeat.freq );
        Assert.AreEqual( repeatDefault.cache, msg.repeat.cache );


        msg = @"{""q"":""/q/"",""repeat"":{""skip"":5}}".FromJson<UniumSocket.Message>();
        Assert.AreEqual( msg.q, "/q/" );
        Assert.IsNull( msg.id );
        Assert.IsNotNull( msg.repeat );

        Assert.AreEqual( 5, msg.repeat.skip );
        Assert.AreEqual( repeatDefault.samples, msg.repeat.samples );
        Assert.AreEqual( repeatDefault.freq, msg.repeat.freq );
        Assert.AreEqual( repeatDefault.cache, msg.repeat.cache );


        msg = @"{""q"":""/q/"",""repeat"":{""skip"":4,""samples"":10,""freq"":3.4, ""batch"":true, ""cache"":true, ""ignore"":7}}".FromJson<UniumSocket.Message>();
        Assert.AreEqual( msg.q, "/q/" );
        Assert.IsNull( msg.id );
        Assert.IsNotNull( msg.repeat );

        Assert.AreEqual( 4, msg.repeat.skip );
        Assert.AreEqual( 10, msg.repeat.samples );
        Assert.AreEqual( 3.4f, msg.repeat.freq );
        Assert.AreEqual( true, msg.repeat.cache );
    }
}

#endif
