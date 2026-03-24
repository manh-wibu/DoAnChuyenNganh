using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRushStateData", menuName = "Data/State Data/Rush Attack State")]
public class D_RushState : ScriptableObject
{
	[Header("Rush Settings")]
	public float rushSpeed = 20f;           // Tốc độ lướt
	public float rushDistance = 10f;        // Khoảng cách lướt tối đa
	public float rushDamage = 15f;          // Damage khi đụng player

	[Header("Rush Detection")]
	public float rushAttackRadius = 0.5f;   // Bán kính detect khi lướt
	public LayerMask whatIsPlayer;          // Layer của player

	[Header("Knockback")]
	public Vector2 knockbackAngle = Vector2.one;
	public float knockbackStrength = 15f;

	[Header("Poise Damage")]
	public float PoiseDamage = 10f;
}
