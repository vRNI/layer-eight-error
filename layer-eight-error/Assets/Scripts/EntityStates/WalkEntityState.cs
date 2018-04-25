
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
        }
        else
        {
            var formationSlot = m_entity.GetFormationSlot();
            var formationConfiguration = m_entity.GetLeader().GetComponent<LeaderEntity>().GetFormationConfiguration();

            if (formationConfiguration.IsValid(formationSlot))
            {
                // check if slot needs to be followed
                if (Vector3.Distance(m_entity.GetFormationSlotWorldPosition(), m_entity.GetWorldPosition()) < formationConfiguration.GetFollowThreshold())
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
