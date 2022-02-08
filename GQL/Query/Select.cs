// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
using System.Collections.Generic;
using System.Collections;


#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System;
using System.Linq;

namespace gw.gql
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public partial class Query
    {
        public Query Select()
        {
            Select( Root, 0 );
            return this;
        }

        void Select( object obj, int depth )
        {
            // item not found?

            if( obj == null )
            {
                return;
            }

            // if we're at the end of the query path then select

            if( depth == SearchPath.Length )
            {
                Selected.Add( obj );
                return;
            }


            // if we are a leaf node but still have a remaining path, then we can go no futher

            var type = obj.GetType();

            if( type.IsPrimitive )
            {
                return;
            }


            // get interpreter for current node

            var interpreter = Interpreters.Get( type );


            // get next path section

            var segment = SearchPath[ depth ];



            // select children

            if( segment.NodeType == Path.Segment.Type.Children )
            {
                // get children from node

                var child_nodes = interpreter.Children( obj );

                // exit if node has no children

                if( child_nodes == null || child_nodes.Length == 0 )
                {
                    return;
                }

                // recurse?

                if( segment.RecursiveFind )
                {
                    foreach( var node in child_nodes )
                    {
                        Select( node.Value, depth );
                    }
                }


                // filter by name

                var filtered_children = from child in child_nodes where MatchName( segment, child ) select child.Value;


                // apply where clause

                ++depth;

                if( segment.Where != null )
                {
                    segment.Where.ForEach( filtered_children, Select, depth );
                }
                else
                {
                    foreach( var child in filtered_children )
                    {
                        Select( child, depth );
                    }
                }
            }


            // otherwise select attribute

            else
            {
                var attr = interpreter.Attr( obj, segment.Select );

                if( attr == null )
                {
                    return;
                }

                ++depth;

                if( segment.Where == null )
                {
                    Select( attr, depth );
                }
                else
                {
                    if( attr is IDictionary )
                    {
                        segment.Where.ForEach( attr as IDictionary, Select, depth );
                    }
                    else if( attr.GetType().IsArray || attr is IEnumerable )
                    {
                        segment.Where.ForEach( ( attr as IEnumerable ).Cast<object>(), Select, depth );
                    }
                    else if( segment.Where.Apply( attr ) )
                    {
                        Select( attr, depth );
                    }
                }
            }
        }


        //----------------------------------------------------------------------------------------------------

        private static bool MatchName( Path.Segment section, Interpreter.Child child )
        {
            if( section.Select == "*" )
            {
                return true;
            }

            if( child.Name == null )
            {
                return false;
            }

            if( section.Match != null )
            {
                return section.Match.IsMatch( child.Name );
            }

            return String.Compare( child.Name, section.Select, StringComparison.InvariantCultureIgnoreCase ) == 0;
        }
    }
}

#endif
