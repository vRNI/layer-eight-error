using UnityEngine;

public class IdleGameState 
    : GameState
{
    public IdleGameState()
    {
    }

    public override void Update()
    {
        var gameStateManager = ValidityManager.FindManagerObjectByTag< GameStateManager >();

        // Check if the player activated the formation state.
        if ( Input.GetButtonDown( AxisName.OpenFormationEditor ) == true )
        {
            gameStateManager.SetCurrentState( new FormationState() );

            return;
        }

        // Check if enemies are near enough to trigger the battle state.
        if ( false )
        {
            gameStateManager.SetCurrentState( new BattleState() );
        }
    }
}
