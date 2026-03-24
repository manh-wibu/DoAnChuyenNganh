/*using UnityEngine;
using Saus.CoreSystem;
using System.Collections;

public class PlayerDeathHandler : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private Stats stats;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        stats = GetComponentInChildren<Stats>();
    }

    // Hàm này được gọi từ Death.cs
    public void HandleDeath()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        // Chờ Death.cs tắt Player
        yield return new WaitForSeconds(0.1f);

        // Bật lại Player
        gameObject.SetActive(true);

        // Reset position
        playerHealth?.RespawnAtCheckpoint();

        // Reset HP
        if (stats != null)
            stats.Health.CurrentValue = stats.Health.MaxValue;

        // Update UI
        if (playerHealth != null && playerHealth.healthBar != null)
            playerHealth.healthBar.UpdateBar(
                (int)stats.Health.CurrentValue,
                (int)stats.Health.MaxValue
            );

        // Bật lại Rigidbody2D
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.bodyType = RigidbodyType2D.Dynamic;

        // Bật lại Collider
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = true;

        // Bật lại Movement
        PlayerMovement movement = GetComponentInChildren<PlayerMovement>();
        if (movement != null)
            movement.enabled = true;

        Debug.Log("Player respawn handled.");
    }
}
*/