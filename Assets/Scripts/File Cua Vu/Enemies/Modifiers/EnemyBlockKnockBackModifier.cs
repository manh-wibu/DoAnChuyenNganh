using Saus.Combat.KnockBack;
using Saus.ModifierSystem;
using System;
using UnityEngine;

namespace Saus.Enemies.Modifiers
{

    public class EnemyBlockKnockBackModifier : Modifier<KnockBackData>
    {
        private readonly float knockBackReductionPercent;
        private readonly Func<bool> isBlockActive;

        public EnemyBlockKnockBackModifier(float knockBackReductionPercent, Func<bool> isBlockActive)
        {
            this.knockBackReductionPercent = Mathf.Clamp01(knockBackReductionPercent);
            this.isBlockActive = isBlockActive;
        }

        public override KnockBackData ModifyValue(KnockBackData value)
        {
            if (isBlockActive != null && isBlockActive.Invoke())
            {
                value.Strength *= (1f - knockBackReductionPercent);
                Debug.Log($"[EnemyBlockKnockBackModifier] Block active! Reduced knockback to: {value.Strength}");
            }

            return value;
        }
    }
}
