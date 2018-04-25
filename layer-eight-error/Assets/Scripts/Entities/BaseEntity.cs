using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[DisallowMultipleComponent]
public class BaseEntity : MonoBehaviour {

    protected NavMeshAgent m_navMeshAgent;
    protected bool m_isHostile;
    public bool IsHostile { get { return m_isHostile; } set { m_isHostile = value; } }

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
