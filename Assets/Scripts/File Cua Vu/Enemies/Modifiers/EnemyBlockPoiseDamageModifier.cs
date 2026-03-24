using Saus.Combat.PoiseDamage;
using Saus.ModifierSystem;
using System;
using UnityEngine;

namespace Saus.Enemies.Modifiers
{

    public class EnemyBlockPoiseDamageModifier : Modifier<PoiseDamageData>
    {
        private readonly float poiseReductionPercent;
        private readonly Func<bool> isBlockActive;

        public EnemyBlockPoiseDamageModifier(float poiseReductionPercent, Func<bool> isBlockActive)
        {
            this.poiseReductionPercent = Mathf.Clamp01(poiseReductionPercent);
            this.isBlockActive = isBlockActive;
        }

        public override PoiseDamageData ModifyValue(PoiseDamageData value)
        {
            if (isBlockActive != null && isBlockActive.Invoke())
            {
                value.SetAmount(value.Amount * (1f - poiseReductionPercent));
                Debug.Log($"[EnemyBlockPoiseDamageModifier] Block active! Reduced poise damage to: {value.Amount}");
            }

            return value;
        }
    }
}
