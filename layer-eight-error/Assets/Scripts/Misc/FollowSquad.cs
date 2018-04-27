using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ RequireComponent( typeof( FormationBoundsUpdater ) ) ]
public class FollowSquad : MonoBehaviour {
    
    [ Tooltip( "The formation leader that should be followed." ) ]
    [ SerializeField ]
    private GameObject m_formationLeader;

    public void SetFormationLeader(GameObject a_formationLeader)
    {
        m_formationLeader = a_formationLeader;
    }

    public GameObject GetFormationLeader()
    {
        return m_formationLeader;
    }

    // Update is called once per frame
    private void Update ()
    {
        if ( m_formationLeader == null )
        {
            Object.Destroy( gameObject );
            return;
        }

        // set leader position as average of alive underlings
        transform.position = CalculateAveragePositionViaUnderlings();
        transform.rotation = m_formationLeader.GetComponent< Transform >().rotation;
    }

    private Vector3 CalculateAveragePositionViaUnderlings()
    {
        var underlings = m_formationLeader.GetComponent< FormationConfiguration >().GetUnderlingUnits();
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach(UnderlingEntity underling in underlings)
        {
            // is underling alive? -> don't consider if underling is dead
            sum += underling.GetWorldPosition();
            count++;
        }

        if ( count == 0 ) { return m_formationLeader.GetComponent< Transform >().position; }

        sum /= count;

        return sum;
    }

    public static void UpdateFormationBoundingBox( GameObject a_formationLeader )
    {
        var followSquads               = Object.FindObjectsOfType< FollowSquad >();
        var followSquad                = followSquads.SingleOrDefault( a_x => a_x.GetFormationLeader() == a_formationLeader );
        if ( followSquad != null )
        {
            var formationBoundsUpdater = followSquad.GetComponent< FormationBoundsUpdater >();
            if ( formationBoundsUpdater != null )
            {
                formationBoundsUpdater.UpdateFormationBoundingBox();
            }
        }
    }
}
