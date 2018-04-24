using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            }
        }
        finally
        {
            enemyDetected = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        var entity = other.gameObject.GetComponent<Entity>();
        if (entity != null)
        {
            if (!entity.IsFriendly())
            {
                enemyDetected = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var gameStateManager = Finder.GetGameStateManager();

        var entity = other.gameObject.GetComponent<Entity>();
        if (entity != null)
        {
            
            if (!entity.IsFriendly())
            {
                if (gameStateManager.GetCurrentState().GetType() != typeof(BattleGameState))
                    EventManager.TriggerEvent("TransitionBattleState");

                enemyDetected = true;
            }
        }
    }
}