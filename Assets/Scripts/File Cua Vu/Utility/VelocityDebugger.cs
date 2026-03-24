using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saus.Utility
{
	/// <summary>
	/// Debug script to monitor Player velocity and identify jump issues
	/// </summary>
	public class VelocityDebugger : MonoBehaviour
	{
		private Rigidbody2D rb;
		private float lastYVelocity = 0f;
		private int frameCounter = 0;

		private void Start()
		{
			rb = GetComponent<Rigidbody2D>();
		}

		private void FixedUpdate()
		{
			frameCounter++;

			// ✅ Log every 30 frames (~0.5 seconds at 60 FPS)
			if (frameCounter >= 30)
			{
				frameCounter = 0;

				float currentYVelocity = rb.velocity.y;
				float deltaY = currentYVelocity - lastYVelocity;
				
				Debug.Log($"[VelocityDebugger] " +
					$"Y: {currentYVelocity:F2} " +
					$"| ΔY: {deltaY:F2} " +
					$"| Gravity: {rb.gravityScale} " +
					$"| Constraints: {rb.constraints}");

				lastYVelocity = currentYVelocity;
			}
		}
	}
}
