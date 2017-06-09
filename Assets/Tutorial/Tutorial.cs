using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
	void Start()
    {
        System.Diagnostics.Process.Start( "http://localhost:8342/tutorial/index.html" );
	}
}

