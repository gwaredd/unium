// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;

using gw.proto.utils;


////////////////////////////////////////////////////////////////////////////////////////////////////

public class TestReflection
{
    enum TestEnum { First, Second, Third }

    [Test]
    public void PrimitiveTypes()
    {
        Assert.AreEqual( "null", JsonReflector.Reflect( null ) );
        Assert.AreEqual( "true", JsonReflector.Reflect( true ) );
        Assert.AreEqual( "7", JsonReflector.Reflect( 7 ) );
        Assert.AreEqual( "3.14", JsonReflector.Reflect( 3.14 ) );
        Assert.AreEqual( @"""c""", JsonReflector.Reflect( 'c' ) );
        Assert.AreEqual( @"""hello""", JsonReflector.Reflect( "hello" ) );

        // edge cases

        Assert.AreEqual( int.MaxValue.ToString(), JsonReflector.Reflect( int.MaxValue ) );
        Assert.AreEqual( @"""':\\{}\""""", JsonReflector.Reflect( @"':\{}""" ) );
    }

    [Test]
    public void EnumsAndValueTypes()
    {
        Assert.AreEqual( @"""Second""", JsonReflector.Reflect( TestEnum.Second ) );
        Assert.AreEqual( @"null", JsonReflector.Reflect( (TestEnum) 100 ) );

        Assert.AreEqual( @"{""x"":3,""y"":5,""z"":100}", JsonReflector.Reflect( new Vector3( 3.0f, 5.0f, 100.0f ) ) );
    }

    [Test]
    public void Objects()
    {
        // object
        Assert.AreEqual( @"{""a"":6,""b"":""hello""}", JsonReflector.Reflect( new { a = 6, b = "hello" } ) );

        // array
        Assert.AreEqual( @"[1,2,3,4]", JsonReflector.Reflect( new int[] { 1, 2, 3, 4 } ) );

        // list
        Assert.AreEqual( @"[1,2,3,4]", JsonReflector.Reflect( new List<int>() { 1, 2, 3, 4 } ) );

        // dictionary

        var dict = new Dictionary<string,int>() {
            { "k1", 1 },
            { "k2", 2 },
            { "k3", 3 },
        };

        Assert.AreEqual( @"{""k1"":1,""k2"":2,""k3"":3}", JsonReflector.Reflect( dict ) );

        var nonStringKeys = new Dictionary<object,int>() {
            { new { x = 1 }, 1 },
            { new { z = 1 }, 2 },
            { new { fish = 1, z = 2 }, 3 },
        };

        Assert.AreEqual( @"{""{ x = 1 }"":1,""{ z = 1 }"":2,""{ fish = 1, z = 2 }"":3}", JsonReflector.Reflect( nonStringKeys ) );
    }


    class A
    {

    }

    class B : A
    {

    }

    public class SerialiseType : JsonSerialiser
    {
        override public string Convert( object o )
        {
            return o.GetType().ToString();
        }
    }

    [Test]
    public void Converters()
    {
        JsonSerialiser aSerializer = new SerialiseType();
        JsonReflector.Add(typeof(A), aSerializer);
        Assert.AreEqual( @"TestReflection+A", JsonReflector.Reflect( new A() ));
        Assert.AreEqual( @"TestReflection+B", JsonReflector.Reflect( new B() ));
    }
}


#endif
