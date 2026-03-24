using UnityEngine;

/// <summary>
/// Data for Boss AOE Attack
/// Configuration for area-of-effect attack damage and knockback
/// </summary>
[CreateAssetMenu(fileName = "D_BossAOEAttack", menuName = "Data/State Data/AOE Attack Data")]
public class D_AOEAttackState : ScriptableObject
{
	[Header("AOE Settings")]
	public float aoeRadius = 3f;
	public float damage = 15f;
	public float knockbackStrength = 8f;
	public Vector2 knockbackAngle = Vector2.up;

	[Header("Detection")]
	public LayerMask whatIsPlayer;
}
