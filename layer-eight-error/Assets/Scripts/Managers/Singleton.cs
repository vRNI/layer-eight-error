using Boo.Lang.Runtime;
using UnityEngine;

public class Singleton
    : MonoBehaviour
{
    private void Update()
    {
        var gameStateManagers = Object.FindObjectsOfType< GameStateManager >();
        if ( gameStateManagers.Length != 1 )
        {
            throw new RuntimeException( "No or multiple game state managers exist." );
        }
    }
}
