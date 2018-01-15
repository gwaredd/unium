// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

using gw.gql;


////////////////////////////////////////////////////////////////////////////////////////////////////

public class TestQuery
{
    class TestObject
    {
        // properties

        public TestObject() { prop = 0; priv = 0; }

        public int      v       = 0;
        public float    f       = 3.141f;
        public object   o       = new { x = 10 };
        public int prop { get; set; }
        public readonly int priv;

        // functions to call

        public int Seven() { return 7; }
        public int Add( int a, int b ) { return a + b; }


        // recursive find ...

        public string   name = null;
        public Interpreter.Child[] children = null;

        public TestObject( string _name, TestObject[] _children = null )
        {
            name = _name;

            if( _children != null )
            {
                int i = 0;
                children = new Interpreter.Child[ _children.Length ];

                foreach( var c in _children )
                {
                    children[ i++ ] = new Interpreter.Child( c.name, c );
                }
            }
        }
    }

    public class InterpreterTestObject : InterpreterDefault
    {
        override public Child[] Children( object obj )
        {
            return ( obj as TestObject ).children;
        }
    }


    //----------------------------------------------------------------------------------------------------

    [SetUp]
    public void Setup()
    {
        Interpreters.Add( typeof( TestObject ), new InterpreterTestObject() );
    }

    [TearDown]
    public void TearDown()
    {
        Interpreters.Remove( typeof( TestObject ) );
    }



    //----------------------------------------------------------------------------------------------------

    [Test]
    public void ShouldGetValues()
    {
        var root = new Dictionary<string,object>() {
            { "a", new { name = "childA", v = new { a = 3 } } },
            { "b", new { name = "childB", v = new { a = 4 } } },
            { "c", new { name = "childC", v = new { a = 5 } } },
        };

        List<object> r;


        // query

        r = new Query( "/a.name", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.AreEqual( r[ 0 ], "childA" );

        r = new Query( "/*.name", root ).Select().Execute();
        Assert.AreEqual( 3, r.Count );
        Assert.AreEqual( r[ 0 ], "childA" );
        Assert.AreEqual( r[ 1 ], "childB" );
        Assert.AreEqual( r[ 2 ], "childC" );

        r = new Query( "/*[2].name", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.AreEqual( r[ 0 ], "childC" );


        // where clause

        r = new Query( "/*[name='childB'].name", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.AreEqual( r[ 0 ], "childB" );

        r = new Query( "/*[v.a>=4].v.a", root ).Select().Execute();
        Assert.AreEqual( 2, r.Count );
        Assert.AreEqual( r[ 0 ], 4 );
        Assert.AreEqual( r[ 1 ], 5 );


        // not found

        r = new Query( "/d.name", root ).Select().Execute();
        Assert.AreEqual( 0, r.Count );

        r = new Query( "/a.x", root ).Select().Execute();
        Assert.AreEqual( 0, r.Count );
    }


    //----------------------------------------------------------------------------------------------------

    [Test]
    public void ShouldSetValues()
    {
        var root = new Dictionary<string,TestObject>() {
            { "a", new TestObject() },
            { "b", new TestObject() },
        };

        List<object> r;

        // set various

        r = new Query( "/a.v=2", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.AreEqual( r[ 0 ], 2 );

        r = new Query( "/a.prop=3", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.AreEqual( r[ 0 ], 3 );

        r = new Query( "/a.o=fish", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.AreEqual( typeof( string ), r[ 0 ].GetType() );
        Assert.AreEqual( "fish", r[ 0 ] );

        // set multiple

        r = new Query( "/*.v=5", root ).Select().Execute();
        Assert.AreEqual( 2, r.Count );
        Assert.AreEqual( 5, root[ "a" ].v );
        Assert.AreEqual( 5, root[ "b" ].v );

        // not found

        r = new Query( "/c.v=5", root ).Select().Execute();
        Assert.AreEqual( 0, r.Count );

        r = new Query( "/a.x=5", root ).Select().Execute();
        Assert.AreEqual( 0, r.Count );

        // bad params

        r = new Query( "/a.v=fish", root ).Select().Execute();
        Assert.AreEqual( 0, r.Count );

        r = new Query( "/a.f=fish", root ).Select().Execute();
        Assert.AreEqual( 0, r.Count );
    }


    //----------------------------------------------------------------------------------------------------

    [Test]
    public void ShouldInvokeFunctions()
    {
        var root = new Dictionary<string,TestObject>() {
            { "a", new TestObject() },
            { "b", new TestObject() },
        };

        List<object> r;

        // invoke 1

        r = new Query( "/a.Seven()", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.AreEqual( r[ 0 ], 7 );

        // invoke many

        r = new Query( "/*.Seven()", root ).Select().Execute();
        Assert.AreEqual( 2, r.Count );
        Assert.AreEqual( r[ 0 ], 7 );
        Assert.AreEqual( r[ 1 ], 7 );

        // invoke params

        r = new Query( "/*.Add(4,7)", root ).Select().Execute();
        Assert.AreEqual( 2, r.Count );
        Assert.AreEqual( r[ 0 ], 11 );
        Assert.AreEqual( r[ 1 ], 11 );

        // not found

        r = new Query( "/c.Add(4,7)", root ).Select().Execute();
        Assert.AreEqual( 0, r.Count );

        // bad params

        r = new Query( "/a.Add(fish,7)", root ).Select().Execute();
        Assert.AreEqual( 0, r.Count );
    }


    //----------------------------------------------------------------------------------------------------

    [Test]
    public void ShouldBeAbleToRecursivelyFindNodes()
    {
        var root = new Dictionary<string,object>()
        {
            {
                "a", new TestObject( "a", new TestObject[] {
                        new TestObject( "aa", new TestObject[] {
                            new TestObject( "aaa" ),
                            new TestObject( "bbb" ),
                            new TestObject( "ccc", new TestObject[] { new TestObject( "dddd" ) } ),
                            new TestObject( "dddd" ),
                        }),
                        new TestObject( "bb", new TestObject[] {
                            new TestObject( "aaa" ),
                            new TestObject( "bbb" ),
                            new TestObject( "ccc", new TestObject[] { new TestObject( "dddd", new TestObject[] { new TestObject( "dddd" ) } ) } ),
                        }),
                        new TestObject( "cc" ),
                    }
                )
            },
            {
                "b", new TestObject( "b", new TestObject[] {
                        new TestObject( "aa", new TestObject[] {
                            new TestObject( "aaa" ),
                            new TestObject( "bbb" ),
                            new TestObject( "ccc", new TestObject[] { new TestObject( "dddd" ) } ),
                        }),
                        new TestObject( "bb", new TestObject[] {
                            new TestObject( "aaa" ),
                            new TestObject( "bbb" ),
                            new TestObject( "ccc", new TestObject[] { new TestObject( "dddd" ) } ),
                        }),
                        new TestObject( "cc" ),
                    }
                )
            },
            {
                "c", new TestObject( "c"  )
            },
        };

        List<object> r;


        // basic queries

        r = new Query( "/a/aa.name", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.AreEqual( r[ 0 ], "aa" );

        r = new Query( "/*/aa.name", root ).Select().Execute();
        Assert.AreEqual( 2, r.Count );
        Assert.AreEqual( r[ 1 ], "aa" );

        r = new Query( "/a/bb/ccc/dddd.name", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.AreEqual( r[ 0 ], "dddd" );

        r = new Query( "/a/*[name='bb'].name", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.AreEqual( r[ 0 ], "bb" );

        //        r = new Query( "/a/[last()].name", root ).Select().Execute();
        //        Assert.AreEqual( 1, r.Count );
        //        Assert.AreEqual( r[ 0 ], "cc" );
        //
        //        r = new Query( "/a/[last()-1].name", root ).Select().Execute();
        //        Assert.AreEqual( 1, r.Count );
        //        Assert.AreEqual( r[ 0 ], "bb" );

        // basic recursive find

        r = new Query( "/a//dddd", root ).Select().Execute();
        Assert.AreEqual( 4, r.Count );

        r = new Query( "//dddd", root ).Select().Execute();
        Assert.AreEqual( 6, r.Count );

        r = new Query( "//[name='ccc']/dddd.name", root ).Select().Execute();
        Assert.AreEqual( 4, r.Count );


        // not found

        r = new Query( "a//ee", root ).Select().Execute();
        Assert.AreEqual( 0, r.Count );

        r = new Query( "//[name='bbb']/dddd", root ).Select().Execute();
        Assert.AreEqual( 0, r.Count );


        // mutiple finds

        r = new Query( "//bb//dddd", root ).Select().Execute();
        Assert.AreEqual( 3, r.Count );

        r = new Query( "//*//dddd", root ).Select().Execute(); // NB: finds dddd multiple times as multiple roots work here
        Assert.AreEqual( 18, r.Count );

        r = new Query( "////////dddd", root ).Select().Execute();
        Assert.AreEqual( 6, r.Count );

        r = new Query( "//dddd////", root ).Select().Execute();
        Assert.AreEqual( 6, r.Count );


        // actions

        r = new Query( "/a//dddd.v=365", root ).Select().Execute();
        Assert.AreEqual( 4, r.Count );
        Assert.AreEqual( r[ 0 ], 365 );

        r = new Query( "/a//dddd.Add(3,4)", root ).Select().Execute();
        Assert.AreEqual( 4, r.Count );
        Assert.AreEqual( r[ 0 ], 7 );


        // bad formatting

        Assert.Throws<FormatException>( () => new Query( "/a//=7", root ) );
        Assert.Throws<FormatException>( () => new Query( "/a//SetThis=7", root ) );
        Assert.Throws<FormatException>( () => new Query( "/a//InvokeThis()", root ) );
    }

    //----------------------------------------------------------------------------------------------------

    enum TestEnum
    {
        Red,
        Green,
        Blue
    }

    [Test]
    public void ShouldWorkWithCommonTypes()
    {
        var root = new Dictionary<string, object>()
        {
            { "l_int",  new { list = new List<int>() { 1,2,3 } } },
            { "l_obj",  new { list = new List<object>() { new { a = 1 }, new { b = 2 }, new { c = 3 } } } },
            { "a_int",  new { arr  = new int[] { 1, 2, 3 } } },
            { "a_obj",  new { arr  = new object[] { new { a = 1 }, new { b = 2 }, new { c = 3 } } } },
            { "l_enum", new { list = new List<TestEnum>() { TestEnum.Red, TestEnum.Green, TestEnum.Blue } } },
            { "dict",   new { dict = new Dictionary<string,int>() { { "a", 1 }, { "b", 2 }, { "c", 3 } } } }
        };

        List<object> r;

        // lists

        r = new Query( "/l_int.list/*", root ).Select().Execute();
        Assert.AreEqual( 3, r.Count );
        Assert.AreEqual( r[ 2 ], 3 );

        r = new Query( "/l_obj.list/*.b", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.AreEqual( 2, r[ 0 ] );

        r = new Query( "/l_enum.list/*", root ).Select().Execute();
        Assert.AreEqual( 3, r.Count );
        Assert.AreEqual( TestEnum.Red, r[ 0 ] );


        // arrays

        r = new Query( "/a_int.arr/*", root ).Select().Execute();
        Assert.AreEqual( 3, r.Count );

        r = new Query( "/a_obj.arr/*.b", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.AreEqual( 2, r[ 0 ] );

        // dictionaries

        r = new Query( "/dict.dict", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.IsNotNull( r[ 0 ] as IDictionary );

        r = new Query( "/dict.dict/*", root ).Select().Execute();
        Assert.AreEqual( 3, r.Count );
        Assert.AreEqual( 1, r[ 0 ] );


        //
        // where clauses
        //

        // array index

        r = new Query( "/a_obj.arr[2]", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.AreEqual( new { c = 3 }, r[ 0 ] );

        r = new Query( "/a_int.arr[2]", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.AreEqual( 3, r[ 0 ] );


        // filter

        r = new Query( "/a_int.arr[$>=2]", root ).Select().Execute();
        Assert.AreEqual( 2, r.Count );
        Assert.AreEqual( 2, r[ 0 ] );
        Assert.AreEqual( 3, r[ 1 ] );

        r = new Query( "/a_obj.arr[b=2].b", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.AreEqual( 2, r[ 0 ] );


        // list index

        r = new Query( "/l_int.list[2]", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.AreEqual( 3, r[ 0 ] );

        // list value filter

        r = new Query( "/l_int.list[$>=2]", root ).Select().Execute();
        Assert.AreEqual( 2, r.Count );
        Assert.AreEqual( 2, r[ 0 ] );
        Assert.AreEqual( 3, r[ 1 ] );

        r = new Query( "/l_obj.list[b=2].b", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.AreEqual( 2, r[ 0 ] );

        // enums in where clauses

        r = new Query( "/l_enum.list[$!='Green']", root ).Select().Execute();
        Assert.AreEqual( 2, r.Count );
        Assert.AreEqual( TestEnum.Red, r[ 0 ] );
        Assert.AreEqual( TestEnum.Blue, r[ 1 ] );

        // where clauses for dictionaries

        r = new Query( "/dict.dict['b']", root ).Select().Execute();
        Assert.AreEqual( 1, r.Count );
        Assert.AreEqual( 2, r[ 0 ] );

        r = new Query( "/dict.dict['z']", root ).Select().Execute();
        Assert.AreEqual( 0, r.Count );

        r = new Query( "/dict.dict[$>=2]", root ).Select().Execute();
        Assert.AreEqual( 2, r.Count );
        Assert.AreEqual( 2, r[ 0 ] );
        Assert.AreEqual( 3, r[ 1 ] );
    }
}

#endif
