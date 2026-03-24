using Saus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Entity
{
    public B1_IdleState idleState { get; private set; }
    public B1_PlayerDetectedState playerDetectedState { get; private set; }
    public B1_MeleeAttackState meleeAttackState { get; private set; }
    public B1_LookForPlayerState lookForPlayerState { get; private set; }
    public B1_StunState stunState { get; private set; }
    public B1_DeadState deadState { get; private set; }
    public B1_RushState rushAttackState { get; private set; }
    public B1_RangedAttackState rangedAttackState { get; private set; }
    public B1_ChargeState chargeState { get; private set; }
    public B1_AOEAttackState aoeAttackState { get; private set; }
    public B1_BlockState blockState { get; private set; }

    private bool hasTriggeredHalfHealthRush = false;

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_PlayerDetected playerDetectedStateData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_LookForPlayer lookForPlayerStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    private D_RangedAttackState rangedAttackStateData;
    [SerializeField]
    private Transform meleeAttackPosition;
    [SerializeField]
    private Transform rangedAttackPosition;
    [SerializeField]
    private D_RushState rushStateData;
    [SerializeField]
    private Transform rushPosition;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_AOEAttackState aoeAttackStateData;
    [SerializeField]
    private Transform aoeAttackPosition;
    [SerializeField]
    private D_BlockState blockStateData;
    [SerializeField]
    private Transform blockPosition;

    public override void Awake()
    {
        base.Awake();

        idleState = new B1_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new B1_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        meleeAttackState = new B1_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        lookForPlayerState = new B1_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        stunState = new B1_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new B1_DeadState(this, stateMachine, "dead", deadStateData, this);
        rangedAttackState = new B1_RangedAttackState(this, stateMachine, "rangedAttack", rangedAttackPosition, rangedAttackStateData, this);
        rushAttackState = new B1_RushState(this, stateMachine, "rushAttack", rushPosition, rushStateData, this);
        chargeState = new B1_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        aoeAttackState = new B1_AOEAttackState(this, stateMachine, "aoeAttack", aoeAttackPosition, aoeAttackStateData, this);
        blockState = new B1_BlockState(this, stateMachine, "block", blockPosition, blockStateData, this);

        stats.Poise.OnCurrentValueZero += HandlePoiseZero;
        stats.Health.OnValueChanged += HandleHealthChanged;
    }

    public override void Update()
    {
        base.Update();
    }

    private void HandlePoiseZero()
    {
        stateMachine.ChangeState(stunState);
    }

    private void HandleHealthChanged(float currentHealth, float maxHealth)
    {
        // Check if health is at or below 50% and hasn't triggered rush attack yet
        if (currentHealth <= maxHealth * 0.5f && !hasTriggeredHalfHealthRush)
        {
            hasTriggeredHalfHealthRush = true;
            stateMachine.ChangeState(rushAttackState);
        }
    }
    protected override void HandleParry()
    {
        base.HandleParry();
        stateMachine.ChangeState(stunState);
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void OnDestroy()
    {
        stats.Poise.OnCurrentValueZero -= HandlePoiseZero;
        stats.Health.OnValueChanged -= HandleHealthChanged;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // Melee Attack - Red
        if (meleeAttackPosition != null && meleeAttackStateData != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
        }

        // Rush Attack - Cyan
        if (rushPosition != null && rushStateData != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(rushPosition.position, rushStateData.rushAttackRadius);
        }

        // AOE Attack - Green
        if (aoeAttackPosition != null && aoeAttackStateData != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(aoeAttackPosition.position, aoeAttackStateData.aoeRadius);
        }

        // Block State - Blue
        if (blockPosition != null && blockStateData != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(blockPosition.position, blockStateData.blockRadius);
        }

        // Reset color
        Gizmos.color = Color.white;
    }
}
