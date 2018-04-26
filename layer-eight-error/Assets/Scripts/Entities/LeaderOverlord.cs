using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderOverlord
    : LeaderEntity
{
    public override bool IsDead()
    {
        return m_healthPoints <= 0;
    }

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //void Die()
    //{

    //}
}
