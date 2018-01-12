// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;

namespace gw.gql
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class Root
    {
        Dictionary<string,object> mRoot = new Dictionary<string,object>();

#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

        public Interpreter.Child[] Children
        {
            get
            {
                var i = 0;
                var children = new Interpreter.Child[ mRoot.Count ];

                foreach( var el in mRoot )
                {
                    var fn = el.Value as Func<object>;
                    children[ i++ ] = new Interpreter.Child( el.Key.ToString(), fn != null ? fn() : el.Value );
                }

                return children;
            }
        }

#endif

        public void Add( string key, object value )
        {
            mRoot.Add( key, value );
        }

        public void Remove( string key )
        {
            mRoot.Remove( key );
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////

#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )
    
    public class InterpreterSearchRoot  : InterpreterDefault
    {
        override public Child[] Children( object obj )
        {
            return ( obj as Root ).Children;
        }
    }

#endif
}


        