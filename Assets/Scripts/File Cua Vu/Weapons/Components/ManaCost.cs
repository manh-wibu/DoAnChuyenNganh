using Saus.Combat.Mana;
using Saus.CoreSystem;
using UnityEngine;

namespace Saus.Weapons.Components
{
    public class ManaCost : WeaponComponent<ManaCostData, AttackMana>
    {
        private Mana manaCore;
        private void HandleAttackAction()
        {
            IManaChangeable[] manaChangeables = Core.GetComponents<IManaChangeable>();
			foreach (var manaChangeable in manaChangeables)
			{
				manaChangeable.ManaChange(new ManaData(currentAttackData.ManaCost));
			}
		}
        protected override void Start()
        {
            base.Start();
            manaCore = Core.GetCoreComponent<Mana>();

            if (manaCore == null)
            {
                Debug.LogError("[ManaCost] Mana component not found in player's Core! Mana cost system disabled.");
                enabled = false;
                return;
            }

            AnimationEventHandler.OnAttackAction += HandleAttackAction;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            AnimationEventHandler.OnAttackAction -= HandleAttackAction;
        }
    }
}
