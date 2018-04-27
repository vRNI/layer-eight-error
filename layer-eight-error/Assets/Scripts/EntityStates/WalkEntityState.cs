
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WalkEntityState
    : EntityState
{
    public override void Update()
    {
        base.Update();

        if (m_entity.IsHostile)
        {
            // clear my own target if it is dead
            if ( m_entity.GetTarget() != null )
            {
                if ( m_entity.GetTarget().IsDead() )
                {
                    m_entity.SetTarget( null );
                }
            }

            // max distance -> go back
            // attack closest enemy
            var entities = new List< BaseEntity > { Finder.GetPlayer().GetComponent< LeaderOverlord >() };
            foreach ( var underling in Finder.GetEntityManager().GetUnderlings().Where( a_x => a_x.IsHostile ) )
            {
                // skip own
                if ( underling == m_entity ) { continue; }
                entities.Add( underling );
            }

            // remove full entities
            int index = 0;
            var maybeFullTarget = GetNearestEntity(entities, m_entity.GetWorldPosition(), m_entity.IsFriendly);
            while ( index < entities.Count )
            {
                var current = entities[ index ];
                // entity is full and remove from list
                if ( current.AreCombatSlotsFree() == false )
                {
                    entities.RemoveAt( index );
                }
                else // check next entry
                {
                    ++index;
                }
            }

            // i am a fighter
            if ( m_entity.GetEntityType() == EntityType.Fighter )
            {
                var target = GetNearestEntity(entities, m_entity.GetWorldPosition(), m_entity.IsFriendly);
                if (m_entity.GetTarget() == null && target != null)
                {
                    // set own target
                    m_entity.SetTarget(target);
                    // register to new target
                    m_entity.GetTarget().RegisterToCombatSlot(m_entity);
                }
                // i'm bored -> going back
                if ( m_entity.GetTarget() == null )
                {
                    // stop following target
                    m_entity.SeekFormationSlot();
                }
            }
            else // i am a ranger / mage
            {
                // check maybe full target
                if (m_entity.GetTarget() == null && maybeFullTarget != null)
                {
                    // set own target
                    m_entity.SetTarget(maybeFullTarget);
                }
                // i stop walking because i'm tired
                if ( m_entity.GetTarget() == null )
                {
                    // stop following target
                    m_entity.StopWalking();
                    return;
                }
            }

            if ( m_entity.GetTarget() == null ) { return; }

            m_entity.SeekTargetPosition();

            // wait until next attack can be launched
            if ( m_entity.CanAttackTarget() )
            {
                // if distance_threshold > distance -> switch to attack state;
                // and attack angle -> adjust rotation, and so on;
                if (m_entity.IsTargetInRange())
                {
                    m_entity.SetCurrentState<AttackingEntityState>();
                }
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
    
    private BaseEntity GetNearestEntity( IEnumerable< BaseEntity > a_entities, Vector3 a_entityPosition, bool isFriendly)
    {
        return SelectMinDistanceEntity(a_entities, a_entityPosition, isFriendly);
    }

    private BaseEntity SelectMinDistanceEntity( IEnumerable< BaseEntity > a_entities, Vector3 a_entityPosition, bool isFriendly)
    {
        float closestDistance = float.MaxValue;
        float currentDistance = float.MaxValue;
        BaseEntity closestEntity = null;

        foreach(BaseEntity entity in a_entities)
        {
            var underling = entity as UnderlingEntity;
            var overlord  = entity as LeaderOverlord;
            // skip entities with same attitude
            if (isFriendly == ( underling != null && underling.IsFriendly || overlord != null ) ) continue;

            currentDistance = Vector3.SqrMagnitude(entity.GetWorldPosition() - a_entityPosition);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                closestEntity = entity;
            }
        }
        
        return closestEntity;
    }
}
