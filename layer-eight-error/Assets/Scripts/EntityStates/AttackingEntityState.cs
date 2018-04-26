using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingEntityState : EntityState
{
    float timeToExit = 1;
    float timePassed = 0;

    public override void Enter()
    {
        base.Enter();

        timeToExit = m_entity.AttackDuration;

        //var target = Finder.GetEntityManager().GetNearestEntity(m_entity.GetWorldPosition(), m_entity.IsFriendly);
        //m_entity.SetTarget(target);
        /// we assume we already know the target
        m_entity.StopWalking();
        m_entity.GetComponent<AnimationInfo>().TriggerAttack();
        m_entity.AttackTarget();
        // kill momentum
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        // attack finished
        if (timePassed >= timeToExit)
        {
            m_entity.SetCurrentState<WalkEntityState>();
            timePassed = 0;
            return;
        }

        // continue waiting
        base.Update();
        // play attack();

        timePassed += Time.deltaTime;
    }
}