using UnityEngine;
using UnityEngine.UI;

public class Sokoban : MonoBehaviour
{
    public static float Volume = 0.5f;
    public static int CurrentLevel = 0;
    
    public GameObject wall;
    public GameObject player;
    public GameObject box;
    public GameObject goal;

    public GameObject game;

    public GameObject goalsText;
    public GameObject movesText;

    public AudioClip win;
    public AudioClip lose;

    int numGoals = 0;
    int numMoves = 0;
    int goalsActive = 0;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //
    // Map Legend
    //
    //    Wall            #
    //    Player          @
    //    Box             $
    //    Goal            .
    //    Player on Goal  + 
    //    Box on Goal     *
    //    Floor           (Space)
    //

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // Sample Maps

    public static string[][] Levels =
    {
        // test

        new string[]
        {
            "########",
            "#      #",
            "# @ $. #",
            "#   $. #",
            "#      #",
            "#      #",
            "#      #",
            "########",
        },

        // minicosmons 1

        new string[]
        {
            "  ##### ",
            "###   # ",
            "# $ # ##",
            "# #  . #",
            "#    # #",
            "## #   #",
            "#@  ####",
            "#####",
        },

        // picokosmos 5

        new string[]
        {
            "   #####  ",
            " ###   ## ",
            "##   #  ##",
            "#  * * * #",
            "#+  $ *  #",
            "#### * ###",
            "   #  ##  ",
            "   ####   ",
        },

        // microban 155 - 'the dungeon'

        new string[]
        {
            "    ######               #### ",
            "#####*#  #################  ##",
            "#   ###                      #",
            "#        ########  ####  ##  #",
            "### ####     #  ####  ####  ##",
            "#*# # .# # # #     #     #   #",
            "#*# #  #     # ##  # ##  ##  #",
            "###    ### ###  # ##  # ##  ##",
            " #   # #*#      #     # #    #",
            " #   # ###  #####  #### #    #",
            " #####   #####  ####### ######",
            " #   # # #**#               # ",
            "## # #   #**#  #######  ##  # ",
            "#    #########  #    ##### ###",
            "# #             # $        #*#",
            "#   #########  ### @#####  #*#",
            "#####       #### ####   ######",
        },
    };


    ////////////////////////////////////////////////////////////////////////////////////////////////////

    void Start()
    {
        CreateLevel( CurrentLevel );
    }


    public void GoalActive()
    {
        goalsActive++;
        UpdateGoalText();

        if( goalsActive == numGoals )
        {
            GetComponent<AudioSource>().PlayOneShot( win, Volume );
            NextLevel();
        }
    }

    public void GoalInactive()
    {
        goalsActive--;
        UpdateGoalText();
    }

    public void AddMove()
    {
        numMoves++;
        var text = movesText.GetComponent<Text>();
        text.text = string.Format( "Moves: {0}", numMoves );
    }

    public void ResetLevel()
    {
        foreach( Transform go in game.transform )
        {
            Destroy( go.gameObject );
        }

        CreateLevel( CurrentLevel );

        //GetComponent<AudioSource>().PlayOneShot( lose );
    }

    public void NextLevel()
    {
        CurrentLevel = ( CurrentLevel + 1 ) % Levels.Length;
        ResetLevel();
    }

    public void PrevLevel()
    {
        CurrentLevel = ( CurrentLevel + Levels.Length - 1 ) % Levels.Length;
        ResetLevel();
    }

    void UpdateGoalText()
    {
        var text = goalsText.GetComponent<Text>();
        text.text = string.Format( "{0}/{1}", goalsActive, numGoals );
    }

    void CreateLevel( int level )
    {
        numGoals = 0;
        goalsActive = 0;
        numMoves = 0;

        var map = Levels[ level ];
        var height = map.Length;
        var width = 0;

        for( var y = 0; y < map.Length; y++ )
        {
            var line = map[ y ];

            width = Mathf.Max( width, line.Length );

            for( var x = 0; x < line.Length; x++ )
            {
                var pos = new Vector3( x, 0.0f, height - y );
                GameObject tile;

                switch( line[x] )
                {
                    case '#':
                        tile = Instantiate( wall, pos, Quaternion.identity ) as GameObject;
                        tile.transform.parent = game.transform;
                        break;

                    case '@':
                        tile = Instantiate( player, pos, Quaternion.identity ) as GameObject;
                        tile.transform.parent = game.transform;
                        break;

                    case '$':
                        tile = Instantiate( box, pos, Quaternion.identity ) as GameObject;
                        tile.transform.parent = game.transform;
                        break;

                    case '.':
                        tile = Instantiate( goal, pos, Quaternion.identity ) as GameObject;
                        tile.transform.parent = game.transform;
                        numGoals++;
                        break;

                    case '+':
                        tile = Instantiate( player, pos, Quaternion.identity ) as GameObject;
                        tile.transform.parent = game.transform;

                        tile = Instantiate( goal, pos, Quaternion.identity ) as GameObject;
                        tile.transform.parent = game.transform;
                        numGoals++;
                        break;

                    case '*':
                        tile = Instantiate( box, pos, Quaternion.identity ) as GameObject;
                        tile.transform.parent = game.transform;

                        tile = Instantiate( goal, pos, Quaternion.identity ) as GameObject;
                        tile.transform.parent = game.transform;
                        numGoals++;
                        break;
                }
            }
        }

        UpdateGoalText();

        var text = movesText.GetComponent<Text>();
        text.text = string.Format( "Moves: {0}", numMoves );

        Camera.main.transform.position = new Vector3( width / 2, 6.0f, -3.0f );
    }
}



