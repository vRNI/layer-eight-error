using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationEditorDragDropTarget
    : MonoBehaviour
{
    private Position2 m_slotPosition;

    public Position2 SlotPosition { get { return m_slotPosition; } set { m_slotPosition = value; } }

    [ Tooltip( "The type of entity the drag drop target represents." ) ]
    [ SerializeField ]
    private EntityType m_entityType;

    /// <summary>
    /// Gets the type of entity the drag drop target represents.
    /// </summary>
    public EntityType GetEntityType()
    {
        return m_entityType;
    }
}
