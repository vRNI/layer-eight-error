using UnityEngine;

public class IdleEntityState
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
            if ( Vector3.Distance( m_entity.GetFormationSlotWorldPosition(), m_entity.GetWorldPosition() ) > formationConfiguration.GetFollowThreshold() )
            {
                m_entity.SetState< WalkEntityState >();
            }
            else
            {
                // stay on current position
            }
        }
        else
        {
            // stay on current position
        }
    }
}
