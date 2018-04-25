using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(EntityStateManager))]
public class UnderlingEntity : BaseEntity {

    [Serializable]
    protected struct UnderlingStats
    {
        int m_healthPoints, m_attackPoints;
        float m_attackRange, m_attackAngle;
    }

    BaseEntity m_target;

    [SerializeField]
    protected GameObject m_formationLeader;
    protected EntityStateManager m_entityStateManager;
    [SerializeField]
    protected Position2 m_formationSlot;
    
    [SerializeField]
    protected UnderlingStats m_underlingStats;
    [SerializeField]
    protected bool m_isFriendly;
    public bool IsFriendly { get { return m_isFriendly; } }
    

    // Use this for initialization
    void Start () {
        m_formationLeader.GetComponent<LeaderEntity>().GetFormationConfiguration().AddUnderlingEntity(this);
        Debug.Log(m_formationLeader.GetComponent<FormationConfiguration>().GetUnderlingUnits().Count);
    }
	
	// Update is called once per frame
	void Update () {
        
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
        var desiredDirection = ClampToMaxDistance(desiredLeaderPosition - currentLeaderPosition, formationConfiguration.GetFollowMaxDistance());
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

    public virtual void SeekTargetPosition()
    {
        m_navMeshAgent.SetDestination(m_target.gameObject.transform.position);
    }

    public virtual void SetTarget(BaseEntity a_entity)
    {
        m_target = a_entity;
    }

    public virtual BaseEntity GetTarget()
    {
        return m_target;
    }

    private static Vector3 ClampToMaxDistance(Vector3 a_vector, float a_maxDistance)
    {
        var length = Vector3.Distance(a_vector, Vector3.zero);
        if (length > a_maxDistance) { return a_vector * a_maxDistance / length; }

        return a_vector;
    }

    public GameObject GetLeader()
    {
        return m_formationLeader;
    }

    protected virtual void Attack() { }
    protected virtual void Die()
    {

    }
}
