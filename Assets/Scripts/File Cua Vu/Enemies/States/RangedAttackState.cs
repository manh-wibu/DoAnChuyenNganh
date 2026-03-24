using System.Collections;
using System.Collections.Generic;
using Saus.ObjectPoolSystem;
using Saus.ProjectileSystem;
using UnityEngine;

/// <summary>
/// Base Ranged Attack State for enemies using the ProjectileSystem.
/// This uses ObjectPools for efficient projectile spawning, just like the Player's ProjectileSpawner.
/// Senior Dev Note: By using ObjectPools and the ProjectileSystem directly, we avoid
/// expensive Instantiate() calls and reuse the Player's proven architecture.
/// </summary>
public class RangedAttackState : AttackState
{
    protected D_RangedAttackState stateData;

    // ObjectPool for efficient projectile spawning
    protected readonly ObjectPools objectPools = new ObjectPools();

    public RangedAttackState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangedAttackState stateData) : base(etity, stateMachine, animBoolName, attackPosition)
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
	}

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        // Debug log
        // Use ObjectPool to get projectile (efficient and reusable)
        Projectile projectile = objectPools.GetObject(stateData.projectilePrefab);
        
        if (projectile == null)
        {
            Debug.LogError("[RangedAttackState] Failed to get projectile from pool!");
            return;
        }

        
        // Store original sprite before Reset
        SpriteRenderer[] renderersBeforeReset = projectile.GetComponentsInChildren<SpriteRenderer>();
        Sprite[] originalSprites = new Sprite[renderersBeforeReset.Length];
        for (int i = 0; i < renderersBeforeReset.Length; i++)
        {
            originalSprites[i] = renderersBeforeReset[i].sprite;
        }
        
        // Set position and rotation
        projectile.transform.position = attackPosition.position;
        projectile.transform.rotation = attackPosition.rotation;
        
        // Draw gizmo to visualize spawn position
        Debug.DrawLine(attackPosition.position, attackPosition.position + Vector3.up, Color.red, 2f);
        
        // Reset projectile state
        projectile.Reset();

        // Send data packages using the Player's same system
        if (stateData.damageData != null)
        {
            projectile.SendDataPackage(stateData.damageData);
        }
        
        if (stateData.knockBackData != null)
            projectile.SendDataPackage(stateData.knockBackData);
        
        if (stateData.poiseDamageData != null)
            projectile.SendDataPackage(stateData.poiseDamageData);

        // Initialize the projectile (this activates components and enables graphics)
        projectile.Init();
        
        // Restore sprites and enable all graphics renderers after Reset
        SpriteRenderer[] renderers = projectile.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < renderers.Length && i < originalSprites.Length; i++)
        {
            renderers[i].sprite = originalSprites[i];
            renderers[i].enabled = true;
        }
        
        // Ensure scale is correct (not 0)
        projectile.transform.localScale = Vector3.one;
        
        // Log final state

	}
}
