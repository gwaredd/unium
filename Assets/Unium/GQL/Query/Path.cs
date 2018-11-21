// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace gw.gql
{
    public class Path
    {
        public Query.Action Action      = Query.Action.Get;
        public string       Target      = null;
        public string[]     Arguments   = null;


        public Path( string query )
        {
            Parse( query );
        }


        // accessors

        public int      Length                  { get { return mPath.Length; } }
        public Segment  this[ int index ]       { get { return mPath[ index ]; } }


        // query path

        Segment[] mPath = null;

        public class Segment
        {
            public enum Type { Children, Attribute };

            public string   Select          = null;             // text of path section
            public Type     NodeType        = Type.Children;
            public Where    Where           = null;             // filter function
            public Regex    Match           = null;             // regex name match
            public bool     RecursiveFind   = false;
        }


        //----------------------------------------------------------------------------------------------------
        // q := /some/node.attr[x>3]/child.value
        // q := /some/node.attr[x>3]/child.value=value
        // q := /some/node.attr[x>3]/child.function(arg)

        static Regex sMatchToken = new Regex(

            @"(?<type>[/\.])" +
            @"(?<name>[^\[\]\./=\(]+)?" +
            @"(\[(?<where>[^\]]+)\])?" +
            @"(?<action>(=|\()(?<params>.*))?"
            , RegexOptions.Singleline

            // enable regex compilation if using the full .net API compatibility (much faster!)
            #if NET_2_0
            | RegexOptions.Compiled
            #endif
        );

        public void Parse( string query )
        {
            // parse path

            var path  = new List<Segment>();
            var match = sMatchToken.Match( query );

            var isRecursive = false;

            while( match.Success )
            {
                var name    = match.Groups[ "name" ].Value;
                var type    = match.Groups[ "type" ].Value[ 0 ] == '/' ? Segment.Type.Children : Segment.Type.Attribute;


                // is this an empty entry flag as recursive find

                if( string.IsNullOrEmpty( name ) )
                {
                    // where clause without name is shorthand for '*'
                    //  q := /a/b/[x=3]/...

                    if( match.Groups[ "where" ].Success )
                    {
                        name = "*";
                    }

                    // no where clause or name then "recursive find"
                    //  q := /a/b//c

                    else
                    {
                        // being recrusive find ...


                        // only available on child nodes because we don't list attr's

                        if( type != Segment.Type.Children )
                        {
                            throw new FormatException( "GQL::Query - recursive find not supported for attributes" );
                        }

                        // actions require targets - so if we have an action this is a fail

                        if( match.Groups[ "action" ].Success )
                        {
                            throw new FormatException( "GQL::Query - field name required for action" );
                        }


                        // flag this - recursion is a modifier on the next whole segment

                        isRecursive = true;

                        // next match
                        match = match.NextMatch();
                        continue;
                    }
                }


                // has action

                if( match.Groups[ "action" ].Success )
                {
                    // actions can only be applied to attributes

                    if( type != Segment.Type.Attribute )
                    {
                        throw new FormatException( "GQL::Query - failed to parse query, no attribute to set" );
                    }

                    // get target (attribute) of action

                    Target = name;

                    var action  = match.Groups[ "action" ].Value;
                    var p       = match.Groups[ "params" ].Value;


                    // set field value - q := /some/node.field=123

                    if( action[ 0 ] == '=' )
                    {
                        Action       = Query.Action.Set;
                        Arguments    = new string[] { p };
                    }


                    // invoke function - q := /some/node.function(a,b,c)

                    else
                    {
                        if( p.Length == 0 || p[ p.Length - 1 ] != ')' )
                        {
                            throw new FormatException( "GQL::Query - failed to parse invoke, end of arguments not found" );
                        }

                        Action = Query.Action.Invoke;

                        var argsStr = p.Substring( 0, p.Length - 1 );

                        var args = new List<string>();

                        for( int pos = 0; pos < argsStr.Length; pos++ )
                        {
                            var ch = argsStr[ pos ];

                            if( char.IsWhiteSpace( ch ) )
                            {
                                continue;
                            }

                            var start = pos;

                            if( ch == '\'' || ch == '{' || ch == '"' )
                            {
                                var end = ch == '{' ? '}' : ch;

                                while( ++pos < argsStr.Length )
                                {
                                    if( argsStr[ pos ] == end )
                                    {
                                        args.Add( argsStr.Substring( start, pos - start + 1 ) );
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                do
                                {
                                    if( pos == argsStr.Length || argsStr[ pos ] == ',' || char.IsWhiteSpace( argsStr[ pos ] ) )
                                    {
                                        args.Add( argsStr.Substring( start, pos - start ) );
                                        break;
                                    }

                                    pos++;
                                }
                                while( true );
                            }
                        }

                        Arguments = args.ToArray();
                    }

                    // an action can only occur on the last segment
                    break;
                }



                // parse where clause

                Where where = match.Groups[ "where" ].Success ? new Where( match.Groups[ "where" ].Value ) : null;


                // wildcard name match?

                #if NET_2_0
                    Regex name_match = name.Contains( "*" ) ? new Regex( name.Replace( "*", ".*?" ), RegexOptions.Compiled ) : null;
                #else
                    Regex name_match = name.Contains( "*" ) ? new Regex( name.Replace( "*", ".*?" ) ) : null;
                #endif


                // add section to path

                var section = new Segment()
                {
                    Select          = name,
                    NodeType        = type,
                    Where           = where,
                    Match           = name_match,
                    RecursiveFind   = isRecursive,
                };

                path.Add( section );

                isRecursive = false;


                // next

                match = match.NextMatch();
            }

            if( path.Count == 0 )
            {
                throw new Exception( "GQL::Query - failed to parse selector" );
            }

            mPath = path.ToArray();
        }
    }
}

#endif
