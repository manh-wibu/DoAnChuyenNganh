using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Saus.CoreSystem;  // 🆕 For WeaponInventory

public class Menu : MonoBehaviour
{
    [Header("UI")]
    public GameObject settingsPanel;
    public Button continueButton;

    private void Start()
    {
        bool hasCheckpoint = PlayerPrefs.GetInt("HasCheckpoint", 0) == 1;
        continueButton.gameObject.SetActive(hasCheckpoint);
    }

    // =================== NEW GAME ===================
    public void PlayGame()
    {
        Debug.Log("[Menu] NEW GAME");

        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("NextScene", "LV1");
        PlayerPrefs.SetInt("HasCheckpoint", 0);
        PlayerPrefs.Save();

        InventoryManager.Instance?.ClearInventory();
        GoldManager.Instance?.ResetGold();
        WeaponInventory.Instance?.ResetWeaponData();
        WeaponInventory.ClearPersistentWeaponData();

        SceneManager.LoadScene("LoadingScene");
    }

    // =================== CONTINUE ===================
    public void ContinueGame()
    {
        Debug.Log("====================================");
        Debug.Log("[Menu] CONTINUE GAME");
        Debug.Log("====================================");

        if (PlayerPrefs.GetInt("HasCheckpoint", 0) != 1)
        {
            PlayGame();
            return;
        }

        string checkpointMap = PlayerPrefs.GetString("CheckpointScene", "LV1");
        Debug.Log($"[Menu] Continue to map: {checkpointMap}");

        // ✓ Load lại từ checkpoint (items, gold, weapons) TRƯỚC khi load scene
        InventoryManager.Instance?.LoadInventory();
        GoldManager.Instance?.LoadGold();
        
        // ✓ QUAN TRỌNG: Nếu WeaponInventory đã tồn tại, restore weapons từ PlayerPrefs
        // Nếu chưa tồn tại, nó sẽ load tự động trong Awake
        if (WeaponInventory.Instance != null)
        {
            Debug.Log("[Menu] WeaponInventory exists, clearing map weapons");
            // Nếu WeaponInventory đã tồn tại, chỉ cần clear map hiện tại
            WeaponInventory.Instance.ClearMapWeapons(checkpointMap);
        }
        else
        {
            Debug.Log("[Menu] WeaponInventory not exists yet, it will load in Awake");
        }
        
        // ✓ Xóa chỉ item/gold của map checkpoint (giả lập reset khi respawn)
        InventoryManager.Instance?.ClearMapInventory(checkpointMap);
        GoldManager.Instance?.ClearMapGold(checkpointMap);

        PlayerPrefs.SetString("NextScene", checkpointMap);
        PlayerPrefs.Save();

        SceneManager.LoadScene("LoadingScene");
    }

    // =================== QUIT ===================
    public void QuitGame()
    {
        // ✓ Weapons đã được save tại checkpoint, không cần save lại
        // (Nếu save lại ở đây, nó sẽ save rỗng vì instance mới từ scene)
        
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
