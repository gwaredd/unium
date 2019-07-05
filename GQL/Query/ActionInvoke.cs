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
                    // find something to invoke - either a method, event or field/property with an Invoke function (e.g. UnityEvent or Button)

                    var target     = current;
                    var targetType = target.GetType();
                    var method     = null as MethodInfo;
                    var multicast  = null as MulticastDelegate;

                    var member     = targetType.GetMember( actionName );
                    var memberType = member.Length > 0 ? member[ 0 ].MemberType : 0;

                    if( ( memberType & MemberTypes.Method ) == MemberTypes.Method )
                    {
                        method = targetType.GetMethod( actionName );
                    }
                    else if( ( memberType & MemberTypes.Event ) == MemberTypes.Event )
                    {
                        var field = targetType.GetField( actionName, BindingFlags.Instance | BindingFlags.NonPublic );
                        multicast = field == null ? null : field.GetValue( target ) as MulticastDelegate;

                        if( multicast != null )
                        {
                            var delegateList = multicast.GetInvocationList();
                            method = delegateList.Length > 0 ? delegateList[ 0 ].Method : null;
                        }
                    }
                    else if( ( memberType & MemberTypes.Field ) == MemberTypes.Field )
                    {
                        var field = targetType.GetField( actionName );
                        target = field == null  ? null : field.GetValue( target );
                        method = target == null ? null : target.GetType().GetMethod( "Invoke" );
                    }
                    else if( ( memberType & MemberTypes.Property ) == MemberTypes.Property  )
                    {
                        var prop = targetType.GetProperty( actionName );
                        target   = prop == null   ? null : prop.GetValue( target, null );
                        method   = target == null ? null : target.GetType().GetMethod( "Invoke" );
                    }

                    // if we didn't find anything invokable then skip this object

                    if( method == null )
                    {
                        continue;
                    }
                    
                    // otherwise, convert arguments passed in to the appropriate types

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
