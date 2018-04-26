using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class IdleGameState
    : GameState
{
    UnityAction listener;

    public IdleGameState()
    {
        listener = new UnityAction(TriggerTransition<BattleGameState>);
    }

    public override void Enter()
    {
        EventManager.StartListening("TransitionBattleState", listener);
    }

    public override void Exit()
    {
        EventManager.StopListening("TransitionBattleState", listener);
    }

    public override void Update()
    {
        base.Update();

        // check if the player activated the formation state
        if (Input.GetButtonDown(AxisName.ToggleFormationMode) == true)
        {
            TriggerTransition< FormationGameState >();
            return;
        }

        if ( Input.GetButtonDown( AxisName.ForceFormationSlotPosition ) == true )
        {
            foreach( var underling in Finder.GetPlayer().GetComponent< FormationConfiguration >().GetUnderlingUnits() )
            {
                underling.SetCurrentState<SeekFormationSlotPositionState>();
            }
            return;
        }
    }
}