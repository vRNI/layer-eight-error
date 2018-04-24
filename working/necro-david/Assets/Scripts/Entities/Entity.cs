using UnityEngine;
using UnityEngine.AI;

[ RequireComponent( typeof( EntityStateManager ) ) ]
[ RequireComponent( typeof( NavMeshAgent ) ) ]
[ DisallowMultipleComponent ]
public /*abstract*/ class Entity // make entity class abstract and add fighter and so on...
    : MonoBehaviour
{
    protected EntityState            m_currentState;
    [ SerializeField ] // replace this by formation editor
    protected Position2              m_formationSlot;
    [ SerializeField ]
    protected GameObject             m_leader;
    protected NavMeshAgent           m_navMeshAgent;
    protected EntityStateManager     m_entityStateManager;

    public virtual void Awake()
    {
        m_navMeshAgent           = gameObject.GetComponent< NavMeshAgent >();
        m_entityStateManager     = gameObject.GetComponent< EntityStateManager >();
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

    public FormationConfiguration GetFormationConfiguration()
    {
        return m_leader.GetComponent< FormationConfiguration >();
    }

    public Vector3 GetFormationSlotWorldPosition()
    {
        var formationConfiguration = GetFormationConfiguration();
        var currentLeaderPosition  = Finder.GetCurrentPosition( m_leader );
        var desiredLeaderPosition  = Finder.GetDesiredPosition( m_leader );
        var desiredDirection       = ClampToMaxDistance( desiredLeaderPosition - currentLeaderPosition, formationConfiguration.GetFollowMaxDistance() );
        var leaderRotationY        = m_leader.GetComponent< Transform >().eulerAngles.y;
        var slotOffset             = formationConfiguration.GetSlotOffset( m_formationSlot );
        var slotOffsetRotated      = Quaternion.AngleAxis( leaderRotationY, Vector3.up ) * slotOffset;
        var desiredSlotPosition    = currentLeaderPosition + desiredDirection + slotOffsetRotated;

        return desiredSlotPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return gameObject.GetComponent< Transform >().position;
    }

    public virtual void SeekFormationSlot()
    {
        if ( m_leader == null ) { return; }

        var target = GetFormationSlotWorldPosition();

        m_navMeshAgent.SetDestination( target );
    }

    private static Vector3 ClampToMaxDistance( Vector3 a_vector, float a_maxDistance )
    {
        var length = Vector3.Distance( a_vector, Vector3.zero );
        if ( length > a_maxDistance ) { return a_vector * a_maxDistance / length; }
        
        return a_vector;
    }
}
