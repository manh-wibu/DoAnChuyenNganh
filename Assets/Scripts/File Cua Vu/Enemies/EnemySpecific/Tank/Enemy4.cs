using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : Entity
{
	public E4_IdleState idleState { get; private set; }
	public E4_MoveState moveState { get; private set; }
	public E4_PlayerDetectedState playerDetectedState { get; private set; }
	public E4_LookForPlayerState lookForPlayerState { get; private set; }
	public E4_BlockState blockState { get; private set; }
	public E4_RushState rushState { get; private set; }
	public E4_StunState stunState { get; private set; }
	public E4_DeadState deadState { get; private set; }

	[SerializeField]
	private D_IdleState idleStateData;
	[SerializeField]
	private D_MoveState moveStateData;
	[SerializeField]
	private D_PlayerDetected playerDetectedData;
	[SerializeField]
	private D_LookForPlayer lookForPlayerStateData;
	[SerializeField]
	private D_BlockState blockStateData;
	[SerializeField]
	private D_RushState rushStateData;
	[SerializeField]
	private D_StunState stunStateData;
	[SerializeField]
	private D_DeadState deadStateData;


	[SerializeField]
	private Transform blockPosition;
	[SerializeField]
	private Transform rushPosition;

	public override void Awake()
	{
		base.Awake();

		moveState = new E4_MoveState(this, stateMachine, "move", moveStateData, this);
		idleState = new E4_IdleState(this, stateMachine, "idle", idleStateData, this);
		playerDetectedState = new E4_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
		lookForPlayerState = new E4_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
		blockState = new E4_BlockState(this, stateMachine, "blockAttack", blockPosition, blockStateData, this);
		rushState = new E4_RushState(this, stateMachine, "rushAttack", rushPosition, rushStateData, this);
		stunState = new E4_StunState(this, stateMachine, "stun", stunStateData, this);
		deadState = new E4_DeadState(this, stateMachine, "dead", deadStateData, this);

		stats.Poise.OnCurrentValueZero += HandlePoiseZero;
	}

	private void HandlePoiseZero()
	{
		stateMachine.ChangeState(stunState);
	}


	private void Start()
	{
		stateMachine.Initialize(moveState);
	}

	private void OnDestroy()
	{
		stats.Poise.OnCurrentValueZero -= HandlePoiseZero;
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();

		Gizmos.DrawWireSphere(blockPosition.position, blockStateData.blockRadius);
	}
}
