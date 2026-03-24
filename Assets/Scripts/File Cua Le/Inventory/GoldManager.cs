using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class GoldEntry
{
    public string mapID;
    public int amount;
}

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    public event Action<int> OnGoldChanged;

    [Header("Gold Data")]
    public List<GoldEntry> goldList = new List<GoldEntry>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 🆕 Load gold from PlayerPrefs on startup (when returning from quit)
            if (PlayerPrefs.HasKey("GoldData"))
            {
                LoadGold();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Tổng vàng tất cả map
    public int CurrentGold => goldList.Sum(x => x.amount);

    // Lấy vàng theo map
    public int GetGold(string mapID = null)
    {
        if (string.IsNullOrEmpty(mapID))
            return CurrentGold;
        return goldList.FirstOrDefault(x => x.mapID == mapID)?.amount ?? 0;
    }

    // Thêm vàng vào map hiện tại (hoặc mapID)
    public void AddGold(int amount, string mapID = null)
    {
        if (amount <= 0) return;

        string currentMap = string.IsNullOrEmpty(mapID) ? SceneManager.GetActiveScene().name : mapID;
        var entry = goldList.FirstOrDefault(x => x.mapID == currentMap);
        if (entry != null)
        {
            entry.amount += amount;
        }
        else
        {
            goldList.Add(new GoldEntry { mapID = currentMap, amount = amount });
        }

        OnGoldChanged?.Invoke(CurrentGold);
        Debug.Log($"[GoldManager] Nhặt vàng: +{amount} (Map: {currentMap}) → Tổng: {CurrentGold}");
    }

    // Trừ vàng theo map hiện tại hoặc tổng
    public void SpendGold(int amount, string mapID = null)
    {
        if (amount <= 0) return;

        if (string.IsNullOrEmpty(mapID))
        {
            // Trừ tổng → ưu tiên trừ map hiện tại trước
            int remaining = amount;
            foreach (var entry in goldList.ToList())
            {
                if (entry.amount >= remaining)
                {
                    entry.amount -= remaining;
                    remaining = 0;
                }
                else
                {
                    remaining -= entry.amount;
                    entry.amount = 0;
                }
            }

            // Xóa entry 0
            goldList.RemoveAll(x => x.amount <= 0);
        }
        else
        {
            var entry = goldList.FirstOrDefault(x => x.mapID == mapID);
            if (entry == null) return;
            entry.amount -= amount;
            if (entry.amount <= 0) goldList.Remove(entry);
        }

        OnGoldChanged?.Invoke(CurrentGold);
        Debug.Log($"[GoldManager] Trừ vàng: -{amount} (Map: {mapID ?? "Tổng"}) → Tổng: {CurrentGold}");
    }

    // Reset toàn bộ vàng (New Game)
    public void ResetGold()
    {
        goldList.Clear();
        OnGoldChanged?.Invoke(CurrentGold);
        Debug.Log("[GoldManager] RESET vàng toàn bộ!");
    }

    // Reset vàng map hiện tại (ví dụ Continue)
    public void ClearMapGold(string mapID)
    {
        if (string.IsNullOrEmpty(mapID)) return;
        goldList.RemoveAll(x => x.mapID == mapID);
        OnGoldChanged?.Invoke(CurrentGold);
        Debug.Log($"[GoldManager] Xóa vàng map {mapID} khi Continue.");
    }

    // 🆕 Save gold to PlayerPrefs
    public void SaveGold()
    {
        string json = JsonUtility.ToJson(new GoldData { goldList = goldList });
        PlayerPrefs.SetString("GoldData", json);
        PlayerPrefs.Save();
        Debug.Log("[GoldManager] Saved gold data.");
    }

    // 🆕 Load gold from PlayerPrefs
    public void LoadGold()
    {
        if (!PlayerPrefs.HasKey("GoldData"))
        {
            Debug.Log("[GoldManager] No saved gold data found.");
            return;
        }

        try
        {
            string json = PlayerPrefs.GetString("GoldData");
            GoldData data = JsonUtility.FromJson<GoldData>(json);
            goldList = data.goldList ?? new List<GoldEntry>();
            OnGoldChanged?.Invoke(CurrentGold);
            Debug.Log("[GoldManager] Loaded gold data.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[GoldManager] Error loading gold: {ex.Message}");
        }
    }

    // 🆕 Helper class for JSON serialization
    [Serializable]
    private class GoldData
    {
        public List<GoldEntry> goldList;
    }
}
