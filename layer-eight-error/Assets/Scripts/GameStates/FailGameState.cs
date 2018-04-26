using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailGameState
    : GameState
{
    public override void Enter()
    {
        base.Enter();

        // create ui elements
        // start camera transitions
        Debug.LogError( "I'm dead." );
        // set all entites non-hostile
    }

    public override void Update()
    {
        base.Update();

        // check if player wannts to restart / exit
    }
}
