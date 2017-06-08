using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour
{
    public GameObject[] menus;

    public void Select( GameObject menu )
    {
        foreach( var m in menus )
        {
            m.SetActive( false );
        }

        menu.SetActive( true );
    }


    public void LoadLevel( int level )
    {
        Sokoban.CurrentLevel = level;
        SceneManager.LoadScene( "Sokoban" );
    }

    public void MainMenu()
    {
        SceneManager.LoadScene( "Menu" );
    }
}

