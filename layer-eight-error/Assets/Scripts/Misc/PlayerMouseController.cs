using UnityEngine;
using UnityEngine.AI;

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
        if ( Finder.GetGameStateManager().IsCurrentState< FormationGameState >() ) { return; }
        if ( Finder.GetGameStateManager().IsCurrentState< FailGameState >() ) { return; }
        if ( Input.GetMouseButtonDown( MouseButtonIndex.Left ) )
        {
            // on hit set desired position
            RaycastHit hit;
            if ( MathUtil.RaycastFromMousePointer( out hit ) == true )
            {
                // add half of player height to desired position, so the distance for idle / walk states are correctly calculated
                m_desiredPosition = hit.point + Finder.GetPlayerHeight() / 2.0f;
            }
        }
        
        if ( Input.GetMouseButtonDown( MouseButtonIndex.Middle ) )
        {
            // on hit set desired position
            RaycastHit hit;
            
            if (MathUtil.RaycastFromMousePointer(out hit) == true)
            {
                var gameObject = hit.transform.gameObject;
                if (gameObject.GetComponent<UnderlingEntity>() != null
                && gameObject.GetComponent<UnderlingEntity>().GetLeader() == null)
                {
                    gameObject.GetComponent<UnderlingEntity>().Resurect();
                }
            }
        }
    }
}
