using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonExitApplication
    : MonoBehaviour
{
    private void Update()
    {
        if ( Input.GetMouseButtonDown( MouseButtonIndex.Left ) )
        {
            RaycastHit hit;
            if ( MathUtil.RaycastFromMousePointer( out hit ) )
            {
                // hitting myself with raycast
                if ( hit.collider.gameObject == gameObject )
                {
                    Application.Quit();
                }
            }
        }
    }
}
