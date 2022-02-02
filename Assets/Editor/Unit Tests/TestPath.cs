// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using NUnit.Framework;
using gw.gql;

////////////////////////////////////////////////////////////////////////////////////////////////////

public class TestPath
{
    //----------------------------------------------------------------------------------------------------

    [Test]
    public void Basic()
    {
        Path p;

        // quick tests
        p = new Path( "/root" );
        Assert.AreEqual( 1, p.Length );
        Assert.AreEqual( "root", p[ 0 ].Select );
        Assert.AreEqual( Query.Action.Get, p.Action );

        p = new Path( "/root/obj[3].attr" );
        Assert.AreEqual( 3, p.Length );

        p = new Path( "/root/obj.attr" );
        Assert.AreEqual( 3, p.Length );

        Assert.AreEqual( "root", p[ 0 ].Select );
        Assert.AreEqual( "obj",  p[ 1 ].Select );
        Assert.AreEqual( "attr", p[ 2 ].Select );

        Assert.IsTrue( p[ 0 ].NodeType == Path.Segment.Type.Children );
        Assert.IsTrue( p[ 1 ].NodeType == Path.Segment.Type.Children );
        Assert.IsTrue( p[ 2 ].NodeType == Path.Segment.Type.Attribute );

        // set value
        p = new Path( "/root/obj.attr.val=abc,def" );
        Assert.AreEqual( 3, p.Length );
        Assert.AreEqual( Query.Action.Set, p.Action );
        Assert.AreEqual( "val", p.Target );
        Assert.AreEqual( 1, p.Arguments.Length );
        Assert.AreEqual( "abc,def", p.Arguments[ 0 ] );

        // function call
        p = new Path( "/root/obj.attr.func(123)" );
        Assert.AreEqual( 3, p.Length );
        Assert.AreEqual( Query.Action.Invoke, p.Action );
        Assert.AreEqual( "func", p.Target );
        Assert.AreEqual( 1, p.Arguments.Length );
        Assert.AreEqual( "123", p.Arguments[ 0 ] );
    }

    //----------------------------------------------------------------------------------------------------

    [Test]
    public void Args()
    {
        Assert.AreEqual( new string[] { }, Path.SplitArgs( "" ) );
        Assert.AreEqual( new string[] { }, Path.SplitArgs( "   " ) );

        Assert.AreEqual( new string[] { "a" }, Path.SplitArgs( "a" ) );
        Assert.AreEqual( new string[] { "a" }, Path.SplitArgs( "  a  " ) );
        Assert.AreEqual( new string[] { "a", "b", "c" }, Path.SplitArgs( "a,b,c" ) );
        Assert.AreEqual( new string[] { "a", "b", "c" }, Path.SplitArgs( " a , b , c " ) );
        Assert.AreEqual( new string[] { "12.3", "45", "abc" }, Path.SplitArgs( "12.3,45,abc" ) );

        Assert.AreEqual( new string[] { "a", "b", "c" }, Path.SplitArgs( " \"a\" , \"b\", \"c\" " ) );
        Assert.AreEqual( new string[] { "ab,c, 7", "0" }, Path.SplitArgs( "\"ab,c, 7\", 0" ) );
        Assert.AreEqual( new string[] { " ab,c\", 7 ", "0" }, Path.SplitArgs( "\" ab,c\\\", 7 \", 0" ) );

        Assert.AreEqual( new string[] { "{}" }, Path.SplitArgs( " {} " ) );
        Assert.AreEqual( new string[] { "{\"name\":\"g\",\"v\":{x:1,y:1,z:1}}" }, Path.SplitArgs( " {\"name\":\"g\",\"v\":{x:1,y:1,z:1}} " ) );
        Assert.AreEqual( new string[] { "{\"name\":\"g\",\"v\":{x:1,y:1,z:1}}", "123" }, Path.SplitArgs( " {\"name\":\"g\",\"v\":{x:1,y:1,z:1}} , 123 " ) );

        Assert.AreEqual( new string[] { "[{x:1},{a:1,b:1},{c:[1,2,3]}]" }, Path.SplitArgs( "[{x:1},{a:1,b:1},{c:[1,2,3]}]" ) );
    }
}

#endif
