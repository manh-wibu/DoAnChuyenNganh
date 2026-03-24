using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E4_StunState : StunState
{
	private Enemy4 enemy;
	public E4_StunState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData, Enemy4 enemy) : base(etity, stateMachine, animBoolName, stateData)
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

		if (isStunTimeOver)
		{
			if (performCloseRangeAction)
			{
				stateMachine.ChangeState(enemy.blockState);
			}
			else if (isPlayerInMinAgroRange)
			{
				stateMachine.ChangeState(enemy.rushState);
			}
			else
			{
				enemy.lookForPlayerState.SetTurnImmediately(true);
				stateMachine.ChangeState(enemy.lookForPlayerState);
			}
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
