using System;
using Saus.Combat.Mana;
using Saus.CoreSystem.StatsSystem;
using Saus.ModifierSystem;
using UnityEngine;

namespace Saus.CoreSystem
{
    public class Mana : CoreComponent
    {
		private Stats stats;
		public Modifiers<Modifier<ManaData>, ManaData> Modifiers { get; } = new();


		//public bool HasEnoughMana(float requiredAmount)
		//{
		//	bool hasEnough = stats.Mana.CurrentValue >= requiredAmount;
		//	if (!hasEnough)
		//	{
		//		Debug.LogWarning($"[Mana] Insufficient mana! Required: {requiredAmount}, Available: {stats.Mana.CurrentValue}");
		//	}
		//	return hasEnough;
		//}

		//public bool TryConsumeMana(float amount)
		//{
		//	if (!HasEnoughMana(amount))
		//	{
		//		return false;
		//	}

		//	stats.Mana.Decrease(amount);
		//	Debug.Log($"[Mana] Consumed {amount}. Remaining: {stats.Mana.CurrentValue}");
		//	return true;
		//}

		public void ManaCost(ManaData data)
		{
			data = Modifiers.ApplyAllModifiers(data);
			Debug.Log("Mana Decrease" + data );
			stats.Mana.Decrease(data.Amount);
		}

		public void ManaReCharge(ManaData data)
		{
			data = Modifiers.ApplyAllModifiers(data);
			stats.Mana.Increase(data.Amount);
		}

		//public float GetCurrentMana() => stats.Mana.CurrentValue;

		//public float GetMaxMana() => stats.Mana.MaxValue;

		protected override void Awake()
		{
			base.Awake();

			stats = core.GetCoreComponent<Stats>();
			if (stats == null)
			{
				Debug.LogError("[Mana] Stats component not found in Core!");
			}
		}
	}
}
