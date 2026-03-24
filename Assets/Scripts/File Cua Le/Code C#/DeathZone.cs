using Saus.CoreSystem;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Chơi âm thanh chết
                if (audioManager != null)
                    audioManager.DeathSource();

                var respawnManager = FindAnyObjectByType<PlayerRespawnManager>();
                if (respawnManager != null)
                {
                    respawnManager.ForceRespawnPlayer();
                }
                else
                {
                    Debug.LogError("[DeathZone] Không tìm thấy PlayerRespawnManager!");
                }
                }
        }
    }
}
