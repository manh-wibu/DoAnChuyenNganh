using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saus.CoreSystem;

namespace Saus.Utility
{
	/// <summary>
	/// Debug script to monitor CanSetVelocity state
	/// </summary>
	public class CanSetVelocityDebugger : MonoBehaviour
	{
		private Core core;
		private Movement movement;
		private bool lastCanSetVelocity = true;

		private void Start()
		{
			core = GetComponentInChildren<Core>();
			if (core != null)
			{
				movement = core.GetCoreComponent<Movement>();
			}
		}

		private void Update()
		{
			if (movement == null) return;

			// ✅ Log when CanSetVelocity changes
			if (movement.CanSetVelocity != lastCanSetVelocity)
			{
				Debug.Log($"[CanSetVelocityDebugger] CanSetVelocity changed: {lastCanSetVelocity} → {movement.CanSetVelocity}");
				lastCanSetVelocity = movement.CanSetVelocity;
			}

			// ✅ Log if stuck false for too long
			if (!movement.CanSetVelocity)
			{
				Debug.LogWarning($"[CanSetVelocityDebugger] ⚠️ CanSetVelocity is FALSE - Movement is locked!");
			}
		}
	}
}
