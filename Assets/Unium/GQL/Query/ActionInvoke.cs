// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System;
using System.Collections.Generic;
using System.Reflection;

namespace gw.gql
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public partial class Query
    {
        List<object> ActionInvoke()
        {
            var actionName = SearchPath.Target;

            if( string.IsNullOrEmpty( actionName ) )
            {
                UniumComponent.Warn( "No function name given for Invoke() call" );
                return null;
            }

            var         strArgs         = SearchPath.Arguments;
            object[]    cachedValues    = new object[ strArgs.Length ];
            Type[]      cachedTypes     = new Type[ strArgs.Length ];
            object[]    args            = new object[ strArgs.Length ];


            var results = new List<object>();


            foreach( var obj in Selected )
            {
                try
                {
                    // get method

                    var type      = obj.GetType();
                    var method    = type.GetMethod( actionName );
                    var multicast = null as MulticastDelegate;

                    if( method == null )
                    {
                        // if we can't find a method, try an event instead

                        var field = type.GetField( actionName, BindingFlags.Instance | BindingFlags.NonPublic );
                        multicast = field.GetValue( obj ) as MulticastDelegate;

                        if( multicast == null )
                        {
                            // can't find either so skip this object
                            continue;
                        }

                        method = multicast.GetMethodInfo();
                    }

                    // convert arguments

                    if( strArgs != null )
                    {
                        var argInfo = method.GetParameters();

                        if( argInfo.Length != strArgs.Length )
                        {
                            //Unium.Instance.Log.Warn( "Can't invoke function {0} - parameters do not match", func );
                            continue;
                        }

                        for( int i=0; i < args.Length; i++ )
                        {
                            // convert string to value

                            var argType = argInfo[ i ].ParameterType;

                            if( argType != cachedTypes[ i ] )
                            {
                                cachedTypes[ i ]  = argType;
                                cachedValues[ i ] = ConvertType.FromString( strArgs[ i ], argType );
                            }

                            args[ i ] = cachedValues[ i ];
                        }
                    }

                    // invoke method

                    object result = null;

                    if( multicast != null )
                    {
                        result = multicast.DynamicInvoke( args );
                    }
                    else
                    {
                        result = method.Invoke( obj, args );
                    }

                    results.Add( result );
                }
                catch( Exception e )
                {
                    if( UniumComponent.IsDebug )
                    {
                        UniumComponent.Warn( string.Format( "Failed to invoke '{0}' - {1}", actionName, e.Message ) );
                    }
                }
            }

            return results;
        }
    }
}

#endif
