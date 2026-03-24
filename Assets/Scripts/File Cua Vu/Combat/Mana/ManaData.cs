using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Saus.Combat.Mana
{
	public class ManaData
	{
		public float Amount { get; private set; }

		public ManaData(float amount)
		{
			Amount = amount;
		}

		public void SetAmount(float amount) => Amount = amount;
	}
}
