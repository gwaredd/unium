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


            foreach( var current in Selected )
            {
                try
                {
                    // get method

                    var target    = current;
                    var type      = target.GetType();
                    var method    = type.GetMethod( actionName );
                    var multicast = null as MulticastDelegate;

                    if( method == null )
                    {
                        // if no method then is there an event?

                        var field = type.GetField( actionName, BindingFlags.Instance | BindingFlags.NonPublic );

                        if( field != null )
                        {
                            multicast = field.GetValue( target ) as MulticastDelegate;
                            method    = multicast != null ? multicast.GetMethodInfo() : null;
                        }
                        else
                        {
                            // if no event, is there a field or property with an Invoke function (e.g. UnityEvent or buttons)

                            field  = type.GetField( actionName );

                            if( field != null )
                            {
                                target = field.GetValue( target );
                            }
                            else
                            {
                                var prop = type.GetProperty( actionName );
                                target = prop.GetValue( target );
                            }

                            method = target != null ? target.GetType().GetMethod( "Invoke" ) : null;
                        }

                        // if unable to find something to invoke then skip this object

                        if( method == null )
                        {
                            continue;
                        }
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
                        result = method.Invoke( target, args );
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
