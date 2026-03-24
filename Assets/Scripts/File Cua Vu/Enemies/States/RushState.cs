using Saus.Combat.Damage;
using Saus.Combat.KnockBack;
using Saus.Combat.PoiseDamage;
using Saus.CoreSystem;
using Saus.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushState : AttackState
{
	private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
	private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

	private Movement movement;
	private CollisionSenses collisionSenses;

	protected D_RushState stateData;
	
	private float rushTimeCounter;
	private float rushTravelDistance;
	private bool isRushing;

	public RushState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RushState stateData) : base(etity, stateMachine, animBoolName, attackPosition)
	{
		this.stateData = stateData;
	}

	public override void DoChecks()
	{
		base.DoChecks();
	}

	public override void Enter()
	{
		base.Enter();
		
		rushTimeCounter = 0f;
		rushTravelDistance = 0f;
		isRushing = true;
	}

	public override void Exit()
	{
		base.Exit();
		isRushing = false;
		Movement?.SetVelocityX(0f);
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();

		if (isRushing)
		{
			rushTimeCounter += Time.deltaTime;
			
			float rushVelocity = stateData.rushSpeed * Movement.FacingDirection;
			Movement?.SetVelocityX(rushVelocity);
			
			rushTravelDistance += Mathf.Abs(stateData.rushSpeed * Time.deltaTime);

			if (rushTravelDistance >= stateData.rushDistance)
			{
				isRushing = false;
				Movement?.SetVelocityX(0f);
			}

			TriggerAttack();
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}

	public override void TriggerAttack()
	{
		base.TriggerAttack();

		Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(
			attackPosition.position, 
			stateData.rushAttackRadius, 
			stateData.whatIsPlayer
		);

		foreach (Collider2D collider in detectedObjects)
		{
			IDamageable damageable = collider.GetComponent<IDamageable>();
			if (damageable != null)
			{
				damageable.Damage(new DamageData(stateData.rushDamage, core.Root));
			}

			IKnockBackable knockbackable = collider.GetComponent<IKnockBackable>();
			if (knockbackable != null)
			{
				knockbackable.KnockBack(new KnockBackData(stateData.knockbackAngle, stateData.knockbackStrength, Movement.FacingDirection, core.Root));
			}
			if (collider.TryGetComponent(out IPoiseDamageable poiseDamageable))
			{
				poiseDamageable.DamagePoise(new PoiseDamageData(stateData.PoiseDamage, core.Root));
			}

			isRushing = false;
			Movement?.SetVelocityX(0f);
		}
	}

	public bool IsRushing => isRushing;
}
