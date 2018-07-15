// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System.Collections.Generic;
using UnityEngine;

namespace gw.gql
{
    public static class UniumUtils
    {
        public static string Path( GameObject go )
        {
            if( go == null )
            {
                return null;
            }

            var path = new List<string>();
            var xf = go.transform;

            while( xf != null )
            {
                path.Add( string.Format( "{0}[iid={1}]", xf.name, xf.gameObject.GetInstanceID() ) );
                xf = xf.parent;
            }

            path.Reverse();

            return "/" + string.Join( "/", path.ToArray() );
        }
    }
}

#endif
