using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEntityState
    : EntityState
{
    // state pattern that determines what strategy we use for attacking (fighter, archer)

    public override void Enter()
    {
        base.Enter();
        // shout animation
    }

    public override void Exit()
    {
        base.Exit();
        // exit animation
    }

    public override void Update()
    {
        base.Update();
        // invoke object -> we need a way to know what entity is closest
    }
}