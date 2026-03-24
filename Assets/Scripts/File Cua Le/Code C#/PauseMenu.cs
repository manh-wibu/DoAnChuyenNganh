using UnityEngine;
using UnityEngine.SceneManagement;
using Saus.CoreSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private PlayerRespawnManager respawnManager;
    [SerializeField] GameObject pauseMenu;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Home()
    {
        if (GameManager.instance != null)
            GameManager.instance.lastCheckpointPos = Vector3.zero;

        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }

    public void Play()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        audioManager.PlayClick();

        bool hasCheckpoint = PlayerPrefs.GetInt("HasCheckpoint", 0) == 1;

        if (!hasCheckpoint)
        {
            // ✓ New Game: Reset hết
            InventoryManager.Instance?.ClearInventory();
            GoldManager.Instance?.ResetGold();
            WeaponInventory.Instance?.ResetWeaponData();
            WeaponInventory.ClearPersistentWeaponData();
            Debug.Log("[PauseMenu] Restart without checkpoint → Reset all.");
        }
        else
        {
            // ✓ With Checkpoint: Restore từ checkpoint data
            InventoryManager.Instance?.LoadInventory();
            GoldManager.Instance?.LoadGold();
            // Weapons sẽ tự load từ PlayerPrefs trong Awake
            
            // Xóa chỉ item/gold của map hiện tại
            string currentMap = SceneManager.GetActiveScene().name;
            InventoryManager.Instance?.ClearMapInventory(currentMap);
            GoldManager.Instance?.ClearMapGold(currentMap);

            Debug.Log($"[PauseMenu] Restart with checkpoint → Restored from checkpoint.");
        }

        // TẢI LẠI SCENE
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
    public void OnRestartButtonPressed()
    {
        if (respawnManager != null)
        {
            respawnManager.ForceRespawnPlayer();
        }
    }
}