// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System;
using System.Collections.Generic;

namespace gw.gql
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public partial class Query
    {
        List<object> ActionSet()
        {
            var fieldName       = SearchPath.Target;
            var valueAsString   = SearchPath.Arguments != null && SearchPath.Arguments.Length > 0 ? SearchPath.Arguments[ 0 ] : null;

            if( string.IsNullOrEmpty( fieldName ) )
            {
                UniumComponent.Warn( "No field name given for Set() call" );
                return null;
            }

            if( valueAsString == null )
            {
                UniumComponent.Warn( "No field value given for Set() call" );
            }

            var results = new List<object>();

            object  cachedValue = null;
            Type    cachedType  = null;
            bool    cacheValid = false;

            foreach( var obj in Selected )
            {
                try
                {
                    // set value

                    var objType = obj.GetType();

                    // set field

                    var fieldInfo = objType.GetField( fieldName );

                    if( fieldInfo != null )
                    {
                        // convert to type
                        
                        if( fieldInfo.FieldType != cachedType )
                        {
                            cachedType  = fieldInfo.FieldType;
                            cachedValue = ConvertType.FromString( valueAsString, cachedType );
                            cacheValid  = true;
                        }

                        if( cacheValid )
                        {
                            fieldInfo.SetValue( obj, cachedValue );
                            results.Add( fieldInfo.GetValue( obj ) );
                        }

                        continue;
                    }

                    // set property

                    var propInfo = objType.GetProperty( fieldName );

                    if( propInfo != null && propInfo.CanWrite )
                    {
                        // convert to type
                        
                        if( propInfo.PropertyType != cachedType )
                        {
                            cachedType  = propInfo.PropertyType;
                            cachedValue = ConvertType.FromString( valueAsString, cachedType );
                            cacheValid  = true;
                        }

                        if( cacheValid )
                        {
                            propInfo.SetValue( obj, cachedValue, null );
                            results.Add( propInfo.GetValue( obj, null ) );
                        }
                    }
                }
                catch( Exception e )
                {
                    cacheValid = false;

                    if( UniumComponent.IsDebug )
                    {
                        UniumComponent.Warn( string.Format( "Failed to set value for field '{0}' - {1}", fieldName, e.Message ) );
                    }
                }
            }

            return results;
        }
    }
}

#endif
