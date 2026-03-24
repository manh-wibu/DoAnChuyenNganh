using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E4_IdleState : IdleState
{
	private Enemy4 enemy;
	// Start is called before the first frame update
	public E4_IdleState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Enemy4 enemy) : base(etity, stateMachine, animBoolName, stateData)
	{
		this.enemy = enemy;
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
		if (isPlayerInMinAgroRange)
		{
			stateMachine.ChangeState(enemy.playerDetectedState);
		}
		else if (isIdleTimeOver)
		{
			stateMachine.ChangeState(enemy.moveState);
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
