using System;
using Saus.CoreSystem.StatsSystem;
using UnityEngine;

namespace Saus.CoreSystem
{
    public class Stats : CoreComponent
    {
       [field: SerializeField] public Stat Health { get; private set; }
       [field: SerializeField] public Stat Poise { get; private set; }
        [field: SerializeField] public Stat Mana { get; private set; }

       [SerializeField] private float poiseRecoveryRate;
       [SerializeField] private float manaRecoveryRate;
        public event Action<float, float> OnValueChanged;
        protected override void Awake()
        {
            base.Awake();
            
            Health.Init();
            Poise.Init();
            Mana.Init();
        }

        private void Update()
        {
            if (Poise.CurrentValue.Equals(Poise.MaxValue))
                return;
            
            Poise.Increase(poiseRecoveryRate * Time.deltaTime);

            if(Mana.CurrentValue.Equals(Mana.MaxValue))
				return;

			Mana.Increase(manaRecoveryRate * Time.deltaTime);
		}
        public void Heal(float amount)
        {
            Health.Increase(amount);
        }
    }
    
}
