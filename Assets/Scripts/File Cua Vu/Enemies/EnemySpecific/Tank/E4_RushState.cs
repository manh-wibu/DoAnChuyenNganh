using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E4_RushState : RushState
{
	private Enemy4 enemy;
	public E4_RushState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RushState stateData, Enemy4 enemy) 
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

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (!IsRushing)
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
	public override void TriggerAttack()
	{
		base.TriggerAttack();
	}

	public override void FinishAttack()
	{
		base.FinishAttack();
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
