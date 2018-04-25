
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
    [SerializeField] // replace this by resurrection spell ( before enemy, after player )
    protected GameObject m_formationLeader;
    protected NavMeshAgent       m_navMeshAgent;
    protected EntityStateManager m_entityStateManager;
    
    public virtual void Awake()
    {
        m_navMeshAgent       = gameObject.GetComponent< NavMeshAgent >();
        m_entityStateManager = gameObject.GetComponent< EntityStateManager >();
        //m_entityStateManager.SetEntity( this );
        m_entityStateManager.SetCurrentState< IdleEntityState >();
        
        if ( m_formationLeader == null ) { return; }
        //GetFormationConfiguration().AddUnderlingEntity( this );
    }
    
    public virtual void SetCurrentState< TState >()
        where TState : EntityState, new()
    {
        m_entityStateManager.SetCurrentState< TState >();
    }

    public Position2 GetFormationSlot()
    {
        return m_formationSlot;
    }

    public void SetFormationSlot( Position2 a_value )
    {
        m_formationSlot = a_value;
    }

    public GameObject GetFormationLeader()
    {
        return m_formationLeader;
    }

    public FormationConfiguration GetFormationConfiguration()
    {
        if ( m_formationLeader == null ) { return null; }
        return m_formationLeader.GetComponent< FormationConfiguration >();
    }

    public Vector3 GetFormationSlotWorldPosition()
    {
        if ( m_formationLeader == null ) { return Vector3.zero; }

        var formationConfiguration = GetFormationConfiguration();
        var currentLeaderPosition  = Finder.GetCurrentPosition( m_formationLeader );
        var desiredLeaderPosition  = Finder.GetDesiredPosition( m_formationLeader );
        var desiredDirection       = ClampToMaxLengthPlanar( desiredLeaderPosition - currentLeaderPosition, formationConfiguration.GetFollowMaxDistance() );
        var leaderRotationY        = m_formationLeader.GetComponent< Transform >().eulerAngles.y;
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
        var target = GetFormationSlotWorldPosition();

        m_navMeshAgent.SetDestination( target );
    }

    public void StopWalking()
    {
        m_navMeshAgent.SetDestination( Finder.GetCurrentPosition( gameObject ) );
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
            //return a_vector * a_maxDistance / length;
        }
        
        return a_vector;
    }

    public bool IsFriendly()
    {
        if (m_formationLeader != null)
            return m_formationLeader.CompareTag(TagName.Player);

        throw new System.InvalidOperationException("m_formationLeader is not assigned.");
    }
    
    //public void AddUnderlingUnit()
    //{
    //    m_formationLeader.GetComponent<FormationConfiguration>().AddUnderlingEntity(this);
    //}
}
