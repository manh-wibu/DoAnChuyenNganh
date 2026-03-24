using Saus.CoreSystem;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    private List<Entity> enemies = new List<Entity>();
    // Add dictionary to store enemy data (prefab and initial position)
    private Dictionary<Entity, (GameObject prefab, Vector3 initialPos)> enemyData = new Dictionary<Entity, (GameObject, Vector3)>();

    private void Awake()
    {
        // Ensure only one instance
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        // Get all Entity components in the scene
        enemies.AddRange(FindObjectsOfType<Entity>());
        // Initialize enemyData for existing enemies
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemyData[enemy] = (enemy.gameObject, enemy.transform.position);
            }
        }
    }

    // Method to register an enemy
    public void RegisterEnemy(GameObject enemyObject, GameObject originalPrefab)
    {
        if (enemyObject == null)
        {
            Debug.LogWarning("Cannot register: enemyObject is null.");
            return;
        }

        Entity entity = enemyObject.GetComponent<Entity>();
        if (entity != null && !enemies.Contains(entity))
        {
            enemies.Add(entity);
            // Store the prefab and initial position in enemyData
            enemyData[entity] = (originalPrefab, enemyObject.transform.position);
            Debug.Log($"Registered enemy: {enemyObject.name}");
        }
        else
        {
            Debug.LogWarning($"Cannot register enemy: Entity missing or already registered.", enemyObject);
        }
    }

    public void ResetAllEnemies()
    {
        List<Entity> newEnemies = new List<Entity>();

        foreach (var enemy in enemies)
        {
            // If enemy is destroyed, recreate from prefab
            if (enemy == null || enemy.gameObject == null)
            {
                if (enemyData.ContainsKey(enemy))
                {
                    var (prefab, initialPos) = enemyData[enemy];
                    if (prefab != null)
                    {
                        GameObject newEnemyObj = Instantiate(prefab, initialPos, Quaternion.identity);
                        Entity newEnemy = newEnemyObj.GetComponent<Entity>();
                        if (newEnemy != null)
                        {
                            newEnemies.Add(newEnemy);
                            enemyData[newEnemy] = (prefab, initialPos);
                            Debug.Log($"Recreated enemy: {newEnemyObj.name}");
                        }
                    }
                }
                continue;
            }

            // Reactivate GameObject
            if (!enemy.gameObject.activeSelf)
            {
                enemy.gameObject.SetActive(true);
                Debug.Log($"Reactivated {enemy.name}");
            }

            // Reset position
            //enemy.transform.position = enemy.initialPosition;

            // Reset state machine
            var enemy1 = enemy.GetComponent<Enemy1>();
            var enemy2 = enemy.GetComponent<Enemy2>();
            if (enemy1 != null && enemy.stateMachine != null)
            {
                enemy.stateMachine.ChangeState(enemy1.moveState);
            }
            else if (enemy2 != null && enemy.stateMachine != null)
            {
                enemy.stateMachine.ChangeState(enemy2.moveState);
            }
            else
            {
                Debug.LogWarning($"Cannot reset state machine for {enemy.name}: Enemy1 or Enemy2 not found.", enemy);
            }

            // Reset state variables
            //if (enemy.entityData != null)
            //{
            //    enemy.SetIsDead(false);
            //    enemy.SetIsStunned(false);
            //    enemy.SetHealth(enemy.entityData.maxHealth);
            //    enemy.SetStunResistance(enemy.entityData.stunResistance);
            //}
            //else
            //{
            //    Debug.LogWarning($"entityData for {enemy.name} is null.", enemy);
            //}
            //enemy.ResetLastDamageTime();

            // Reset Rigidbody2D
            var movement = enemy.GetComponentInChildren<Movement>();
            if (movement?.RB != null)
            {
                movement.RB.velocity = Vector2.zero;
                movement.RB.bodyType = RigidbodyType2D.Dynamic;
            }

            // Reset SpriteRenderer
            var sr = enemy.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.enabled = true;
            }

            // Reset Animator
            var animator = enemy.GetComponent<Animator>();
            if (animator != null)
            {
            }

            // Stop all coroutines
            enemy.StopAllCoroutines();

            // Call custom ResetState
            //enemy.ResetState();

            newEnemies.Add(enemy);
        }

        enemies = newEnemies;

        // Debug enemy list
        Debug.Log($"Enemy count after reset: {enemies.Count}");
        foreach (var enemy in enemies)
        {
            Debug.Log($"Enemy: {enemy.name}, Active: {enemy.gameObject.activeSelf}, Position: {enemy.transform.position}");
        }
    }

    // Optional: Remove enemy from list
    public void RemoveEnemy(Entity entity)
    {
        enemies.Remove(entity);
        enemyData.Remove(entity); // Also remove from enemyData
    }

    // 🆕 Kiểm tra xem còn enemy sống nào không
    public bool HasRemainingEnemies()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null && enemy.gameObject.activeSelf)
            {
                return true; // Còn enemy sống
            }
        }
        return false; // Hết enemy
    }
}