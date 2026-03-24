using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;

    [Header("Input Slots - Sẽ tự tìm lại nếu bị null")]
    public CraftingSlot slotA;
    public CraftingSlot slotB;
    public CraftingOutputSlot outputSlot;

    public List<SO_CraftingRecipe> recipes = new List<SO_CraftingRecipe>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Tự động tìm slot nếu chưa gán
        FindCraftingSlots();
    }

    private void OnEnable()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged += TryPreviewResult;
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged -= TryPreviewResult;
    }

    // TỰ ĐỘNG TÌM LẠI SLOT KHI RESTART HOẶC MỞ LẠI CRAFTING
    void FindCraftingSlots()
    {
        if (slotA == null)
            slotA = GameObject.Find("SlotA")?.GetComponent<CraftingSlot>();
        if (slotB == null)
            slotB = GameObject.Find("SlotB")?.GetComponent<CraftingSlot>();
        if (outputSlot == null)
            outputSlot = GameObject.Find("OutputSlot")?.GetComponent<CraftingOutputSlot>();
    }

    public void PutItemToCraftingSlot(SO_Item item)
    {
        if (item == null) return;

        // TỰ ĐỘNG TÌM LẠI SLOT NẾU BỊ NULL (sau restart)
        if (slotA == null || slotB == null)
            FindCraftingSlots();

        if (slotA == null || slotB == null)
        {
            Debug.LogError("Crafting slots vẫn null sau khi tìm lại!");
            return;
        }

        if (slotA.storedItem == null)
            slotA.SetItem(item);
        else if (slotB.storedItem == null)
            slotB.SetItem(item);

        TryPreviewResult();
    }

    public void TryPreviewResult()
    {
        if (slotA == null || slotB == null || outputSlot == null) return;

        if (slotA.storedItem == null || slotB.storedItem == null)
        {
            outputSlot.Clear();
            return;
        }

        SO_Item result = GetResultPreview(slotA.storedItem, slotB.storedItem);
        if (result != null)
            outputSlot.ShowResult(result);
        else
            outputSlot.Clear();
    }

    private SO_Item GetResultPreview(SO_Item a, SO_Item b)
    {
        foreach (var r in recipes)
        {
            if (r.inputItems.Count != 2) continue;

            bool match = (r.inputItems[0].itemID == a.itemID && r.inputItems[1].itemID == b.itemID) ||
                         (r.inputItems[0].itemID == b.itemID && r.inputItems[1].itemID == a.itemID);

            if (match)
                return InventoryManager.Instance.GetItemByID(r.outputItemID); // <-- sửa ở đây
        }
        return null;
    }

    public bool CraftItem()
    {
        if (slotA == null || slotB == null) return false;
        if (slotA.storedItem == null || slotB.storedItem == null) return false;

        SO_Item result = GetResultPreview(slotA.storedItem, slotB.storedItem);
        if (result == null) return false;

        InventoryManager.Instance.RemoveItemByID(slotA.storedItem.itemID, 1); // <-- sửa ở đây
        InventoryManager.Instance.RemoveItemByID(slotB.storedItem.itemID, 1); // <-- sửa ở đây
        InventoryManager.Instance.AddItem(result, 1); // <-- sửa ở đây

        slotA.Clear();
        slotB.Clear();
        TryPreviewResult();

        return true;
    }
}
