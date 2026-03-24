using Saus.ProjectileSystem;
using Saus.ProjectileSystem.DataPackages;
using UnityEngine;

/// <summary>
/// Data configuration for Enemy Ranged Attack State.
/// This integrates with the ProjectileSystem using DataPackages (same as Player weapons).
/// Senior Dev Note: By using this data-driven approach, different enemies can have
/// completely different projectile behaviors without code changes.
/// </summary>
[CreateAssetMenu(fileName = "newRangedAttackStateData", menuName = "Data/State Data/Ranged Attack State")]
public class D_RangedAttackState : ScriptableObject
{
    [Header("Projectile Configuration")]
    public Projectile projectilePrefab;
    
    [Header("Projectile Data Packages")]
    public DamageDataPackage damageData;
    public KnockBackDataPackage knockBackData;
    public PoiseDamageDataPackage poiseDamageData;
    
    [Header("Legacy Settings (Optional)")]
    [Tooltip("Kept for backwards compatibility. Use dataPackages above instead.")]
    public float projectileDamage = 10f;
    public float projectileSpeed = 12f;
    public float projectileTravelDistance;
}

