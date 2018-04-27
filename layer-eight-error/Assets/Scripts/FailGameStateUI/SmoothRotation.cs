
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothRotation
    : MonoBehaviour
{
    [ SerializeField ]
    private float m_rotationSpeed = 1.0f;
    
    [ SerializeField ]
    private float m_rotationSpeedSlow = 0.25f;

    [ SerializeField ]
    private bool m_isSlowingDownOnHover = false;

    private void Update()
    {
        var ownTransform  = GetComponent< Transform >();
        var isHovered = false;
        if ( m_isSlowingDownOnHover )
        {
            RaycastHit hit;
            if ( MathUtil.RaycastFromMousePointer( out hit ) )
            {
                // hitting myself with raycast
                if ( hit.collider.gameObject == gameObject )
                {
                    isHovered = true;
                }
            }
        }

        var rotationSpeed = isHovered ? m_rotationSpeedSlow : m_rotationSpeed;
        ownTransform.RotateAround( ownTransform.position, Vector3.left, Time.deltaTime * 360.0f * rotationSpeed );
    }
}
