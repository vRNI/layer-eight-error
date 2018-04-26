
using UnityEngine;

public class WalkEntityState
    : EntityState
{
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

        if (m_entity.IsHostile)
        {
            // max distance -> go back
            // attack closest enemy
            if (m_entity.GetTarget() != null)
            {
                var target = Finder.GetEntityManager().GetNearestEntity(m_entity.GetWorldPosition(), m_entity.IsFriendly);
                m_entity.SetTarget(target);
            }
            else
            {
                var target = Finder.GetEntityManager().GetNearestEntity(m_entity.GetWorldPosition(), m_entity.IsFriendly);
                if (m_entity.GetTarget() != target)
                {
                    m_entity.SetTarget(target);
                }
            }

            m_entity.SeekTargetPosition();

            // if distance_threshold > distance -> switch to attack state;
            // and attack angle -> adjust rotation, and so on;
            if (m_entity.GetDistanceToTarget() < m_entity.AttackRange)
            {
                m_entity.SetCurrentState<AttackingEntityState>();
            }
        }
        else
        {
            var formationSlot = m_entity.GetFormationSlot();
            var formationConfiguration = m_entity.GetLeader().GetComponent<LeaderEntity>().GetFormationConfiguration();

            if (formationConfiguration.IsValid(formationSlot))
            {
                // check if slot needs to be followed
                if ( m_entity.IsFormationSlotReached() )
                {
                    m_entity.SetCurrentState<IdleEntityState>();
                }
                else
                {
                    m_entity.SeekFormationSlot();
                }
            }
            else
            {
                m_entity.SetCurrentState<IdleEntityState>();
            }
        }
    }
}
