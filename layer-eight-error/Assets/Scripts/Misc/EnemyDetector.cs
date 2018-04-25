﻿using System.Collections;
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
                // change
                gameObject.GetComponent<LeaderOverlord>().SetUnderlingsHostility(false);
            }
        }
        finally
        {
            enemyDetected = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        var entity = other.gameObject.GetComponent<LeaderEntity>();
        if (entity != null)
        {
            if (other.gameObject != Finder.GetPlayer())
            {
                enemyDetected = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var gameStateManager = Finder.GetGameStateManager();

        // Get Leader State Machine -> is not null;
        var entity = other.gameObject.GetComponent<LeaderEntity>();
        if (entity != null)
        {
            if (other.gameObject != Finder.GetPlayer())
            {
                if (gameStateManager.GetCurrentState().GetType() != typeof(BattleGameState))
                {
                    EventManager.TriggerEvent("TransitionBattleState");
                    Finder.GetPlayer().GetComponent<LeaderOverlord>().SetUnderlingsHostility(true);
                }

                if (!other.gameObject.GetComponent<LeaderEntity>().IsHostile)
                    other.gameObject.GetComponent<LeaderEntity>().SetUnderlingsHostility(true);

                enemyDetected = true;
            }
        }
    }
}