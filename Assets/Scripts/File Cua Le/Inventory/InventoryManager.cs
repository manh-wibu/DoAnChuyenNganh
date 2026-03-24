using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class InventoryEntry
{
    public string itemID;
    public int count;
    public string mapID; // map mà item được nhặt
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory Data")]
    public List<InventoryEntry> items = new List<InventoryEntry>();
    public List<SO_Item> itemDatabase = new List<SO_Item>();

    public event Action OnInventoryChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 🆕 Load inventory from PlayerPrefs on startup (when returning from quit)
        if (PlayerPrefs.HasKey("InventoryData"))
        {
            LoadInventory();
        }
    }

    // ==========================
    // Lấy item theo ID
    // ==========================
    public SO_Item GetItemByID(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        return itemDatabase.Find(i => i.itemID == id);
    }

    // ==========================
    // Lấy tổng count (tất cả map nếu mapID=null)
    // ==========================
    public int GetItemCountByID(string id, string mapID = null)
    {
        if (string.IsNullOrEmpty(id)) return 0;
        if (string.IsNullOrEmpty(mapID))
            return items.Where(x => x.itemID == id).Sum(x => x.count);
        else
            return items.Where(x => x.itemID == id && x.mapID == mapID).Sum(x => x.count);
    }

    // ==========================
    // Thêm item
    // ==========================
    public bool AddItem(SO_Item item, string mapID, int amount = 1)
    {
        if (item == null || amount <= 0) return false;

        InventoryEntry existing = null;
        if (item.stackable)
            existing = items.FirstOrDefault(x => x.itemID == item.itemID && x.count < item.maxStack && x.mapID == mapID);

        if (existing != null)
        {
            existing.count = Mathf.Min(existing.count + amount, item.maxStack);
        }
        else
        {
            items.Add(new InventoryEntry
            {
                itemID = item.itemID,
                mapID = mapID,
                count = amount
            });
        }

        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool AddItem(SO_Item item, int amount = 1)
    {
        string currentMap = SceneManager.GetActiveScene().name;
        return AddItem(item, currentMap, amount);
    }

    // ==========================
    // Xóa item theo map hoặc tổng hợp map
    // ==========================
    public bool RemoveItemByID(string id, int amount = 1, string mapID = null)
    {
        if (string.IsNullOrEmpty(id) || amount <= 0) return false;

        if (!string.IsNullOrEmpty(mapID))
        {
            var entry = items.Find(x => x.itemID == id && x.mapID == mapID);
            if (entry != null)
            {
                entry.count -= amount;
                if (entry.count <= 0) items.Remove(entry);
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        // Nếu map hiện tại không có, tự trừ map khác
        var otherEntry = items.Find(x => x.itemID == id);
        if (otherEntry != null)
        {
            otherEntry.count -= amount;
            if (otherEntry.count <= 0) items.Remove(otherEntry);
            OnInventoryChanged?.Invoke();
            return true;
        }

        return false; // không còn item
    }

    // ==========================
    // Dùng item
    // ==========================
    public void UseItem(SO_Item item, GameObject target)
    {
        if (item == null || target == null) return;
        string currentMap = SceneManager.GetActiveScene().name;
        item.Use(target);
        RemoveItemByID(item.itemID, 1, currentMap);
    }

    // ==========================
    // Clear inventory
    // ==========================
    public void ClearInventory()
    {
        items.Clear();
        OnInventoryChanged?.Invoke();
    }

    public void ClearMapInventory(string mapID)
    {
        if (string.IsNullOrEmpty(mapID)) return;
        items.RemoveAll(x => x.mapID == mapID);
        OnInventoryChanged?.Invoke();
    }

    // 🆕 Save inventory to PlayerPrefs
    public void SaveInventory()
    {
        string json = JsonUtility.ToJson(new InventoryData { items = items });
        PlayerPrefs.SetString("InventoryData", json);
        PlayerPrefs.Save();
        Debug.Log("[InventoryManager] Saved inventory data.");
    }

    // 🆕 Load inventory from PlayerPrefs
    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey("InventoryData"))
        {
            Debug.Log("[InventoryManager] No saved inventory data found.");
            return;
        }

        try
        {
            string json = PlayerPrefs.GetString("InventoryData");
            InventoryData data = JsonUtility.FromJson<InventoryData>(json);
            items = data.items ?? new List<InventoryEntry>();
            OnInventoryChanged?.Invoke();
            Debug.Log("[InventoryManager] Loaded inventory data.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[InventoryManager] Error loading inventory: {ex.Message}");
        }
    }

    // 🆕 Helper class for JSON serialization
    [Serializable]
    private class InventoryData
    {
        public List<InventoryEntry> items;
    }
}

