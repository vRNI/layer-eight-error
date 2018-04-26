using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeekFormationSlotPositionState : EntityState {
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        var formationSlot  = m_entity.GetFormationSlot();
        var leader         = m_entity.GetLeader();
        var leaderOverlord = leader.GetComponent< LeaderOverlord >();
        // check false state assignment ( non-player entities )
        if ( leaderOverlord == null )
        {
            m_entity.SetCurrentState< WalkEntityState >();
            return;
        }

        var formationConfiguration = leaderOverlord.GetFormationConfiguration();
        // search underling units with valid formation slot position for which we need to wait
        var underlingUnits = formationConfiguration.GetUnderlingEntitiesWithValidFormationSlots();

        if ( formationConfiguration.IsValid( formationSlot ) )
        {
            var areAllEntitiesInFormation = underlingUnits.All( a_x => a_x.IsFormationSlotReached() );

            // check if all slots are reached
            if ( areAllEntitiesInFormation )
            {
                m_entity.SetCurrentState< WalkEntityState >();
            }
            else
            {
                m_entity.SeekFormationSlot();
            }
        }
        else
        {
            m_entity.SetCurrentState< WalkEntityState >();
        }
    }
}