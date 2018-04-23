using UnityEngine;

[ RequireComponent( typeof( Entity ) ) ]
[ DisallowMultipleComponent ]
public class PlayerSlotMover
    : MonoBehaviour
{
    private Entity m_entity;

    private void Awake()
    {
        m_entity = gameObject.GetComponent< Entity >();
    }

    private void Update()
    {
        if ( Input.GetMouseButtonUp( MouseButtonIndex.Left ) )
        {
            var ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;
            if ( Physics.Raycast( ray, out hit ) == true )
            {
                // Set desired player position
                // navMeshAgent.SetDestination( hit.point );
            }
        }
    }
}
