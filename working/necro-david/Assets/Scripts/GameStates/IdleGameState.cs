
using UnityEngine;

public class IdleGameState
    : GameState
{
    public override void Update()
    {
        base.Update();

        var gameStateManager = Finder.GetGameStateManager();

        // check if the player activated the formation state
        if ( Input.GetButtonDown( AxisName.ToggleFormationMode ) == true )
        {
            gameStateManager.SetCurrentState< FormationGameState >();
            return;
        }
    }
}
