using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekFormationSlotPositionState : EntityState {
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (m_entity.HasNavMeshStoppedMoving())
        {
            m_entity.SetCurrentState<WalkEntityState>();
        }
    }
}