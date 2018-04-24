using UnityEngine;

[ RequireComponent( typeof( Transform ) ) ]
public class CameraFollowPlayer
    : MonoBehaviour
{
    private Transform m_transform;

    private void Awake()
    {
        m_transform = gameObject.GetComponent< Transform >();
    }

    private void Update ()
    {
        var targetPosition = Finder.GetCurrentPosition( Finder.GetPlayer() );
        var targetRotation = m_transform.rotation;

        m_transform.SetPositionAndRotation( targetPosition, targetRotation );
    }
}
