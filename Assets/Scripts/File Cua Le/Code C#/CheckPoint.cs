using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saus.CoreSystem;  // 🆕 For WeaponInventory

public class Checkpoint : MonoBehaviour
{
    public Transform respawnPoint;
    SpriteRenderer spriteRenderer;
    public Sprite passive, active;
    Collider2D coll;
    private AudioManager audioManager;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.UpdateCheckpoint(respawnPoint.position);
                spriteRenderer.sprite = active;
                coll.enabled = false;
                audioManager.CheckPointSource();

                // 🆕 Save inventory and checkpoint data
                string currentMap = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                PlayerPrefs.SetString("CheckpointScene", currentMap);
                PlayerPrefs.SetInt("HasCheckpoint", 1);
                InventoryManager.Instance?.SaveInventory();
                GoldManager.Instance?.SaveGold();
                WeaponInventory.Instance?.SaveWeapons();
                playerHealth.SaveHealth();  // 🆕 Also save health
                PlayerPrefs.Save();
                Debug.Log("[Checkpoint] Saved checkpoint, inventory, gold, weapons and health data.");
            }
        }
    }
}