using UnityEngine;
using Saus.CoreSystem;
using System.Collections.Generic;
using System.Collections;

public class ObstacleDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private float flashDuration = 0.2f; // Thời gian mỗi lần chớp
    [SerializeField] private int flashCount = 5; // Số lần chớp

    // ✓ Track cooldown riêng cho từng player (key = player instance, value = last damage time)
    private Dictionary<PlayerHealth, float> playerLastDamageTime = new Dictionary<PlayerHealth, float>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ✓ Kiểm tra xem collision object có tag "Player" không
        if (!collision.CompareTag("Player"))
            return;

        // ✓ Lấy PlayerHealth
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth == null)
            return;

        // ✓ Kiểm tra cooldown riêng cho player này
        if (!playerLastDamageTime.ContainsKey(playerHealth))
        {
            playerLastDamageTime[playerHealth] = -999f;
        }

        float timeSinceLastDamage = Time.time - playerLastDamageTime[playerHealth];
        if (timeSinceLastDamage < damageCooldown)
            return;

        // ✓ Gây damage
        playerHealth.TakeDamage(damageAmount);
        playerLastDamageTime[playerHealth] = Time.time;
        Debug.Log($"[ObstacleDamage] Player hit! Dealt {damageAmount} damage.");

        // ✓ Hiệu ứng chợp tắt
        StartCoroutine(FlashPlayer(collision.gameObject));

        // ✓ Kiểm tra xem player hết máu không (check after taking damage)
        // Schedule death handling for next frame
        if (playerHealth.GetCurrentHealth() <= 0)
        {
            HandlePlayerDeath(playerHealth, collision.gameObject);
        }
    }

    private void HandlePlayerDeath(PlayerHealth playerHealth, GameObject playerObject)
    {
        Debug.Log("[ObstacleDamage] Player died from obstacle! Resetting...");

        // ✓ Mất hết item
        InventoryManager.Instance?.ClearInventory();
        Debug.Log("[ObstacleDamage] Inventory cleared!");

        // ✓ Mất hết vũ khí
        WeaponInventory.Instance?.ResetWeaponData();
        WeaponInventory.ClearPersistentWeaponData();
        Debug.Log("[ObstacleDamage] Weapons cleared!");

        // ✓ Teleport về spawn point (use startPoint if available)
        PlayerStartPosition spawnManager = FindObjectOfType<PlayerStartPosition>();
        if (spawnManager != null && spawnManager.startPoint != null)
        {
            playerObject.transform.position = spawnManager.startPoint.position;
            Debug.Log($"[ObstacleDamage] Player teleported to spawn: {spawnManager.startPoint.position}");
        }
        else
        {
            Debug.LogWarning("[ObstacleDamage] PlayerStartPosition or startPoint NOT found!");
        }

        // ✓ Reset health về max
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(-(int)playerHealth.GetCurrentHealth()); // Negative damage to heal
            Debug.Log("[ObstacleDamage] Health reset!");
        }
    }

    // ✓ Hiệu ứng chợp tắt (flash) khi bị damage
    private IEnumerator FlashPlayer(GameObject playerObject)
    {
        SpriteRenderer sr = playerObject.GetComponent<SpriteRenderer>();
        if (sr == null)
            yield break;

        Color originalColor = sr.color;
        Color flashColor = new Color(1f, 1f, 1f, 0.5f); // Màu trắng bán trong suốt

        for (int i = 0; i < flashCount; i++)
        {
            sr.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            sr.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }

        sr.color = originalColor;
        Debug.Log("[ObstacleDamage] Flash effect finished!");
    }
}
