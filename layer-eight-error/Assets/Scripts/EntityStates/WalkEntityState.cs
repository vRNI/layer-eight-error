
using UnityEngine;

public class WalkEntityState
    : EntityState
{
    public override void Update()
    {
        base.Update();
        
        if ( Finder.GetGameStateManager().IsCurrentState< FormationGameState >() )
        {
            m_entity.StopWalking();
            m_entity.SetCurrentState< IdleEntityState >();
            return;
        }

        var formationSlot          = m_entity.GetFormationSlot();
        var formationConfiguration = m_entity.GetFormationConfiguration();

        if ( formationConfiguration.IsValid( formationSlot ) )
        {
            // check if slot needs to be followed
            if ( Vector3.Distance( m_entity.GetFormationSlotWorldPosition(), m_entity.GetWorldPosition() ) < formationConfiguration.GetFollowThreshold() )
            {
                m_entity.SetCurrentState< IdleEntityState >();
            }
            else
            {
                m_entity.SeekFormationSlot();
            }
        }
        else
        {
            m_entity.SetCurrentState< IdleEntityState >();
        }
    }
}
