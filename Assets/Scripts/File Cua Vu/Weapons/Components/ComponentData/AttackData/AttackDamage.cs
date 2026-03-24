using System;
using UnityEngine;

namespace Saus.Weapons.Components
{
    [Serializable]
    public class AttackDamage : AttackData
    {
        [field: SerializeField] public float Amount { get; private set; }
    }
}