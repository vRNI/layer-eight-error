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

    protected override void Die()
    {
        // change to fail state
        Finder.GetGameStateManager().GetCurrentState().TriggerTransition< FailGameState >();
        //// delete myself
        //Object.Destroy( gameObject );
    }
}
