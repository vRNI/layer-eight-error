using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ DisallowMultipleComponent ]
[ RequireComponent( typeof( FormationBoundsUpdater ) ) ]
public class EnemyDetector : MonoBehaviour
{
    bool enemyDetected = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        try
        {
            var gameStateManager = Finder.GetGameStateManager();
            if (gameStateManager.GetCurrentState().GetType() == typeof(IdleGameState)) return;

            if (!enemyDetected)
            {
                EventManager.TriggerEvent("TransitionIdleState");
                // change back from battle state
                Finder.GetPlayer().GetComponent<LeaderOverlord>().SetUnderlingsHostility(false);
            }
        }
        finally
        {
            enemyDetected = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        var gameStateManager = Finder.GetGameStateManager();

        if ( gameStateManager.IsCurrentState< IdleGameState >() )
        {
            var followSquad     = other.gameObject.GetComponent< FollowSquad >();
            if ( followSquad == null ) { return; }
            var formationLeader = followSquad.GetFormationLeader();
            if ( formationLeader == null ) { return; }
            // Get Leader State Machine -> is not null;
            var entity = formationLeader.GetComponent<LeaderEntity>();
            if (entity != null && entity.IsDead() == false)
            {
                if (entity != Finder.GetPlayer())
                {
                    if (gameStateManager.GetCurrentState().GetType() != typeof(BattleGameState))
                    {
                        EventManager.TriggerEvent("TransitionBattleState");
                        Finder.GetPlayer().GetComponent<LeaderOverlord>().SetUnderlingsHostility(true);
                    }

                    if (!entity.IsHostile)
                        entity.SetUnderlingsHostility(true);

                    enemyDetected = true;
                }
            }
        }
        else
        {
            var entity = other.gameObject.GetComponent<LeaderEntity>();
            if (entity != null && entity.IsDead() == false)
            {
                if (other.gameObject != Finder.GetPlayer())
                {
                    enemyDetected = true;
                }
            }
        }
    }
}
