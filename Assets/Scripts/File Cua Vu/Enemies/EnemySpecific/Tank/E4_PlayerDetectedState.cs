using Saus.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E4_PlayerDetectedState : PlayerDetectedState
{
	private Movement Movement
	{
		get => movement ?? core.GetCoreComponent(ref movement);
	}

	private CollisionSenses CollisionSenses
	{
		get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
	}

	private Movement movement;
	private CollisionSenses collisionSenses;
	private Enemy4 enemy;
	public E4_PlayerDetectedState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData, Enemy4 enemy) : base(etity, stateMachine, animBoolName, stateData)
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

		if (performCloseRangeAction)
		{
			stateMachine.ChangeState(enemy.blockState);
		}
		else if (performLongRangeAction)
		{
			stateMachine.ChangeState(enemy.rushState);
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
