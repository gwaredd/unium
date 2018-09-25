// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

using UnityEngine;
using gw.unium;


////////////////////////////////////////////////////////////////////////////////////////////////////
// register an object in the node hierarchy - useful for DontDestroy objects

public class UniumRegister : MonoBehaviour
{
    public string OptionalName;

    #if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

    void Start()
    {
        if( string.IsNullOrEmpty( OptionalName ) )
        {
            OptionalName = gameObject.name;
        }

        Unium.Root.Add( OptionalName, gameObject );
    }

    void Destroy()
    {
        Unium.Root.Remove( OptionalName );
    }

    #endif
}

