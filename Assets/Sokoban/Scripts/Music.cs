using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour
{
    public static Music Instance = null;


    AudioSource mAudioSource;

    
    void Awake()
    {
        if( Instance != null && Instance != this )
        {
            Destroy( this.gameObject );
            return;
        }

        mAudioSource = GetComponent<AudioSource>();
        
        Instance = this;
        DontDestroyOnLoad( transform.gameObject );
    }

    public float Volume
    {
        get
        {
            return mAudioSource.volume;
        }
        
        set
        {
            mAudioSource.volume = value;
        }
    }
}

