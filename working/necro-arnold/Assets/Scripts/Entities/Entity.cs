using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(EntityStateManager))]
public abstract class Entity : MonoBehaviour {

    protected int m_healthPoints;
    protected int m_damagePoints;
    protected float m_attackSpeed;
    protected float m_attackRange;
    protected EntityState m_currentState;
    protected Position2 formationPosition;
    protected NavMeshAgent navMeshAgent;
    protected EntityStateManager m_manager;

    public virtual void Update()
    {
        if (m_healthPoints <= 0)
        {
            SetState<DeadState>();
        }
    }

    public virtual void Awake()
    {
        m_manager = gameObject.GetComponent<EntityStateManager>();
    }

    public virtual void SetState<TState>()
        where TState : EntityState, new()
    {
        m_manager.SetCurrentState <TState>();
    }

    public abstract void AttackTarget(GameObject target);

    public virtual void Seek(Vector3 target)
    {
        target.x += formationPosition.x;
        target.z += formationPosition.z;
        navMeshAgent.SetDestination(target);
    }


}
