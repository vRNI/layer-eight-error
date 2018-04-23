using UnityEngine;

[ DisallowMultipleComponent ]
public class FormationConfiguration
    : MonoBehaviour
{
    [ SerializeField ]
    private Position2 m_gridSize = new Position2( 9, 9 );

    public Position2 GetGridSize()
    {
        return m_gridSize;
    }

    public int GetSlotCount()
    {
        return m_gridSize.X * m_gridSize.Z;
    }

    [ SerializeField ]
    private Vector2 m_slotDistance = new Vector2( 2.0f, 2.0f );

    public Vector2 GetSlotDistance()
    {
        return m_slotDistance;
    }

    public Vector3 GetSlotOffset( Position2 a_position )
    {
        return new Vector3( m_slotDistance.x * a_position.X, 0.0f, m_slotDistance.y * a_position.Z );
    }

    [ SerializeField ]
    private float m_followThreshold = 0.2f;

    public float GetFollowThreshold()
    {
        return m_followThreshold;
    }

    public bool IsValid( Position2 a_position )
    {
        if ( a_position.X < -( m_gridSize.X - 1 ) / 2 || a_position.X > ( m_gridSize.X - 1 ) / 2 ) return false;
        if ( a_position.Z < -( m_gridSize.Z - 1 ) / 2 || a_position.Z > ( m_gridSize.Z - 1 ) / 2 ) return false;

        return true;
    }
}
