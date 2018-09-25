// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
using System.Collections;


#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System;
using System.Collections.Generic;

using gw.gql.calc;

namespace gw.gql
{
    public class Where
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // WhereContext

        static object GetAttrFromChain( object o, string name )
        {
            foreach( var field in name.Split( new char[] { '.' } ) )
            {
                o = o != null ? Interpreters.Get( o.GetType() ).Attr( o, field ) : null;
            }

            return o;
        }


        delegate object WhereFunction( LogicalExpression[] arguments, WhereContext context );

        static Dictionary<string,WhereFunction> sFunctions = new Dictionary<string, WhereFunction>()
        {
            { "abs",    ( args, context ) => Math.Abs( Convert.ToDouble( args[0].Evaluate( context ) ) ) },

            { "cos",    ( args, context ) => Math.Cos( Convert.ToDouble( args[0].Evaluate( context ) ) ) },
            { "sin",    ( args, context ) => Math.Sin( Convert.ToDouble( args[0].Evaluate( context ) ) ) },
            { "tan",    ( args, context ) => Math.Tan( Convert.ToDouble( args[0].Evaluate( context ) ) ) },

            { "sqrt",   ( args, context ) => Math.Sqrt( Convert.ToDouble( args[0].Evaluate( context ) ) ) },
            { "ceil",   ( args, context ) => Math.Ceiling( Convert.ToDouble( args[0].Evaluate( context ) ) ) },
            { "floor",  ( args, context ) => Math.Floor( Convert.ToDouble( args[0].Evaluate( context ) ) ) },
            { "log",    ( args, context ) => Math.Log( Convert.ToDouble( args[0].Evaluate( context ) ) ) },
            { "round",  ( args, context ) => Math.Round( Convert.ToDouble( args[0].Evaluate( context ) ) ) },
            { "sign",   ( args, context ) => Math.Sign( Convert.ToDouble( args[0].Evaluate( context ) ) ) },

            { "has",    ( args, context ) => GetAttrFromChain( context.Subject, args[0].Evaluate( context ) as string ) != null },
        };

        public class WhereContext : EvaluationContext
        {
            public object Subject;

            public WhereContext()
            {
                EvaluateParameter = ( name, context ) =>
                {
                    var subject = ( context as WhereContext ).Subject;
                    return name == "$" ? subject : GetAttrFromChain( subject, name );
                };

                InvokeFunction = ( string name, LogicalExpression[] arguments, EvaluationContext context ) =>
                {
                    return sFunctions.ContainsKey( name ) ? sFunctions[ name ].Invoke( arguments, context as WhereContext ) : null;
                };
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // where

        Expression   mExpr    = null;
        WhereContext mContext = new WhereContext();

        public Where( string str )
        {
            mExpr = new Expression( str );
        }

        public bool IsBoolean { get { return mExpr.IsBoolean; } }

        public delegate void Visitor( object o, int depth );


        //----------------------------------------------------------------------------------------------------

        public bool Apply( object obj )
        {
            if( obj == null || mExpr.IsBoolean == false )
            {
                return false;
            }

            var selected = false;

            try
            {
                mContext.Subject = obj;
                selected = Convert.ToBoolean( mExpr.Evaluate( mContext ) );
            }
            catch( Exception )
            {
            }

            mContext.Subject = null;
            return selected;
        }


        //----------------------------------------------------------------------------------------------------
        // foreach

        public void ForEach( IEnumerable<object> objects, Visitor visit, int depth )
        {
            if( objects == null )
            {
                return;
            }

            if( mExpr.IsBoolean )
            {
                foreach( var o in objects )
                {
                    try
                    {
                        mContext.Subject = o;

                        if( Convert.ToBoolean( mExpr.Evaluate( mContext ) ) )
                        {
                            visit( o, depth );
                        }
                    }
                    catch( Exception )
                    {
                    }
                }
            }
            else
            {
                try
                {
                    var index = Convert.ToInt32( mExpr.Evaluate( mContext ) );

                    if( index < 0 )
                    {
                        return;
                    }

                    mContext.Subject = objects;

                    foreach( var o in objects )
                    {
                        if( index == 0 )
                        {
                            visit( o, depth );
                            break;
                        }

                        --index;
                    }
                }
                catch( Exception )
                {
                }
            }

            mContext.Subject = null;
        }

        //----------------------------------------------------------------------------------------------------
        // foreach

        public void ForEach( IDictionary dict, Visitor visit, int depth )
        {
            if( dict == null )
            {
                return;
            }

            if( mExpr.IsBoolean )
            {
                foreach( var o in dict.Values )
                {
                    try
                    {
                        mContext.Subject = o;

                        if( Convert.ToBoolean( mExpr.Evaluate( mContext ) ) )
                        {
                            visit( o, depth );
                        }
                    }
                    catch( Exception )
                    {
                    }
                }
            }
            else
            {
                try
                {
                    var key = mExpr.Evaluate( mContext );
                    var keyType = dict.GetType().GetGenericArguments()[ 0 ];
                    var keyActual = Convert.ChangeType( key, keyType );

                    if( dict.Contains( keyActual  ) )
                    {
                        visit( dict[ keyActual ], depth );
                    }
                }
                catch( Exception )
                {
                }
            }

            mContext.Subject = null;
        }
    }
}

#endif

