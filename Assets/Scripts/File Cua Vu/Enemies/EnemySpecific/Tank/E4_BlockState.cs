using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E4_BlockState : BlockState
{
	private Enemy4 enemy;
	public E4_BlockState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_BlockState stateData, Enemy4 enemy) : base(etity, stateMachine, animBoolName, attackPosition, stateData)
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

		if (!IsBlockActive)
		{
			if (isPlayerInMinAgroRange)
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
}
