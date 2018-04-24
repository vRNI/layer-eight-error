using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationEditorDragDropTarget
    : MonoBehaviour
{
    private Position2 m_slotPosition;

    public Position2 SlotPosition { get { return m_slotPosition; } set { m_slotPosition = value; } }
}
