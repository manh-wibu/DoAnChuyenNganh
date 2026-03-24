using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Saus.CoreSystem;

public class FinishPoint : MonoBehaviour
{
    private AudioManager audioManager;
    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (collision.CompareTag("Player"))
        {
            // 🆕 Kiểm tra còn enemy không
            if (EnemyManager.instance != null && EnemyManager.instance.HasRemainingEnemies())
            {
                Debug.Log("[FinishPoint] Còn enemy trên map. Không thể chuyển level!");
                return;
            }

            // Lưu weapon trước khi qua scene mới
            WeaponInventory weaponInventory = collision.GetComponentInParent<WeaponInventory>();
            if (weaponInventory != null && WeaponSaveManager.Instance != null)
            {
                WeaponSaveManager.Instance.SaveWeapons(weaponInventory);
                Debug.Log("[FinishPoint] Đã lưu weapon trước khi qua level mới.");
            }

            // 🆕 Save player health trước khi qua level mới
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.SaveHealth();
                Debug.Log("[FinishPoint] Đã lưu health trước khi qua level mới.");
            }

            UnlockNewLevel();
            GoToNextLevel();
            audioManager.PlayNextLevelSound();
        }    
    }
    void UnlockNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }

    public void GoToNextLevel()
{
    int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

    if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
    {
        // Xóa checkpoint của map cũ
        //PlayerPrefs.DeleteKey("CheckpointX");
        //PlayerPrefs.DeleteKey("CheckpointY");
        //PlayerPrefs.DeleteKey("CheckpointZ");
        //PlayerPrefs.DeleteKey("CheckpointScene");
        //PlayerPrefs.DeleteKey("HasCheckpoint");
        PlayerPrefs.Save();

        string nextScenePath = SceneUtility.GetScenePathByBuildIndex(nextSceneIndex);
        string nextSceneName = Path.GetFileNameWithoutExtension(nextScenePath);

        PlayerPrefs.SetString("NextScene", nextSceneName);
        SceneManager.LoadScene("LoadingScene");
    }
}

}
