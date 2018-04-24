using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGameState
    : GameState
{
    public override void Enter()
    {
        base.Enter();

        var entities = Finder.GetEntities();
        foreach (var entity in entities)
        {
            if (entity.GetComponent<EntityStateManager>().IsCurrentState<WalkEntityState>()
                || entity.GetComponent<EntityStateManager>().IsCurrentState<IdleEntityState>())
            {
                entity.GetComponent<EntityStateManager>().SetCurrentState<CombatEntityState>();
            }

        }
    }
    public override void Update()
    {
        base.Update();

        var gameStateManager = Finder.GetGameStateManager();

        // todo. check if enemies !nearby
        if (false)
        {
            gameStateManager.SetCurrentState<IdleGameState>();
            return;
        }
    }
}
