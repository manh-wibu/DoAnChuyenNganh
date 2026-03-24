using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_MeleeAttackState : MeleeAttackState
{
    private Boss enemy;
    public B1_MeleeAttackState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, Boss enemy) : base(etity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if (performCloseRangeAction)
            {
				stateMachine.ChangeState(enemy.blockState);
			} 
            else if(isPlayerInMinAgroRange)
            {
				stateMachine.ChangeState(enemy.playerDetectedState);
			}
            else
            {
				stateMachine.ChangeState(enemy.lookForPlayerState);
			}
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }
}
