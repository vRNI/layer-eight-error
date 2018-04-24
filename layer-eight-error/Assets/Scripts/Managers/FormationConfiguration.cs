using UnityEngine;
using System.Collections.Generic;

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
    private List<Entity> m_underlingUnits;

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

    public void AddUnderlingEntity(Entity a)
    {
        if (m_underlingUnits != null)
        {
            m_underlingUnits.Add(a);
        }
        else
        {
            m_underlingUnits = new List<Entity>();
            m_underlingUnits.Add(a);
        }
    }

    public List<Entity> GetUnderlingUnits()
    {
        return m_underlingUnits;
    }


}
