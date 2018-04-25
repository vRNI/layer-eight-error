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
                {

                    EventManager.TriggerEvent("TransitionBattleState");
                }

                if (entity.GetComponent<EntityStateManager>().IsCurrentState<WalkEntityState>()
                    || entity.GetComponent<EntityStateManager>().IsCurrentState<IdleEntityState>())
                {
                    entity.GetComponent<EntityStateManager>().IsCurrentState<CombatEntityState>();
                    // set unit state to battle
                    var underlings = other.gameObject.GetComponent<FormationConfiguration>().GetUnderlingUnits();
                    foreach (var underling in underlings)
                    {
                        if (underling.GetComponent<EntityStateManager>().IsCurrentState<WalkEntityState>()
                            || underling.GetComponent<EntityStateManager>().IsCurrentState<IdleEntityState>())
                        {
                            underling.GetComponent<EntityStateManager>().SetCurrentState<CombatEntityState>();
                        }
                    }
                }

                enemyDetected = true;
            }
        }
    }
}