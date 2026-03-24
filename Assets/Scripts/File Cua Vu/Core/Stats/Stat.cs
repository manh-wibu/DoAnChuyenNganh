using System;
using UnityEngine;

namespace Saus.CoreSystem.StatsSystem
{
    [Serializable]
    public class Stat
    {
        public event Action OnCurrentValueZero;
        public event Action<float, float> OnValueChanged;
        [field: SerializeField] public float MaxValue { get; private set; }

        public float CurrentValue
    {
        get => currentValue;
        set
        {
            currentValue = Mathf.Clamp(value, 0f, MaxValue);
            
            // GỌI UPDATE UI Ở ĐÂY
            OnValueChanged?.Invoke(currentValue, MaxValue);

            if (currentValue <= 0f)
                OnCurrentValueZero?.Invoke();
        }
    }
        
        private float currentValue;

        public void Init() => CurrentValue = MaxValue;

        public void Increase(float amount) => CurrentValue += amount;

        public void Decrease(float amount) => CurrentValue -= amount;
    }
}