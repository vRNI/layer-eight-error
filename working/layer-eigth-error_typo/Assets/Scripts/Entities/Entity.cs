using UnityEngine;
using UnityEngine.AI;

[ RequireComponent( typeof( EntityStateManager ) ) ]
[ RequireComponent( typeof( NavMeshAgent ) ) ]
[ DisallowMultipleComponent ]
public abstract class Entity
    : MonoBehaviour
{
    protected int m_healthPoints;
    protected int m_damagePoints;
    protected float m_attackSpeed;
    protected float m_attackRange;
    protected EntityState m_currentState;
    protected Position2 m_formationSlot;
    protected NavMeshAgent navMeshAgent;
    protected EntityStateManager m_manager;

    public virtual void Awake()
    {
        navMeshAgent = gameObject.GetComponent< NavMeshAgent >();
        m_manager = gameObject.GetComponent< EntityStateManager >();
        m_manager.SetEntity( this );
        m_manager.SetCurrentState< IdleEntityState >();

        m_formationSlot = new Position2( 2, 2 );
    }

    public virtual void Update()
    {
    }

    public virtual void SetState <TState >()
        where TState : EntityState, new()
    {
        m_manager.SetCurrentState< TState >();
    }

    public Position2 GetFormationSlot()
    {
        return m_formationSlot;
    }

    public Vector3 GetFormationSlotWorldPosition()
    {
        var formationConfiguration = Finder.GetFormationConfiguration();
        var playerPosition         = Finder.GetPlayerPosition();

        return playerPosition + formationConfiguration.GetSlotOffset( m_formationSlot );
    }

    public Vector3 GetWorldPosition()
    {
        return gameObject.GetComponent< Transform >().position;
    }

    public virtual void SeekFormationSlot()
    {
        var target = GetFormationSlotWorldPosition();

        navMeshAgent.SetDestination( target );
    }
}
