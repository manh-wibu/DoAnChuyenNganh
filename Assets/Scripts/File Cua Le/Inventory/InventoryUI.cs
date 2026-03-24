using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("=== CÀI ĐẶT INVENTORY ===")]
    public GameObject slotPrefab;          // Prefab Slot của mày
    public Transform slotParent;           // Kéo SlotParent (empty object) vào đây
    public GameObject inventoryPanel;      // Panel chính chứa inventory + crafting

    [Header("Số lượng ô")]
    [Range(0, 60)]
    public int slotCount = 9;             // MÀY MUỐN BAO NHIÊU THÌ SET Ở ĐÂY TRÊN INSPECTOR

    private List<InventorySlot> slots = new List<InventorySlot>();
    private bool slotsCreated = false;

    private void Awake()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
    }

    private void Start()
    {
        CreateSlotsOnce();
        SubscribeToInventory();
        Refresh();
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged -= Refresh;
    }

    void CreateSlotsOnce()
    {
        // ĐÃ TẠO RỒI → THOÁT, KHÔNG TẠO LẠI NỮA
        if (slotsCreated || slotPrefab == null || slotParent == null) return;

        // XÓA HẾT SLOT CŨ NẾU CÓ (khi reload scene)
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        slots.Clear();

        slots.Clear();

        // TẠO ĐÚNG SỐ LƯỢNG MÀ MÀY SET TRÊN INSPECTOR
        for (int i = 0; i < slotCount; i++)
        {
            GameObject obj = Instantiate(slotPrefab, slotParent);
            obj.name = $"Slot_{i}"; // đặt tên cho dễ nhìn
            var slotComp = obj.GetComponent<InventorySlot>();
            if (slotComp != null)
                slots.Add(slotComp);
        }

        slotsCreated = true;
        Debug.Log($"[InventoryUI] Đã tạo {slotCount} slot inventory (một lần duy nhất)");
    }

    void SubscribeToInventory()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged += Refresh;
    }

    public void Refresh()
    {
        if (InventoryManager.Instance == null) return;

        // --- Gom item theo ID ---
        Dictionary<string, int> merged = new Dictionary<string, int>();

        foreach (var entry in InventoryManager.Instance.items)
        {
            if (!merged.ContainsKey(entry.itemID))
                merged[entry.itemID] = 0;

            merged[entry.itemID] += entry.count;
        }

        // --- Hiển thị lên các slot ---
        int index = 0;
        foreach (var pair in merged)
        {
            if (index >= slots.Count) break;

            var itemData = InventoryManager.Instance.GetItemByID(pair.Key);
            slots[index].Setup(itemData, pair.Value);

            index++;
        }

        // Clear các slot còn dư
        for (; index < slots.Count; index++)
            slots[index].Setup(null, 0);
    }

    // === MỞ/ĐÓNG INVENTORY ===
    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        Time.timeScale = 0f;
        Refresh();
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ToggleInventory()
    {
        if (inventoryPanel.activeSelf)
            CloseInventory();
        else
            OpenInventory();
    }
}