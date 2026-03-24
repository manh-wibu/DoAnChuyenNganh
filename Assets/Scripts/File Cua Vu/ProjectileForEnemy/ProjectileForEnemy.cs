
using Saus.ObjectPoolSystem;
using Saus.ProjectileSystem;
using UnityEngine;

namespace Saus.Projectiles
{
	/// <summary>
	/// Wrapper class for spawning projectiles for Enemy ranged attacks.
	/// This uses the ObjectPooling system for efficiency, just like the Player's ProjectileSpawner.
	/// Senior Dev Note: This approach is more performant than Instantiate() and integrates seamlessly
	/// with the Player's ProjectileSystem using the same DataPackage architecture.
	/// </summary>
	public class ProjectileForEnemy : MonoBehaviour
	{
		[Header("Projectile Prefab")]
		public Projectile projectilePrefab;

		[Header("Shoot Settings")]
		public Transform firePoint;

		// ObjectPool for efficient projectile spawning
		private readonly ObjectPools objectPools = new ObjectPools();

		public void Fire()
		{
			if (projectilePrefab == null || firePoint == null)
				return;

			SpawnProjectileFromPool();
		}

		/// <summary>
		/// Spawns a projectile using ObjectPooling (efficient and reusable)
		/// </summary>
		private void SpawnProjectileFromPool()
		{
			// Get projectile from pool
			Projectile proj = objectPools.GetObject(projectilePrefab);
			
			// Set position and rotation
			proj.transform.position = firePoint.position;
			proj.transform.rotation = firePoint.rotation;
			
			// Reset projectile state
			proj.Reset();

			// Initialize the projectile
			// Note: DataPackages should be sent via RangedAttackState.TriggerAttack()
			// This method is kept for backwards compatibility
			proj.Init();
		}
	}
}
