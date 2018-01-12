// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System;
using System.Collections.Generic;

namespace gw.gql
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public partial class Query
    {
        List<object> ActionInvoke()
        {
            var functionName = SearchPath.Target;

            if( string.IsNullOrEmpty( functionName ) )
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
                    // get function pointer

                    var type    = obj.GetType();
                    var method  = type.GetMethod( functionName );

                    if( method == null )
                    {
                        continue;
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

                    var result = method.Invoke( obj, args );
                    results.Add( result );
                }
                catch( Exception e )
                {
                    if( UniumComponent.IsDebug )
                    {
                        UniumComponent.Warn( string.Format( "Failed to invoke '{0}' - {1}", functionName, e.Message ) );
                    }
                }
            }

            return results;
        }
    }
}

#endif
