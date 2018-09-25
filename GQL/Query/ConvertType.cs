// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using UnityEngine;
using System;
using TinyJson;

namespace gw.gql
{
    public static class ConvertType
    {
        public static object FromString( string value, Type type )
        {
            if( type.IsEnum )
            {
                return Enum.Parse( type, value );
            }

            if( type == typeof( Vector3 ) )
            {
                return value.FromJson<Vector3>();
            }

            return Convert.ChangeType( value, type );
        }
    }
}

#endif

