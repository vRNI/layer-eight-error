using UnityEngine;

public class IdleEntityState
    : EntityState
{
    private float m_rotationSyncTime;

    public override void Update()
    {
        base.Update();
        
        if ( Finder.GetGameStateManager().IsCurrentState< FormationGameState >() ) { return; }

        var formationLeader                 = m_entity.GetLeader();
        var leaderTransform                 = formationLeader.GetComponent< Transform >();
        var entityTransform                 = m_entity.GetComponent< Transform >();
        
        var leaderEulerAngleY               = leaderTransform.eulerAngles.y;
        var entityEulerAngleY               = entityTransform.eulerAngles.y;
        var rotationOffset                  = Mathf.Abs( leaderEulerAngleY - entityEulerAngleY );
        // scale down max sync duration ( 360° offset equal to max sync duration, 180° to half of max sync duration, ... )
        var idleLookRotationSyncMaxDuration = m_entity.GetIdleLookRotationSyncMaxDuration();
        var syncDuration                    = rotationOffset / 360.0f * idleLookRotationSyncMaxDuration;
        // calculate lerp scale
        var lerpScale                       = m_rotationSyncTime / syncDuration;
        // calculate new entity look rotation
        var lookRotationNew = MathUtil.LerpQuaternion(entityTransform.rotation, leaderTransform.rotation, lerpScale);

        var formationSlot                   = m_entity.GetFormationSlot();
        var formationConfiguration          = m_entity.GetLeader().GetComponent<LeaderEntity>().GetFormationConfiguration();
        
        // check if entity is in formation
        if ( formationConfiguration.IsValid( formationSlot ) )
        {
            // check if slot needs to be followed
            if ( Vector3.Distance( m_entity.GetFormationSlotWorldPosition(), m_entity.GetWorldPosition() ) > formationConfiguration.GetFollowThreshold() )
            {
                m_entity.SetCurrentState< WalkEntityState >();
                m_rotationSyncTime          = 0.0f;
            }
            else // stay on current position
            {
                // Update rotation sync time if not in formation state
                m_rotationSyncTime         += Time.deltaTime;
                // follow leader look rotation if not moving
                entityTransform.rotation    = lookRotationNew;
            }
        }
        else // stay on current position
        {
            // Update rotation sync time if not in formation state
            m_rotationSyncTime             += Time.deltaTime;
            // follow leader look rotation if not moving
            entityTransform.rotation        = lookRotationNew;
        }
    }
}
