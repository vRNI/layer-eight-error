using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleGameState : GameState {

    static UnityAction listener;

    bool m_areAlliesSeekingFormationSlot;

    public void SetAreareAlliesSeekingFormationSlot( bool a_value )
    {
        m_areAlliesSeekingFormationSlot = a_value;
    }

    public BattleGameState()
    {
        listener = new UnityAction(TriggerTransition<IdleGameState>);
    }

    public override void Enter()
    {
        EventManager.StartListening("TransitionIdleState", listener);
    }

    public override void Exit()
    {
        EventManager.StopListening("TransitionIdleState", listener);
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
        // update force seek formation slot behavior
        if ( Input.GetButtonDown( AxisName.ForceFormationSlotPosition ) == true )
        {
            if ( m_areAlliesSeekingFormationSlot == false )
            {
                foreach( var underling in Finder.GetPlayer().GetComponent< FormationConfiguration >().GetUnderlingUnits() )
                {
                    underling.SetCurrentState<SeekFormationSlotPositionState>();
                    m_areAlliesSeekingFormationSlot = true;
                }
            }
            else
            {
                foreach( var underling in Finder.GetPlayer().GetComponent< FormationConfiguration >().GetUnderlingUnits() )
                {
                    underling.SetCurrentState<WalkEntityState>();
                    m_areAlliesSeekingFormationSlot = false;
                }
            }
            return;
        }
    }
}
