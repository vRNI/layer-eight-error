using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeadEntityState
    : EntityState {

    public override void Enter()
    {
        base.Enter();
        if (!m_entity.IsFriendly)
        {
            Finder.GetEntityManager().PushToDeadUnderlings(m_entity);
            m_entity.GetComponent<NavMeshAgent>().enabled = false;
        }
        else
        {
            Finder.GetEntityManager().RemoveUnderling(m_entity);
            GameObject.Destroy(m_entity.gameObject);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
