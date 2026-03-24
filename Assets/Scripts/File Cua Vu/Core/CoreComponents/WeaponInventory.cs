using System;
using Saus.Weapons;
using UnityEngine;

namespace Saus.CoreSystem
{
    public class WeaponInventory : CoreComponent
    {
        public event Action<int, WeaponDataSO> OnWeaponDataChanged;
        public static WeaponInventory Instance { get; private set; }
        [field: SerializeField] public WeaponDataSO[] weaponData { get; private set; }

        // Static data sẽ được giữ lại giữa các scene
        private static WeaponDataSO[] persistentWeaponData;

        // 🆕 Store weapon names for PlayerPrefs serialization
        private static string[] weaponNames = new string[2]; // Assuming 2 weapon slots

        protected override void Awake()
        {
            base.Awake();

            // ✓ Nếu đã có instance, destroy cái mới
            if (Instance != null && Instance != this)
            {
                Debug.Log($"[WeaponInventory] Instance already exists! Destroying new one: {gameObject.name}");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            Debug.Log($"[WeaponInventory] Awake: weaponData length={weaponData?.Length ?? 0}");
            Debug.Log($"[WeaponInventory] Awake: weaponData[0]={weaponData?[0]}, weaponData[1]={weaponData?[1]}");

            // ✓ Priority 1: Restore from persistent storage (scene transition - fastest)
            if (persistentWeaponData != null && persistentWeaponData.Length == weaponData.Length)
            {
                for (int i = 0; i < persistentWeaponData.Length; i++)
                {
                    weaponData[i] = persistentWeaponData[i];
                    OnWeaponDataChanged?.Invoke(i, persistentWeaponData[i]);
                }
                Debug.Log("[WeaponInventory] Restored weapons from persistent storage.");
            }
            else
            {
                Debug.Log("[WeaponInventory] No persistent storage available - will try PlayerPrefs in Start()");
            }

            Debug.Log($"[WeaponInventory] Awake complete: {gameObject.name}");
        }

        private void Start()
        {
            // ✓ Delay load từ PlayerPrefs tới Start, khi WeaponDatabase đã khởi tạo
            if (weaponData != null && persistentWeaponData == null)
            {
                Debug.Log("[WeaponInventory] Start: Trying to load from PlayerPrefs (delayed from Awake)");
                StartCoroutine(LoadWeaponsWhenDatabaseReady());
            }
        }

        private System.Collections.IEnumerator LoadWeaponsWhenDatabaseReady()
        {
            // ✓ Chờ WeaponDatabase khởi tạo (tối đa 5 giây)
            float timeout = 5f;
            float elapsed = 0f;
            
            while (Saus.Weapons.WeaponDatabase.Instance == null && elapsed < timeout)
            {
                elapsed += Time.deltaTime;
                yield return new WaitForSeconds(0.1f);
            }
            
            if (Saus.Weapons.WeaponDatabase.Instance == null)
            {
                Debug.LogError("[WeaponInventory] WeaponDatabase never initialized after 5 seconds!");
                yield break;
            }
            
            Debug.Log("[WeaponInventory] WeaponDatabase is now ready, loading weapons from PlayerPrefs");
            LoadWeaponsFromPlayerPrefs();
        }

        private void OnDestroy()
        {
            // Lưu lại weaponData vào static trước khi bị destroy
            if (weaponData != null && weaponData.Length > 0)
            {
                persistentWeaponData = new WeaponDataSO[weaponData.Length];
                System.Array.Copy(weaponData, persistentWeaponData, weaponData.Length);
                Debug.Log("[WeaponInventory] Đã lưu weapon data vào persistent storage trước khi destroy.");
            }
        }
        public bool TrySetWeapon(WeaponDataSO newData, int index, out WeaponDataSO oldData)
        {
            if (index >= weaponData.Length)
            {
                oldData = null;
                return false;
            }

            oldData = weaponData[index];
            weaponData[index] = newData;

            OnWeaponDataChanged?.Invoke(index, newData);
            
            Debug.Log($"[WeaponInventory] TrySetWeapon[{index}]: {newData?.Name ?? "null"}");

            return true;
        }

        public bool TryGetWeapon(int index, out WeaponDataSO data)
        {
            if (index >= weaponData.Length)
            {
                data = null;
                return false;
            }

            data = weaponData[index];
            return true;
        }

        public bool TryGetEmptyIndex(out int index)
        {
            for (var i = 0; i < weaponData.Length; i++)
            {
                if (weaponData[i] is not null)
                    continue;

                index = i;
                return true;
            }

            index = -1;
            return false;
        }

        public WeaponSwapChoice[] GetWeaponSwapChoices()
        {
            var choices = new WeaponSwapChoice[weaponData.Length];

            for (var i = 0; i < weaponData.Length; i++)
            {
                var data = weaponData[i];

                choices[i] = new WeaponSwapChoice(data, i);
            }

            return choices;
        }

        public void ResetWeaponData()
        {
            for (var i = 0; i < weaponData.Length; i++)
            {
                weaponData[i] = null;
            }
            // 🆕 Xóa persistent data để không khôi phục lại khi chuyển scene
            persistentWeaponData = null;
            Debug.Log("[WeaponInventory] Weapon data reset to null.");
        }

        // ✓ Clear weapons when respawning at checkpoint
        public void ClearMapWeapons(string mapID)
        {
            // For now, we don't track map ID for weapons like we do for items
            // Just don't clear weapons - they're global
            Debug.Log($"[WeaponInventory] ClearMapWeapons({mapID}) - weapons are global, not clearing");
        }

        // 🆕 Static method để clear persistent data từ bên ngoài (menu)
        public static void ClearPersistentWeaponData()
        {
            persistentWeaponData = null;
            Debug.Log("[WeaponInventory] Cleared persistent weapon data.");
        }

        // 🆕 Save weapons to PlayerPrefs
        public void SaveWeapons()
        {
            Debug.Log($"[WeaponInventory] SaveWeapons called. weaponData={weaponData}");
            if (weaponData != null)
                Debug.Log($"[WeaponInventory] weaponData.Length={weaponData.Length}, [0]={weaponData[0]}, [1]={weaponData[1]}");
            
            for (int i = 0; i < weaponData.Length; i++)
            {
                string weaponName = weaponData[i] != null ? weaponData[i].Name : "";
                PlayerPrefs.SetString($"WeaponData_{i}", weaponName);
                Debug.Log($"[WeaponInventory] SaveWeapons[{i}]: '{weaponName}' (weapon={weaponData[i]})");
            }
            PlayerPrefs.Save();
            Debug.Log("[WeaponInventory] Saved weapons to PlayerPrefs.");
        }


        // 🆕 Load weapons from PlayerPrefs - returns true if loaded, false if nothing in PlayerPrefs
        private bool LoadWeaponsFromPlayerPrefs()
        {
            if (weaponData == null || weaponData.Length == 0)
            {
                Debug.LogWarning("[WeaponInventory] weaponData is null or empty!");
                return false;
            }

            bool hasAnyWeapon = false;
            for (int i = 0; i < weaponData.Length; i++)
            {
                string weaponName = PlayerPrefs.GetString($"WeaponData_{i}", "");
                Debug.Log($"[WeaponInventory] LoadWeaponsFromPlayerPrefs[{i}]: weaponName='{weaponName}'");
                
                if (string.IsNullOrEmpty(weaponName))
                {
                    Debug.Log($"[WeaponInventory] Slot {i} is empty, skipping");
                    weaponData[i] = null;
                }
                else
                {
                    WeaponDataSO weapon = null;
                    
                    // ✓ Priority 1: Find from WeaponDatabase
                    Debug.Log($"[WeaponInventory] Trying WeaponDatabase...");
                    if (Saus.Weapons.WeaponDatabase.Instance != null)
                    {
                        Debug.Log($"[WeaponInventory] WeaponDatabase.Instance EXISTS. allWeapons.Count={Saus.Weapons.WeaponDatabase.Instance.allWeapons.Count}");
                        foreach (var w in Saus.Weapons.WeaponDatabase.Instance.allWeapons)
                        {
                            if (w != null)
                                Debug.Log($"  - Database weapon: '{w.Name}'");
                        }
                        
                        weapon = Saus.Weapons.WeaponDatabase.Instance.allWeapons
                            .Find(w => w != null && w.Name == weaponName);
                        if (weapon != null)
                        {
                            Debug.Log($"[WeaponInventory] ✓ Found in WeaponDatabase: {weaponName}");
                        }
                        else
                        {
                            Debug.LogWarning($"[WeaponInventory] ✗ NOT found in WeaponDatabase: {weaponName}");
                        }
                    }
                    else
                    {
                        Debug.LogError("[WeaponInventory] WeaponDatabase.Instance is NULL!");
                    }
                    
                    if (weapon != null)
                    {
                        weaponData[i] = weapon;
                        OnWeaponDataChanged?.Invoke(i, weapon);
                        hasAnyWeapon = true;
                        Debug.Log($"[WeaponInventory] ✓✓ Loaded weapon[{i}]: {weaponName}");
                    }
                    else
                    {
                        Debug.LogError($"[WeaponInventory] ✗✗ FAILED to load weapon[{i}]: {weaponName}");
                        weaponData[i] = null;
                    }
                }
            }
            Debug.Log($"[WeaponInventory] LoadWeaponsFromPlayerPrefs RESULT: hasAnyWeapon={hasAnyWeapon}");
            return hasAnyWeapon;
        }
    }
}