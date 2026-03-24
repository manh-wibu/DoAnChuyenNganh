using System;
using UnityEngine;

namespace Saus.Weapons.Components
{
	[Serializable]
	public class AttackMana : AttackData
	{
		[field: SerializeField] public float ManaCost { get; private set; }
	}
}
