using UnityEngine;

public class IdleState 
    : GameState
{
    public IdleState()
    {
    }

    public override void Update()
    {
        var gameStateManager = ValidityManager.FindManager< GameStateManager >();

        // Check if the player activated the formation state.
        if ( Input.GetButtonDown( AxisName.OpenFormationEditor ) == true )
        {
            gameStateManager.SetCurrentState< FormationState >();

            return;
        }

        // Check if enemies are near enough to trigger the battle state.
        if ( false )
        {
            gameStateManager.SetCurrentState< BattleState >();
        }
    }
}
