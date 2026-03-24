using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3_PlayerDetectedState : PlayerDetectedState
{
	private Enemy3 enemy;

	public E3_PlayerDetectedState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData, Enemy3 enemy) : base(etity, stateMachine, animBoolName, stateData)
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

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (performCloseRangeAction)
		{
			if (enemy.dodgeState.CanDodge())
			{
				stateMachine.ChangeState(enemy.dodgeState);
			}
			else
			{
				stateMachine.ChangeState(enemy.meleeAttackState);
			}
		}
		else if (performLongRangeAction)
		{
			stateMachine.ChangeState(enemy.rangedAttackState);
		}
		else if (!isPlayerInMaxAgroRange)
		{
			stateMachine.ChangeState(enemy.lookForPlayerState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
	
}
