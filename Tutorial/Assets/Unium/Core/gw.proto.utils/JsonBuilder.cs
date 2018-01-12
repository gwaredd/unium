// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

using System;
using System.Text;

namespace gw.proto.utils
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // quick and dirty (non-validating) json formatting utility class

    public class JsonBuilder
    {
        enum Token
        {
            None,
            ObjectBegin,
            ArrayBegin,
            Key,

            ObjectEnd,
            ArrayEnd,
            Value
        }

        StringBuilder   mBuilder        = new StringBuilder();
        Token           mPrevious       = Token.None;

        public string GetString()
        {
            return mBuilder.ToString();
        }

        private void NewItem()
        {
            if( mPrevious == Token.ArrayEnd || mPrevious == Token.ObjectEnd || mPrevious == Token.Value )
            {
                mBuilder.Append( ',' );
            }
        }

        public void BeginObject()
        {
            NewItem();
            mBuilder.Append( '{' );
            mPrevious = Token.ObjectBegin;
        }

        public void EndObject()
        {
            if( mPrevious == Token.Key )
            {
                throw new Exception( "JsonBuilder - object property not given a value" );
            }

            mBuilder.Append( '}' );
            mPrevious = Token.ObjectEnd;
        }

        public void BeginArray()
        {
            NewItem();
            mBuilder.Append( '[' );
            mPrevious = Token.ArrayBegin;
        }

        public void EndArray()
        {
            if( mPrevious == Token.Key )
            {
                throw new Exception( "JsonBuilder - object property not given a value" );
            }

            mBuilder.Append( ']' );
            mPrevious = Token.ArrayEnd;
        }

        public void Name( string name )
        {
            if( mPrevious == Token.Key )
            {
                throw new Exception( "JsonBuilder - object property not given a value" );
            }

            NewItem();
            mBuilder.AppendFormat( "\"{0}\":", name );
            mPrevious = Token.Key;
        }

        public void Value( bool value )
        {
            NewItem();
            mBuilder.Append( value ? "true" : "false" );
            mPrevious = Token.Value;
        }

        public void Value( string value )
        {
            NewItem();
            mBuilder.Append( value );
            mPrevious = Token.Value;
        }

        public void StringValue( string value )
        {
            NewItem();
            mBuilder.AppendFormat( JsonTypeConverters.EscapedString( value ) );
            mPrevious = Token.Value;
        }
    }
}

