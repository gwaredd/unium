// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

using System;
using System.Collections;
using System.Collections.Generic;

namespace gw.gql
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // interpreters provide the "DOM" search interface to a given type 

    public class Interpreter
    {
        public class Child
        {
            public Child( string name, object value )
            {
                Name    = name;
                Value   = value;
            }

            public string Name;
            public object Value;
        }

        virtual public object   Attr( object obj, string name )     { return null; }
        virtual public Child[]  Children( object obj )              { return null; }
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public static class Interpreters
    {
        static Interpreter mDefault = new InterpreterDefault();
        static Dictionary<Type, Interpreter> mInterpreters = new Dictionary<Type, Interpreter>(); 

        static public void Add( Type type, Interpreter interpreter )
        {
            mInterpreters[ type ] = interpreter;
        }

        static public void Remove( Type type )
        {
            mInterpreters.Remove( type );
        }

        static public Interpreter Get( Type type )
        {
            return mInterpreters.ContainsKey( type ) ? mInterpreters[ type ] : mDefault;
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // default interpreter - use reflection to provide interface onto object

    public class InterpreterDefault : Interpreter
    {
        //----------------------------------------------------------------------------------------------------
        // attr

        public override object Attr( object obj, string name )
        {
            var type = obj.GetType();

            // find field?

            var field = type.GetField( name );

            if( field != null )
            {
                if( field.IsPrivate )
                {
                    return null;
                }

                return field.GetValue( obj );
            }

            // property?

            var prop = type.GetProperty( name );

            if( prop != null )
            {
                if( prop.CanRead == false )
                {
                    return null;
                }

                return prop.GetValue( obj, null );
            }

            return null;
        }


        //----------------------------------------------------------------------------------------------------
        // children

        override public Child[] Children( object obj )
        {
            // only for collection types

            var dict = obj as IDictionary;

            if( dict != null )
            {
                var children = new Child[ dict.Count ];
                var i = 0;

                foreach( var key in dict.Keys )
                {
                    children[ i++ ] = new Child( key.ToString(), dict[ key ] );
                }

                return children;
            }

            // array

            var type = obj.GetType();

            if( typeof( IEnumerable ).IsAssignableFrom( type ) )
            {
                var results = new List<Child>();

                foreach( var child in obj as IEnumerable )
                {
                    results.Add( new Child( null, child ) );
                }

                return results.ToArray();
            }
            else if( type.IsArray )
            {
                // foreach element, pass query

                var results = new List<Child>();

                foreach( var element in obj as object[] )
                {
                    results.Add( new Child( null, element ) );
                }

                return results.ToArray();
            }

            return null;
        }
    }
}

