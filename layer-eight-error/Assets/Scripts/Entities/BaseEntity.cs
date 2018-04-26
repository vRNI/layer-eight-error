using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EntityType
{
    /// <summary>
    /// Represents no specific entity.
    /// </summary>
    None,

    /// <summary>
    /// Represents the fighter entity.
    /// </summary>
    Fighter,

    /// <summary>
    /// Represents the ranger entity.
    /// </summary>
    Ranger,
    
    /// <summary>
    /// Represents the mage entity.
    /// </summary>
    Mage,
}

[RequireComponent(typeof(NavMeshAgent))]
[DisallowMultipleComponent]
public class BaseEntity : MonoBehaviour {

    [SerializeField]
    protected int m_healthPoints;
    public int HealthPoints { get { return m_healthPoints; } set { m_healthPoints = value; } }
    [SerializeField]
    protected float m_attackRange;
    public float AttackRange { get { return m_attackRange; } set { m_attackRange = value; } }
    [SerializeField]
    protected int m_attackPoints;
    public int AttackPoints { get { return m_attackPoints; } set { m_attackPoints = value; } }
    [SerializeField]
    protected float m_attackAngle;
    public float AttackAngle { get { return m_attackAngle; } set { m_attackAngle = value; } }
    [ Tooltip( "Duration of one attack." ) ]
    [SerializeField]
    protected float m_attackDuration;
    public float AttackDuration { get { return m_attackDuration; } set { m_attackDuration = value; } }
    [ Tooltip( "Duration between two attacks." ) ]
    [SerializeField]
    protected float m_attackCooldown;
    public float AttackCooldown { get { return m_attackCooldown; } set { m_attackCooldown = value; } }

    [ SerializeField ]
    protected EntityType m_entityType;
    [ Tooltip( "The maximal duration it takes until the entity and formation leader look rotation are synced." ) ]
    [ SerializeField ]
    protected float              m_idleLookRotationSyncMaxDuration = 1.0f;
    protected NavMeshAgent m_navMeshAgent;
    protected bool m_isHostile;
    public bool IsHostile { get { return m_isHostile; } set { m_isHostile = value; } }
    
    private float m_attackTimer = 0.0f;

    public EntityType GetEntityType()
    {
        return m_entityType; // move type to each explicit class
    }
    
    public float GetIdleLookRotationSyncMaxDuration()
    {
        return m_idleLookRotationSyncMaxDuration;
    }

    /// <summary>
    /// Checks whether the entity can launch an attack or not.
    /// </summary>
    public bool CanAttackTarget()
    {
        return m_attackTimer > m_attackDuration;
    }

    public virtual void AttackTarget()
    {
        // reset attack timer
        m_attackTimer = 0.0f;
    }

    public virtual bool IsDead()
    {
        return m_healthPoints <= 0;
    }

    protected virtual void Die()
    {
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        if ( IsDead() ) // already dead
        {
           Die();
        }

		m_attackTimer += Time.deltaTime;
	}

    protected virtual void Awake()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public Vector3 GetWorldPosition()
    {
        return gameObject.GetComponent<Transform>().position;
    }

    public bool HasNavMeshStoppedMoving()
    {
        // Check if we've reached the destination
        if (!m_navMeshAgent.pathPending)
        {
            if (m_navMeshAgent.remainingDistance <= m_navMeshAgent.stoppingDistance)
            {
                if (!m_navMeshAgent.hasPath || m_navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public virtual void ReceiveDamage(int damageValue)
    {
        m_healthPoints -= damageValue;
        // if <= 0 -> dead entity
        // to list -> dead entities
    }

    public virtual void StopWalking()
    {
        m_navMeshAgent.SetDestination(Finder.GetCurrentPosition(gameObject));
    }
}
