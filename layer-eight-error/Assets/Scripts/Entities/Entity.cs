using UnityEngine;
using UnityEngine.AI;

[ RequireComponent( typeof( EntityStateManager ) ) ]
[ RequireComponent( typeof( NavMeshAgent ) ) ]
[ DisallowMultipleComponent ]
public /*abstract*/ class Entity // make entity class abstract and add fighter and so on...
    : MonoBehaviour
{
    protected EntityState        m_currentState;
    [ SerializeField ] // replace this by formation editor
    protected Position2          m_formationSlot;
    protected NavMeshAgent       m_navMeshAgent;
    protected EntityStateManager m_entityStateManager;
    
    public virtual void Awake()
    {
        m_navMeshAgent       = gameObject.GetComponent< NavMeshAgent >();
        m_entityStateManager = gameObject.GetComponent< EntityStateManager >();
        m_entityStateManager.SetEntity( this );
        m_entityStateManager.SetCurrentState< IdleEntityState >();
    }
    
    public virtual void SetCurrentState <TState >()
        where TState : EntityState, new()
    {
        m_entityStateManager.SetCurrentState< TState >();
    }

    public Position2 GetFormationSlot()
    {
        return m_formationSlot;
    }

    public Vector3 GetFormationSlotWorldPosition()
    {
        var formationConfiguration = Finder.GetFormationConfiguration();
        var playerPosition         = Finder.GetPlayerCurrentPosition();

        return playerPosition + formationConfiguration.GetSlotOffset( m_formationSlot );
    }

    public Vector3 GetWorldPosition()
    {
        return gameObject.GetComponent< Transform >().position;
    }

    public virtual void SeekFormationSlot()
    {
        var target = GetFormationSlotWorldPosition();

        m_navMeshAgent.SetDestination( target );
    }
}
