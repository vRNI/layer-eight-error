
using UnityEngine;

public class FormationGameState
    : GameState
{
    public override void Enter()
    {
        base.Enter();

        Finder.GetOrthoPerspectiveSwitcher().SwitchToOrtho();
    }

    public override void Update()
    {
        base.Update();

        // check if the player activated the idle state
        if ( Input.GetButtonDown( AxisName.ToggleFormationMode ) == true )
        {
            TriggerTransition< IdleGameState >();
            return;
        }
    }

    public override void Exit()
    {
        Finder.GetOrthoPerspectiveSwitcher().SwitchToPerspective();

        base.Exit();
    }
}
