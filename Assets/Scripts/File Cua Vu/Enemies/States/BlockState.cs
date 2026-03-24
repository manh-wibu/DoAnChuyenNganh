using Saus.Combat.Damage;
using Saus.Combat.KnockBack;
using Saus.Combat.PoiseDamage;
using Saus.CoreSystem;
using Saus.Enemies.Modifiers;
using Saus.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockState : AttackState
{
	private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
	private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

	private DamageReceiver DamageReceiver { get => damageReceiver ?? core.GetCoreComponent(ref damageReceiver); }
	private KnockBackReceiver KnockBackReceiver { get => knockBackReceiver ?? core.GetCoreComponent(ref knockBackReceiver); }
	private PoiseDamageReceiver PoiseDamageReceiver { get => poiseDamageReceiver ?? core.GetCoreComponent(ref poiseDamageReceiver); }

	private Movement movement;
	private CollisionSenses collisionSenses;
	private DamageReceiver damageReceiver;
	private KnockBackReceiver knockBackReceiver;
	private PoiseDamageReceiver poiseDamageReceiver;

	protected D_BlockState stateData;
	private float blockTimeCounter;
	private bool isBlockActive;

	protected EnemyBlockDamageModifier blockDamageModifier;
	protected EnemyBlockKnockBackModifier blockKnockBackModifier;
	protected EnemyBlockPoiseDamageModifier blockPoiseDamageModifier;

	public BlockState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_BlockState stateData) : base(etity, stateMachine, animBoolName, attackPosition)
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
		
		blockTimeCounter = 0f;
		isBlockActive = true;
		Movement?.SetVelocityX(0f);

		ApplyBlockModifiers();
	}

	public override void Exit()
	{
		base.Exit();
		isBlockActive = false;
		RemoveBlockModifiers();
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		
		Movement?.SetVelocityX(0f);
		
		blockTimeCounter += Time.deltaTime;
		
		if (blockTimeCounter >= stateData.blockDuration)
		{
			isBlockActive = false;
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}

	public override void TriggerAttack()
	{
		base.TriggerAttack();

		if (!isBlockActive) return;

		Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.blockRadius, stateData.whatIsPlayer);

		foreach (Collider2D collider in detectedObjects)
		{
			Vector2 directionToAttacker = (collider.transform.position - attackPosition.position).normalized;
			Vector2 facingDirection = new Vector2(Movement.FacingDirection, 0);

			float angle = Vector2.Angle(facingDirection, directionToAttacker);

			if (angle <= stateData.blockAngleRange / 2f)
			{
				IDamageable damageable = collider.GetComponent<IDamageable>();
				if (damageable != null)
				{
					//damageable.Damage(stateData.blockDuration * (1f - stateData.damageReductionPercent));
				}
			}
		}
	}

	public bool IsBlockActive => isBlockActive;

	/// <summary>
	/// Applies damage reduction modifiers when block becomes active.
	/// Creates modifiers with the configured reduction percentages from D_BlockState.
	/// </summary>
	protected virtual void ApplyBlockModifiers()
	{
		// Initialize modifiers on first use (lazy initialization)
		if (blockDamageModifier == null)
		{
			blockDamageModifier = new EnemyBlockDamageModifier(
				stateData.damageReductionPercent,
				() => IsBlockActive
			);
		}

		if (blockKnockBackModifier == null)
		{
			blockKnockBackModifier = new EnemyBlockKnockBackModifier(
				stateData.knockbackReductionPercent,
				() => IsBlockActive
			);
		}

		if (blockPoiseDamageModifier == null)
		{
			blockPoiseDamageModifier = new EnemyBlockPoiseDamageModifier(
				stateData.poiseReductionPercent,
				() => IsBlockActive
			);
		}

		// Add modifiers to receivers - they will modify incoming damage
		DamageReceiver?.Modifiers.AddModifier(blockDamageModifier);
		KnockBackReceiver?.Modifiers.AddModifier(blockKnockBackModifier);
		PoiseDamageReceiver?.Modifiers.AddModifier(blockPoiseDamageModifier);

		Debug.Log($"[BlockState] Applied block modifiers. Damage Reduction: {stateData.damageReductionPercent * 100}%");
	}

	/// <summary>
	/// Removes damage reduction modifiers when block ends.
	/// This allows the enemy to take full damage again after blocking.
	/// </summary>
	protected virtual void RemoveBlockModifiers()
	{
		if (blockDamageModifier != null)
			DamageReceiver?.Modifiers.RemoveModifier(blockDamageModifier);

		if (blockKnockBackModifier != null)
			KnockBackReceiver?.Modifiers.RemoveModifier(blockKnockBackModifier);

		if (blockPoiseDamageModifier != null)
			PoiseDamageReceiver?.Modifiers.RemoveModifier(blockPoiseDamageModifier);

		Debug.Log("[BlockState] Removed block modifiers. Enemy now takes full damage again.");
	}
}
