// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace gw.proto.utils
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // walk an object hierarchy and convert to json

    public static class JsonReflector
    {
        static JsonTypeConverters mConverters = new JsonTypeConverters();

        public static void Add( Type type, JsonSerialiser converter )
        {
            mConverters.Add( type, converter );
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static string Reflect( object obj )
        {
            var json = new JsonBuilder();

            Recurse( obj, json, 0 );

            return json.GetString();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void ReflectObject( object obj, JsonBuilder json, int depth = 0 )
        {
            var type = obj.GetType();

            json.BeginObject();

            foreach( var field in type.GetFields( BindingFlags.Instance | BindingFlags.Public ) )
            {
                var value = field.GetValue( obj );
                var valueType = field.FieldType;

                if( valueType.IsPrimitive || valueType == typeof( string ) )
                {
                    json.Name( field.Name );
                    json.Value( JsonTypeConverters.SerialiseValueType( value ) );
                }
                else
                {
                    json.Name( field.Name );

                    if( depth <= 1 )
                    {
                        Recurse( value, json, depth );
                    }
                    else
                    {
                        json.Value( "{}" );
                    }
                }
            }

            foreach( var prop in type.GetProperties() )
            {
                if( prop.CanRead == false )
                {
                    continue;
                }

                try
                {
                    var obsolute = prop.GetCustomAttributes( typeof( ObsoleteAttribute ), true );

                    if( obsolute.Length > 0 )
                    {
                        continue;
                    }

                    var value = prop.GetValue( obj, null );
                    var valueType = prop.PropertyType;

                    if( valueType.IsPrimitive || valueType == typeof( string ) )
                    {
                        json.Name( prop.Name );
                        json.Value( JsonTypeConverters.SerialiseValueType( value ) );
                    }
                    else if( depth <= 1 )
                    {
                        json.Name( prop.Name );

                        if( depth <= 1 )
                        {
                            Recurse( value, json, depth );
                        }
                        else
                        {
                            json.Value( "{}" );
                        }
                    }
                }
                catch( Exception e )
                {
                    Util.Warn( e.ToString() );
                }
            }

            json.EndObject();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private static void Recurse( object obj, JsonBuilder json, int depth )
        {
            if( obj == null )
            {
                json.Value( "null" );
                return;
            }

            depth++;

            var type = obj.GetType();

            // primitives

            if( type == typeof( string ) )
            {
                json.Value( JsonTypeConverters.EscapedString( obj as string ) );
            }
            else if( type.IsPrimitive )
            {
                if( type == typeof( bool ) )
                {
                    json.Value( obj.ToString().ToLower() );
                }
                else if( type == typeof( char ) )
                {
                    json.Value( JsonTypeConverters.EscapedString( obj.ToString() ) );
                }
                else
                {
                    json.Value( obj.ToString() );
                }
            }

            // custom

            else if( mConverters.HasOverride( type ) )
            {
                json.Value( mConverters.Serialise( obj ) );
            }

            // enum

            else if( type.IsEnum )
            {
                json.Value( JsonTypeConverters.EscapedString( Enum.GetName( type, obj ) ) );
            }

            else if( type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>) )
            {
                json.BeginObject();

                var dict = obj as IDictionary;

                foreach( var key in dict.Keys )
                {
                    json.Name( key.ToString() );

                    if( depth == 1 )
                    {
                        Recurse( dict[ key ], json, depth );
                    }
                    else
                    {
                        json.Value( "{}" );
                    }
                }

                json.EndObject();
            }


            // arrays

            else if( typeof( IEnumerable ).IsAssignableFrom( type ) )
            {
                json.BeginArray();

                if( depth <= 2 )
                {
                    foreach( var child in obj as IEnumerable )
                    {
                        Recurse( child, json, depth );
                    }
                }

                json.EndArray();
            }
            else if( type.IsArray )
            {
                json.BeginArray();

                if( depth <= 2 )
                {
                    if( type.GetArrayRank() == 1 )
                    {
                        foreach( var elem in obj as Array )
                        {
                            Recurse( elem, json, depth );
                        }
                    }
                }

                json.EndArray();
            }

            // objects

            else if( type.IsValueType )
            {
                json.BeginObject();

                foreach( var field in type.GetFields( BindingFlags.Instance | BindingFlags.Public ) )
                {
                    var value = field.GetValue( obj );

                    json.Name( field.Name );
                    json.Value( JsonTypeConverters.SerialiseValueType( value ) );
                }

                json.EndObject();
            }

            else if( type.IsClass )
            {
                ReflectObject( obj, json, depth );
            }

            // unknown type!

            else
            {
                json.Value( "null" );
            }
        }
    }
}
