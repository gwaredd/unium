using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuOptions : MonoBehaviour
{
    public Slider music;
    public Slider sfx;


    void Start()
    {
        music.value = Music.Instance.Volume;
        music.onValueChanged.AddListener( OnMusicChanged );

        sfx.value = Sokoban.Volume;
        sfx.onValueChanged.AddListener( OnSfxChanged );
    }

    void OnMusicChanged( float value )
    {
        Music.Instance.Volume = value;
    }

    public void OnSfxChanged( float value )
    {
        Sokoban.Volume = value;
    }
}
