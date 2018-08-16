// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

using System;
using System.Collections.Generic;
using System.Linq;

namespace gw.proto.utils
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // convert value types to json


    // TODO: enum types
    // TODO: dictionaries
    // TODO: support xref's?


    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // serialisers for overriding specific types

    abstract public class JsonSerialiser
    {
        abstract public string Convert( object o );
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // bool

    public class SerialiseBool : JsonSerialiser
    {
        override public string Convert( object o )
        {
            return (bool) o ? "true" : "false";
        }
    }

    public class SerialiseInt : JsonSerialiser
    {
        override public string Convert( object o )
        {
            return ((int)o).ToString();
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// wrappers around unity value types

    public class JsonTypeConverters
    {
        Dictionary<Type, JsonSerialiser> mConverters;

        public JsonTypeConverters()
        {
            mConverters = new Dictionary<Type, JsonSerialiser>()
            {
                { typeof( bool ), new SerialiseBool() },
                { typeof( int ),  new SerialiseInt() },
            };
        }

        public void Add( Type type, JsonSerialiser converter )
        {
            mConverters[ type ] = converter;
        }

        public bool HasOverride( Type type )
        {
            return FindFirstConverter( type ) != null;
        }

        // note doesn't take into account if there's a better converter down the class hierarchy
        private JsonSerialiser FindFirstConverter( Type type )
        {
            Type t = mConverters.Keys.FirstOrDefault( item => item.IsAssignableFrom( type ) );
            return t != null ? mConverters[t] : null;
        }

        public string Serialise( object o )
        {
            var type = o.GetType();

            JsonSerialiser serializer = FindFirstConverter( type );

            if( serializer != null )
            {
                return serializer.Convert( o );
            }

            return SerialiseValueType( o );
        }

        public static string SerialiseValueType( object o )
        {
            var type = o.GetType();

            if( type == typeof( bool ) )
            {
                return o.ToString().ToLower();
            }
            else if( type.IsPrimitive )
            {
                return o.ToString();
            }

            return EscapedString( o.ToString() );
        }

        public static string EscapedString( string str )
        {
            if( str == null )
            {
                return "null";
            }

            int index = 0;
            char[] text = new char[ str.Length * 2 + 2 ];

            text[ index++ ] = '"';

            foreach( var ch in str )
            {
                switch( ch )
                {
                    // escape backslash and quotes

                    case '\\':
                    case '"':
                        text[ index++ ] = '\\';
                        break;

                    // replace these control characters

                    case '\b':
                        text[ index++ ] = '\\';
                        text[ index++ ] = '\b';
                        continue;

                    case '\t':
                        text[ index++ ] = '\\';
                        text[ index++ ] = '\t';
                        continue;

                    case '\n':
                        text[ index++ ] = '\\';
                        text[ index++ ] = 'n';
                        continue;

                    // ignore these control characters

                    case '\0':
                    case '\f':
                    case '\r':
                        continue;
                }

                text[ index++ ] = ch;
            }

            text[ index++ ] = '"';

            return new string( text, 0, index );
        }
    }
}
