using UnityEngine;

public class IdleEntityState
    : EntityState
{
    public override void Update()
    {
        base.Update();
        
        if ( Finder.GetGameStateManager().IsCurrentState< FormationGameState >() ) { return; }

        var formationSlot          = m_entity.GetFormationSlot();
        var formationConfiguration = m_entity.GetFormationConfiguration();

        // check if entity is in formation
        if ( formationConfiguration.IsValid( formationSlot ) )
        {
            // check if slot needs to be followed
            if ( Vector3.Distance( m_entity.GetFormationSlotWorldPosition(), m_entity.GetWorldPosition() ) > formationConfiguration.GetFollowThreshold() )
            {
                m_entity.SetCurrentState< WalkEntityState >();
            }
            else // stay on current position
            {
            }
        }
        else // stay on current position
        {
        }
    }
}
