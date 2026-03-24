/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Saus.CoreSystem;

public class CraftingPanel : MonoBehaviour
{
    public static CraftingPanel Instance;

    [Header("Slot để ghép")]
    public CraftingSlot slotA;
    public CraftingSlot slotB;

    [Header("Manager")]
    public CraftingManager craftingManager;

    [Header("Middle Panel UI")]
    public GameObject middlePanel;
    public Image bigIcon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI description;

    [Header("Output Slot UI")]
    public CraftingOutputSlot outputSlot;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false); 
    }

    public void Open()
    {
        ClearSlots();
        gameObject.SetActive(true);
        if (middlePanel != null)
            middlePanel.SetActive(false);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void AssignItemToSlot(SO_Item item)
    {
        if (item == null) return;

        if (middlePanel != null)
            middlePanel.SetActive(true);

        if (bigIcon != null && item.icon != null)
            bigIcon.sprite = item.icon;

        if (itemName != null)
            itemName.text = item.itemName;

        if (description != null)
            description.text = item.description;

        CraftingSlot targetSlot = null;

        if (slotA.storedItem == null)
            targetSlot = slotA;
        else if (slotB.storedItem == null)
            targetSlot = slotB;

        if (targetSlot != null)
        {
            targetSlot.SetItem(item);
            Debug.Log($"Đã gán {item.itemName} vào {targetSlot.name}");
        }

        UpdateOutputSlot();
    }

    public void ClearSlots()
    {
        slotA?.Clear();
        slotB?.Clear();
        UpdateMiddlePanel();
        UpdateOutputSlot();
    }

    // =============================
    // ⭐⭐ QUAN TRỌNG — FIX ẤN CRAFT ⭐⭐
    // =============================
    public void OnCraftButton()
    {
        if (slotA.storedItem == null || slotB.storedItem == null)
        {
            Debug.Log("Thiếu nguyên liệu!");
            return;
        }

        List<string> inputIDs = new List<string>()
        {
            slotA.storedItem.itemID,
            slotB.storedItem.itemID
        };

        craftingManager.TryCraftByInput(inputIDs);

        // Xóa Item trong Slot
        slotA.Clear();
        slotB.Clear();
        outputSlot.Clear();

        // ❗ GIỮ LẠI description + name
        // ❗ KHÔNG ẨN middlePanel
        // ❗ Chỉ xóa icon
        if (bigIcon != null)
            bigIcon.sprite = null;
    }

    public void UpdateMiddlePanel()
    {
        SO_Item displayItem = slotA.storedItem ?? slotB.storedItem;

        if (displayItem != null)
        {
            middlePanel.SetActive(true);
            bigIcon.sprite = displayItem.icon;
            itemName.text = displayItem.itemName;
            description.text = displayItem.description;
        }
        else
        {
            if (bigIcon != null)
            bigIcon.sprite = null;
        }
    }

    public void UpdateOutputSlot()
    {
        if (slotA.storedItem != null && slotB.storedItem != null)
        {
            List<string> inputIDs = new List<string>()
            {
                slotA.storedItem.itemID,
                slotB.storedItem.itemID
            };

            SO_Item resultItem = craftingManager.GetCraftResult(inputIDs);
            outputSlot.SetItem(resultItem);
        }
        else
        {
            outputSlot.Clear();
        }
    }
}*/
