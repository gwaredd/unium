// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using UnityEngine;

using gw.gql;

namespace gw.unium
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class InterpreterGameObject : InterpreterDefault
    {
        override public Child[] Children( object obj )
        {
            var go = obj as GameObject;

            if( go == null )
            {
                return null;
            }

            var count = 0;
            var children = new Child[ go.transform.childCount ];

            foreach( Transform child in go.transform )
            {
                children[ count++ ] = new Child( child.name, child.gameObject );
            }

            return children;
        }


        public override object Attr( object obj, string name )
        {
            var go = obj as GameObject;

            if( go != null )
            {
                if( name == "path" )
                {
                    return UniumUtils.Path( go );
                }
                else if( name == "iid" )
                {
                    return go.GetInstanceID();
                }

                // find component

                var component = go.GetComponent( name );

                if( component != null )
                {
                    return component;
                }
            }

            return base.Attr( obj, name );
        }
    }

    public class InterpreterGameObjectArray : InterpreterDefault
    {
        override public Child[] Children( object obj )
        {
            var goa = obj as GameObject[];

            if( goa == null )
            {
                return null;
            }

            var count = 0;
            var children = new Child[ goa.Length ];

            foreach( var child in goa )
            {
                children[ count++ ] = new Child( child.name, child );
            }

            return children;
        }
    }
}

#endif
