﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleGameState : GameState {

    static UnityAction listener;

    public BattleGameState()
    {
        listener = new UnityAction(TriggerTransition<IdleGameState>);
    }

    public override void Enter()
    {
        EventManager.StartListening("TransitionIdleState", listener);
    }

    public override void Exit()
    {
        EventManager.StopListening("TransitionIdleState", listener);
    }

    public override void Update()
    {
        base.Update();
    }
}