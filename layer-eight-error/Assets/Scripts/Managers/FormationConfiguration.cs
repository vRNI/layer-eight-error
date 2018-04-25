using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines formation specific settings.
/// </summary>
[DisallowMultipleComponent]
public class FormationConfiguration
    : MonoBehaviour
{
    [Tooltip("Defines the formation grid size in slots.")]
    [SerializeField]
    private Position2 m_gridSize = new Position2(9, 9);

    /// <summary>
    /// Gets the formation grid size in slots.
    /// </summary>
    public Position2 GetGridSize()
    {
        return m_gridSize;
    }

    /// <summary>
    /// Gets the total formation slot count.
    /// </summary>
    public int GetSlotCount()
    {
        return m_gridSize.X * m_gridSize.Z;
    }

    [Tooltip("Defines the distance in world units between two formation slots.")]
    [SerializeField]
    private Vector2 m_slotDistance = new Vector2(2.0f, 2.0f);

    /// <summary>
    /// Gets the distance in world units between two formation slots.
    /// </summary>
    public Vector2 GetSlotDistance()
    {
        return m_slotDistance;
    }

    /// <summary>
    /// Gets the world offset for a specific formation slot.
    /// </summary>
    /// <param name="a_position">
    /// Position within the formation grid.
    /// </param>
    public Vector3 GetSlotOffset(Position2 a_position)
    {
        if (IsValid(a_position)) { return new Vector3(m_slotDistance.x * a_position.X, 0.0f, m_slotDistance.y * a_position.Z); }

        // if invalid return zero vector
        return Vector3.zero;
    }

    [Tooltip("Defines the threshold in world units which triggers units to follow their formation slots.")]
    [SerializeField]
    private float m_followThreshold = 0.2f;

    /// <summary>
    /// Gets the threshold in world units which triggers units to follow their formation slots.
    /// </summary>
    public float GetFollowThreshold()
    {
        return m_followThreshold;
    }

    [Tooltip("Defines the max distance followed positions should be away.")]
    [SerializeField]
    private float m_followMaxDistance = 2.0f;

    /// <summary>
    /// Gets the max distance followed positions should be away.
    /// </summary>
    public float GetFollowMaxDistance()
    {
        return m_followMaxDistance;
    }

    /// <summary>
    /// Checks if a formation slot is valid.
    /// </summary>
    /// <param name="a_position">
    /// The formation slot to check.
    /// </param>
    public bool IsValid(Position2 a_position)
    {
        if (a_position.X < -(m_gridSize.X - 1) / 2 || a_position.X > (m_gridSize.X - 1) / 2) return false;
        if (a_position.Z < -(m_gridSize.Z - 1) / 2 || a_position.Z > (m_gridSize.Z - 1) / 2) return false;

        return true;
    }

    /// <summary>
    /// Enumerates the grid slot positions.
    /// </summary>
    public IEnumerable<Position2> EnumerateSlotPosition()
    {
        for ( int x = 0; x < m_gridSize.X; ++x )
        {
            for ( int z = 0; z < m_gridSize.Z; ++z )
            {
                int slotX = x - ( m_gridSize.X - 1 ) / 2;
                int slotZ = z - ( m_gridSize.Z - 1 ) / 2;

                yield return new Position2( slotX, slotZ );
            }
        }
    }

    /// <summary>
    /// Contains all entities this formation contains.
    /// </summary>
    private readonly List< Entity > m_entities = new List< Entity >();

    /// <summary>
    /// Adds an entity to the formation.
    /// </summary>
    /// <param name="a_entity">
    /// The entity.
    /// </param>
    public void AddUnderlingEntity( Entity a_entity )
    {
        m_entities.Add( a_entity );
    }

    /// <summary>
    /// Gets all entities.
    /// </summary>
    public Entity[] GetUnderlingEntities()
    {
        return m_entities.ToArray();
    }
}
