using UnityEngine;
using UnityEngine.SceneManagement;
using Saus.CoreSystem;
using Saus.Weapons;
public class WeaponSaveManager : MonoBehaviour
{
    public static WeaponSaveManager Instance;

    // Dữ liệu vũ khí đã lưu giữa các map
    private WeaponDataSO[] savedWeapons;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Khởi tạo mảng lưu weapon
        savedWeapons = new WeaponDataSO[2]; // số slot tùy game mày (2–3 slot)
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Khi scene load, tìm player và hồi lại weapon đã lưu
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySavedWeaponsToNewPlayer();
    }

    public void SaveWeapons(WeaponInventory inv)
    {
        if (inv.weaponData == null) return;

        for (int i = 0; i < savedWeapons.Length; i++)
        {
            savedWeapons[i] = inv.weaponData[i];
        }

        Debug.Log("[WeaponSaveManager] Đã LƯU weapon từ player.");
    }

    public void ApplySavedWeaponsToNewPlayer()
    {
        WeaponInventory inv = FindObjectOfType<WeaponInventory>();
        if (inv == null)
        {
            Debug.LogWarning("[WeaponSaveManager] Không tìm thấy WeaponInventory trong scene.");
            return;
        }

        // Reset weapon data của player mới
        inv.ResetWeaponData();

        // Khôi phục lại saved weapons
        for (int i = 0; i < savedWeapons.Length && i < inv.weaponData.Length; i++)
        {
            if (savedWeapons[i] != null)
            {
                inv.TrySetWeapon(savedWeapons[i], i, out _);
            }
        }

        Debug.Log("[WeaponSaveManager] Đã GÁN LẠI weapon cho player trong map mới.");
    }
}
