using System.Collections;
using System.Collections.Generic;
using Saus.Combat.Damage;
using Saus.Combat.KnockBack;
using Saus.CoreSystem;
using UnityEngine;

public class AOEAttackState : AttackState
{
	private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
	private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

	private Movement movement;
	private CollisionSenses collisionSenses;

	protected D_AOEAttackState stateData;

	public AOEAttackState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_AOEAttackState stateData)
		: base(etity, stateMachine, animBoolName, attackPosition)
	{
		this.stateData = stateData;
	}

	public override void TriggerAttack()
	{
		base.TriggerAttack();

		// Get all entities in AOE radius
		Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.aoeRadius, stateData.whatIsPlayer);

		foreach (Collider2D collider in detectedObjects)
		{
			IDamageable damageable = collider.GetComponent<IDamageable>();

			if (damageable != null)
			{
				damageable.Damage(new DamageData(stateData.damage, core.Root));
			}

			IKnockBackable knockBackable = collider.GetComponent<IKnockBackable>();

			if (knockBackable != null)
			{
				knockBackable.KnockBack(new KnockBackData(stateData.knockbackAngle, stateData.knockbackStrength, Movement.FacingDirection, core.Root));
			}
		}
	}
}
