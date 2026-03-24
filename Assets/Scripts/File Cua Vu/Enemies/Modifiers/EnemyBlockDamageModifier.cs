using Saus.Combat.Damage;
using Saus.ModifierSystem;
using System;
using UnityEngine;

namespace Saus.Enemies.Modifiers
{
    public class EnemyBlockDamageModifier : Modifier<DamageData>
    {
        private readonly float damageReductionPercent;
        private readonly Func<bool> isBlockActive;

        public EnemyBlockDamageModifier(float damageReductionPercent, Func<bool> isBlockActive)
        {
            this.damageReductionPercent = Mathf.Clamp01(damageReductionPercent);
            this.isBlockActive = isBlockActive;
        }

        public override DamageData ModifyValue(DamageData value)
        {
            if (isBlockActive != null && isBlockActive.Invoke())
            {
                value.SetAmount(value.Amount * (1f - damageReductionPercent));
                Debug.Log($"[EnemyBlockDamageModifier] Block active! Reduced damage to: {value.Amount}");
            }

            return value;
        }
    }
}
