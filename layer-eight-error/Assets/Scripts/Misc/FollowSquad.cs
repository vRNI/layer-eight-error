using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSquad : MonoBehaviour {

    private FormationConfiguration m_formationConfiguration;
	// Use this for initialization
	void Start () {
        m_formationConfiguration = GetComponent<FormationConfiguration>();
	}
	
	// Update is called once per frame
	void Update () {
        var newPosition = CalculateAveragePositionViaUnderlings();
        // set leader position as average of alive underlings
        transform.position = newPosition;
	}

    Vector3 CalculateAveragePositionViaUnderlings()
    {
        var underlings = m_formationConfiguration.GetUnderlingUnits();
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach(UnderlingEntity underling in underlings)
        {
            // is underling alive? -> don't consider if underling is dead
            sum += underling.GetWorldPosition();
            count++;
        }

        if ( count == 0 ) { return Vector3.zero; }

        sum /= count;

        return sum;
    }
}
