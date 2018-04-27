
﻿using System.Collections;
using System.Collections.Generic;
 using System.Linq;
 using UnityEngine;
 using UnityEngine.AI;

[RequireComponent(typeof(EntityStateManager))]
public class UnderlingEntity : BaseEntity {

    BaseEntity m_target;

    [SerializeField]
    protected GameObject m_formationLeader;
    protected EntityStateManager m_entityStateManager;
    [SerializeField]
    protected Position2 m_formationSlot;
    [SerializeField]
    protected bool m_isFriendly;
    public bool IsFriendly { get { return m_isFriendly; } set { m_isFriendly = value; } }
    

    private void Start()
    {
        // add to formation configuration in start, because formation leader is not set in awake
        m_formationLeader.GetComponent<LeaderEntity>().GetFormationConfiguration().AddUnderlingEntity(this);
    }

    public override bool IsDead()
    {
        return m_healthPoints <= 0;
    }
    
    protected override void Awake()
    {
        base.Awake();
        m_entityStateManager = gameObject.GetComponent<EntityStateManager>();
        m_entityStateManager.SetEntity(this);
        m_entityStateManager.SetCurrentState<IdleEntityState>();

        Finder.GetEntityManager().AddUnderling(this);
    }

    public void SetCurrentState<TState>()
        where TState : EntityState, new()
    {
        m_entityStateManager.SetCurrentState<TState>();
    }

    public EntityState GetCurrentState()
    {
        return m_entityStateManager.GetCurrentState();
    }

    public Position2 GetFormationSlot()
    {
        return m_formationSlot;
    }
    
    public void SetFormationSlot( Position2 a_value )
    {
        m_formationSlot = a_value;
    }

    public Vector3 GetFormationSlotWorldPosition()
    {
        if (m_formationLeader == null) { return Vector3.zero; }

        var formationConfiguration = m_formationLeader.GetComponent<LeaderEntity>().GetFormationConfiguration();
        var currentLeaderPosition = Finder.GetCurrentPosition(m_formationLeader);
        var desiredLeaderPosition = Finder.GetDesiredPosition(m_formationLeader);
        var desiredDirection = ClampToMaxLengthPlanar(desiredLeaderPosition - currentLeaderPosition, formationConfiguration.GetFollowMaxDistance());
        var leaderRotationY = m_formationLeader.GetComponent<Transform>().eulerAngles.y;
        var slotOffset = formationConfiguration.GetSlotOffset(m_formationSlot);
        var slotOffsetRotated = Quaternion.AngleAxis(leaderRotationY, Vector3.up) * slotOffset;
        var desiredSlotPosition = currentLeaderPosition + desiredDirection + slotOffsetRotated;

        return desiredSlotPosition;
    }

    public virtual void SeekFormationSlot()
    {
        var target = GetFormationSlotWorldPosition();

        m_navMeshAgent.SetDestination(target);
    }

    public virtual bool IsFormationSlotReached()
    {
        return Vector3.Distance( GetFormationSlotWorldPosition(), GetWorldPosition() ) < m_formationLeader.GetComponent<LeaderEntity>().GetFormationConfiguration().GetFollowThreshold();
    }

    public virtual void SeekTargetPosition()
    {
        if ( IsTargetInRange() )
        {
            StopWalking();
        }
        else
        {
            m_navMeshAgent.SetDestination(m_target.gameObject.transform.position);
        }
    }

    public bool IsTargetInRange()
    {
        return GetDistanceToTarget() < AttackRange;
    }

    public override void AttackTarget()
    {
        if ( CanAttackTarget() == false ) { return; }

        // timer reset
        base.AttackTarget();

        m_target.ReceiveDamage(m_attackPoints);
        Debug.Log(m_attackPoints + " were inflicted.");
    }

    public virtual void SetTarget(BaseEntity a_entity)
    {
        m_target = a_entity;
    }

    public virtual BaseEntity GetTarget()
    {
        return m_target;
    }

    /// <summary>
    /// Clamps the vector to a maximal distance on the XZ 2D plane.
    /// </summary>
    /// <param name="a_vector">
    /// The vector to clamp.
    /// </param>
    /// <param name="a_maxDistance">
    /// The max vector length.
    /// </param>
    /// <returns>
    /// The clamped vector.
    /// </returns>
    private static Vector3 ClampToMaxLengthPlanar( Vector3 a_vector, float a_maxDistance )
    {
        // calculate distance on XZ plane
        var length    = Vector3.Distance( a_vector, new Vector3( 0.0f, a_vector.y, 0.0f ) );
        // clamp on XZ plane
        if ( length > a_maxDistance )
        {
            var scale = a_maxDistance / length;
            return new Vector3( a_vector.x * scale, a_vector.y, a_vector.z * scale );
        }
        
        return a_vector;
    }

    public GameObject GetLeader()
    {
        return m_formationLeader;
    }

    public void SetLeader(GameObject a_leader)
    {
        m_formationLeader = a_leader;
    }
    
    protected override void Die()
    {
        base.Die();

        // remove myself from target
        if ( GetTarget() != null )
        {
            GetTarget().DeregisterFromCombatSlot( this );
            SetTarget( null );
        }
        // remove myself from formation configuration
        if ( m_formationLeader != null )
        {
            m_formationLeader.GetComponent<LeaderEntity>().GetFormationConfiguration().RemoveUnderlingEntity( this );
            m_formationLeader = null;
        }
        // set state to dead
        SetCurrentState<DeadEntityState>();
    }
    
    public virtual void Resurect()
    {
        Debug.Log("Ressurect");
        m_isFriendly      = true;
        m_isHostile       = false;
        m_formationLeader = Finder.GetPlayer();
        var formationConfiguration = m_formationLeader.GetComponent<LeaderOverlord>().GetFormationConfiguration();
        var slotPos = formationConfiguration.GetEmptyFormationSlot();
        SetFormationSlot(slotPos);
        formationConfiguration.AddUnderlingEntity(this);
        m_healthPoints = 100;
        GetComponent<NavMeshAgent>().enabled = true;
        SetCurrentState<SeekFormationSlotPositionState>();

        // update formation collider size
        FollowSquad.UpdateFormationBoundingBox( Finder.GetPlayer() );
    }
    
    public float GetDistanceToTarget()
    {
        return Vector3.Distance(this.GetWorldPosition(), m_target.GetWorldPosition());
    }
}
