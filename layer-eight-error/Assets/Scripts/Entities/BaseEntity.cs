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
    
    [ SerializeField ]
    protected EntityType m_entityType;
    [ Tooltip( "The maximal duration it takes until the entity and formation leader look rotation are synced." ) ]
    [ SerializeField ]
    protected float              m_idleLookRotationSyncMaxDuration = 1.0f;
    protected NavMeshAgent m_navMeshAgent;
    protected bool m_isHostile;
    public bool IsHostile { get { return m_isHostile; } set { m_isHostile = value; } }
    
    public EntityType GetEntityType()
    {
        return m_entityType; // move type to each explicit class
    }
    
    public float GetIdleLookRotationSyncMaxDuration()
    {
        return m_idleLookRotationSyncMaxDuration;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected virtual void Awake()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public Vector3 GetWorldPosition()
    {
        return gameObject.GetComponent<Transform>().position;
    }
}
