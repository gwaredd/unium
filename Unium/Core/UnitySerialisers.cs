// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using UnityEngine;

using gw.proto.utils;

namespace gw.unium
{
    //
    // custom JSON serialisers
    //


    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // GameObject

    public class SerialiseGameObject : JsonSerialiser
    {
        override public string Convert( object o )
        {
            var go = o as GameObject;

            if( go == null )
            {
                return "null";
            }

            var json = new JsonBuilder();

            json.BeginObject();

            json.Name( "name" );
            json.StringValue( go.name );

            json.Name( "tag" );
            json.StringValue( go.tag );

            json.Name( "activeInHierarchy" );
            json.Value( go.activeInHierarchy );

            // components

            json.Name( "components" );
            json.BeginArray();

            var components = go.GetComponents<Component>();

            foreach( var c in components )
            {
                json.StringValue( c.GetType().Name );
            }

            json.EndArray();

            // children

            json.Name( "children" );
            json.BeginArray();

            foreach( Transform child in go.transform )
            {
                json.StringValue( child.name );
            }

            json.EndArray();
            json.EndObject();

            return json.GetString();
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // Transform

    public class SerialiseTransform : JsonSerialiser
    {
        override public string Convert( object o )
        {
            var t = o as Transform;

            if( t == null )
            {
                return "null";
            }

            var json = new JsonBuilder();

            json.BeginObject();

            json.Name( "name" );
            json.StringValue( t.name );

            
            json.Name( "position" );
            json.Value( JsonReflector.Reflect( t.position ) );

            json.Name( "rotation" );
            json.Value( JsonReflector.Reflect( t.rotation ) );

            json.Name( "scale" );
            json.Value( JsonReflector.Reflect( t.localScale ) );

            json.Name( "children" );
            json.BeginArray();

            foreach( Transform c in t )
            {
                json.StringValue( c.name );
            }

            json.EndArray();

            json.EndObject();

            return json.GetString();
        }
    }
}

#endif
