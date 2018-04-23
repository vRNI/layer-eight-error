using UnityEngine;

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
        m_desiredPosition = Finder.GetPlayerCurrentPosition();
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
                m_desiredPosition = hit.point;
            }
        }
    }
}
