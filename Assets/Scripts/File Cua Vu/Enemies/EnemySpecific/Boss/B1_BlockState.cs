using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_BlockState : BlockState
{
    private Boss enemy;

    public B1_BlockState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_BlockState stateData, Boss enemy) 
        : base(etity, stateMachine, animBoolName, attackPosition, stateData)
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

        if (!IsBlockActive)
        {
            if (performCloseRangeAction)
            {
                stateMachine.ChangeState(enemy.aoeAttackState);
            }
            else if (isPlayerInMinAgroRange)
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
