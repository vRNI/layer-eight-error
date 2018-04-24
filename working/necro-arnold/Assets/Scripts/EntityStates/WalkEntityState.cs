
using UnityEngine;

public class WalkEntityState
    : EntityState
{
    public override void Update()
    {
        base.Update();
        
        var formationSlot          = m_entity.GetFormationSlot();
        var formationConfiguration = Finder.GetFormationConfiguration();

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
