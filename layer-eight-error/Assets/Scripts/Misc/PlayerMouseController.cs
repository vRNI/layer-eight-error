﻿using UnityEngine;

public class PlayerMouseController
    : MonoBehaviour
{
    private Vector3 m_desiredPosition;

    public Vector3 GetDesiredPosition()
    {
        return m_desiredPosition;
    }

    private void Start()
    {
        m_desiredPosition = Finder.GetCurrentPosition( Finder.GetPlayer() );
    }

    private void Update () 
    {
        if ( Input.GetMouseButtonUp( MouseButtonIndex.Left ) )
        {
            // cast ray against scene
            var ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            
            // on hit set desired position
            RaycastHit hit;
            if ( Physics.Raycast( ray, out hit ) == true )
            {
                // add half of player height to desired position, so the distance for idle / walk states are correctly calculated
                m_desiredPosition = hit.point + Finder.GetPlayerHeight() / 2.0f;
            }
        }
    }
}
